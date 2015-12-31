using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Singleton pattern
    public static MenuManager instance = null;

    // Variables for Pause menu
    private bool paused;
    public GameObject mPauseCanvas;
    // Audio files
    public AudioClip menuOpen;
    public AudioClip itemHover;

    // Variables for Player HUD
    public GameObject mHUD;
    public GameObject mTimerTextObj;


        // Title references
    private GameObject mMainPanel;
    private GameObject mCreditsPanel;
    private GameObject mInstructionsPanel;
        // Campfire references
    private Quest mQuest;
    Objective mFirstObjective;
    Objective mSecondObjective;
    private GameObject mQuestPanel;
    private GameObject mAllocPanel;
        // Quest references
    private GameObject mQuestTitleText;
    private GameObject mDescriptionText;
    private GameObject mObjectivesText;
    private int mDialogueIndex;
        // Alloc references
    private GameObject mFirstResource;
    private GameObject mSecondResource;
    Objective TSFirst;
    Objective TSSecond;
    Objective AAFirst;
    Objective AASecond;

    GameObject CampfireSpr;

    void Start()
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        // Manager object stays between scene loads
        DontDestroyOnLoad(gameObject);

        menuOpen = Resources.Load<AudioClip>("Audio/Menu/MutedGeigerTick") as AudioClip;

        GameObject USCImage = GameObject.Find("Canvas/USCLogo");
        GameObject BerkleeImage = GameObject.Find("Canvas/BerkleeLogo");

        if(Application.loadedLevel == 0)
            Intro(USCImage, BerkleeImage);
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize references
        if (mPauseCanvas == null)
            mPauseCanvas = GameObject.Find("PauseMenu");
        if (mHUD == null)
            mHUD = GameObject.Find("HUD");
        if (CampfireSpr == null)
            CampfireSpr = GameObject.Find("SF Scene Elements/Background");

        PauseMenu();
        UpdateHUD();
    }

    #region Regular gameplay
    void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu"))
        {
            paused = !paused;
            DisplayCursor(paused);
            if(menuOpen)
                SoundManager.instance.PlaySingle(menuOpen);
        }

        if (paused)
        {
            Time.timeScale = 0;
            if (mPauseCanvas) // nullcheck
                mPauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            if (mPauseCanvas) // nullcheck
                mPauseCanvas.SetActive(false);
        }
    }

    void OnLevelWasLoaded(int level) // When a level loads don't pause
    {
        paused = false;
        // If title screen is loaded, display the main title page
        if (level == 1)
            DisplayMain();
    }

    void UpdateHUD()
    {
        if (mHUD)
        {
            if (mTimerTextObj) // nullcheck and update timer text
            {
                if (GameManager.instance.getTimeLeft() == 0)
                    mTimerTextObj.GetComponent<Text>().text = "";
                else
                    mTimerTextObj.GetComponent<Text>().text = "Time Left: " + GameManager.instance.getTimeLeft().ToString();
            }
            else
                mTimerTextObj = GameObject.Find("HUD/PlayerInfo/TimeLeftText");
        }
        else{
            mHUD = GameObject.Find("HUD");
        }
    }
    #endregion

    #region Intro sequence
    void Intro(GameObject USC, GameObject Berklee)
    {
        Debug.Log("Intro");
        if (USC && Berklee)
        {
            Debug.Log("Intro");
            StartCoroutine(LoadIntro(USC, Berklee, 2.0f));
            // Application.LoadLevel(1);
        }
    }

    IEnumerator LoadIntro(GameObject USC, GameObject Berklee, float time)
    {
        StartCoroutine(FadeIn(USC, 2.0f));
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeToWhite(USC, 2.0f));
        StartCoroutine(FadeOut(USC, 2.0f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FadeIn(Berklee, 2.0f));
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeToWhite(Berklee, 2.0f));
        StartCoroutine(FadeOut(Berklee, 2.0f));
        yield return new WaitForSeconds(1.0f);
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    IEnumerator FadeIn(GameObject mObjFade, float speed)
    {
        CanvasGroup fadeCanvasGroup = mObjFade.GetComponent<CanvasGroup>();
        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOut(GameObject mObjFade, float speed)
    {
        CanvasGroup fadeCanvasGroup = mObjFade.GetComponent<CanvasGroup>();
        while (fadeCanvasGroup.alpha > 0.0f)
        {
            fadeCanvasGroup.alpha -= speed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(GameObject mObjFade, float speed)
    {
        Image fadeImage = mObjFade.GetComponent<Image>();
        fadeImage.CrossFadeColor(Color.white, 2.0f, false, false);
        yield return new WaitForSeconds(2.0f);
    }
    #endregion

    #region Title Screen

    private bool TitleCheck()
    {
        if (mMainPanel == null)
            mMainPanel = GameObject.Find("Canvas/TitleScreen/MainPanel");
        if (mCreditsPanel == null)
            mCreditsPanel = GameObject.Find("Canvas/TitleScreen/CreditsPanel");
        if (mInstructionsPanel == null)
            mInstructionsPanel = GameObject.Find("Canvas/TitleScreen/InstructionsPanel");

        if (mMainPanel && mCreditsPanel && mInstructionsPanel)
            return true;
        else
            return false;
    }

    public void DisplayMain()
    {
       if(TitleCheck())
       {
            mMainPanel.SetActive(true);
            mCreditsPanel.SetActive(false);
            mInstructionsPanel.SetActive(false);
       }
    }

    public void DisplayInstructions()
    {
        if(TitleCheck())
        {
            mMainPanel.SetActive(false);
            mInstructionsPanel.SetActive(true);
            mCreditsPanel.SetActive(false);
        }
    }

    public void DisplayCredits()
    {
        if(TitleCheck())
        {
            mMainPanel.SetActive(false);
            mInstructionsPanel.SetActive(false);
            mCreditsPanel.SetActive(true);
        }
    }



    #endregion

    #region Quest Menu
    public void QuestMode()
    {
        if (CampfireSpr)
            CampfireSpr.GetComponent<SpriteRenderer>().sprite = mQuest.GetCampfireSprite();
        mQuest = GameManager.instance.GetQuest(); // set mQuest to current quest
        mDialogueIndex = 0;

        // Initialize references to canvases if necessary
        if (mQuestPanel == null)
            mQuestPanel = GameObject.Find("MainCanvas/QuestPanel");
        if (mAllocPanel == null)
            mAllocPanel = GameObject.Find("MainCanvas/AllocationPanel");

        if (mQuestPanel != null && mAllocPanel != null)
        {
            // Switch panels
            mQuestPanel.SetActive(true);
            mAllocPanel.SetActive(false);
            // Instantiate dialogue
            mDescriptionText = GameObject.Find("MainCanvas/QuestPanel/QuestDescription/TextDialogue"); // Can be optomized
            if (mDescriptionText) // nullcheck
                mDescriptionText.GetComponent<Text>().text = mQuest.GetDialogueLine(mDialogueIndex);
            // Instantiate quest title
            mQuestTitleText = GameObject.Find("MainCanvas/QuestPanel/QuestDescription/TextQuestTitle");
            if (mQuestTitleText) // nullcheck
                mQuestTitleText.GetComponent<Text>().text = mQuest.GetTitle();
            // Instantiate quest objectives
            mObjectivesText = GameObject.Find("MainCanvas/QuestPanel/QuestDescription/TextObjectives");
            if (mObjectivesText) // nullcheck
            {
                string mObjectives = "";
                foreach (Objective obj in mQuest.GetMainObjectives())
                {
                    mObjectives += "\nScan " + obj.GetName() + " " + obj.GetCurrentAmt() + "/" + obj.GetTargetAmt();
                    Debug.Log(mObjectives);
                }
                mObjectivesText.GetComponent<Text>().text = mObjectives;
            }
        }
    }

    public void NextDialogue()
    {
        if(mDialogueIndex < mQuest.GetNumDialogueLines() -1)
            mDialogueIndex++;
        if(mDescriptionText)
            mDescriptionText.GetComponent<Text>().text = mQuest.GetDialogueLine(mDialogueIndex);
    }

    public void PreviousDialogue()
    {
        if(mDialogueIndex > 0)
            mDialogueIndex--;
        if(mDescriptionText)
            mDescriptionText.GetComponent<Text>().text = mQuest.GetDialogueLine(mDialogueIndex);
    }
    #endregion

    #region Allocation Menu
    public void AllocMode()
    {
        // Initialize references to canvases if necessary
        if (mQuestPanel == null)
            mQuestPanel = GameObject.Find("MainCanvas/QuestPanel");
        if (mAllocPanel == null)
            mAllocPanel = GameObject.Find("MainCanvas/AllocationPanel");

        if (mQuestPanel != null && mAllocPanel != null)
        {
            mQuestPanel.SetActive(false);
            mAllocPanel.SetActive(true);

            // Instantiate values for menu
            InstantiateCollected();
            InstantiateObjectives();
        }
    }

    private void InstantiateCollected() // Hard coded, but could take in paramater for position to be optomized
    {
        mFirstObjective = mQuest.GetObjective(0);
        mSecondObjective = mQuest.GetObjective(1);
        
        // Instantiate images
        GameObject mResourceImage1 = GameObject.Find("AllocationPanel/ResourcePanel1/ResourceInfoPanel/ResourceImage");
        if (mResourceImage1) // nullcheck
            mResourceImage1.GetComponent<Image>().sprite = mFirstObjective.GetSprite();

        GameObject mResourceImage2 = GameObject.Find("AllocationPanel/ResourcePanel2/ResourceInfoPanel/ResourceImage");
        if (mResourceImage2) // nullcheck
            mResourceImage2.GetComponent<Image>().sprite = mSecondObjective.GetSprite();

        // Instantiate collected
        GameObject mResourceText1 = GameObject.Find("AllocationPanel/ResourcePanel1/ResourceInfoPanel/ResourceAmtText");
        if (mResourceText1) // nullcheck
            mResourceText1.GetComponent<Text>().text = mFirstObjective.GetCurrentAmt().ToString() + '/' + 
                mFirstObjective.GetTargetAmt().ToString();

        GameObject mResourceText2 = GameObject.Find("AllocationPanel/ResourcePanel2/ResourceInfoPanel/ResourceAmtText");
        if (mResourceText2) // nullcheck
            mResourceText2.GetComponent<Text>().text = mSecondObjective.GetCurrentAmt().ToString() + '/' +
                mSecondObjective.GetTargetAmt().ToString();
    }

    private void InstantiateObjectives()
    {
        // Instantiate TS references
        TSFirst = mQuest.GetTSObjective(0);
        TSSecond = mQuest.GetTSObjective(1);

        // Instantiate target amounts for TS
        GameObject mTSAmount1 = GameObject.Find("AllocationPanel/ResourcePanel1/TSAllocPanel/AmountText");
        if (mTSAmount1) // nullcheck
            mTSAmount1.GetComponent<Text>().text = TSFirst.GetCurrentAmt().ToString() + '/' +
                TSFirst.GetTargetAmt().ToString();

        GameObject mTSAmount2 = GameObject.Find("AllocationPanel/ResourcePanel2/TSAllocPanel/AmountText");
        if (mTSAmount2) // nullcheck
            mTSAmount2.GetComponent<Text>().text = TSSecond.GetCurrentAmt().ToString() + '/' +
                TSSecond.GetTargetAmt().ToString();

        // Instantiate AA references
        AAFirst = mQuest.GetAAObjective(0);
        AASecond = mQuest.GetAAObjective(1);

        // Instantiate target amounts for AA
        GameObject mAAAmount1 = GameObject.Find("AllocationPanel/ResourcePanel1/AAAllocPanel/AmountText");
        if (mAAAmount1) // nullcheck
            mAAAmount1.GetComponent<Text>().text = AAFirst.GetCurrentAmt().ToString() + '/' +
                AAFirst.GetTargetAmt().ToString();

        GameObject mAAAmount2 = GameObject.Find("AllocationPanel/ResourcePanel2/AAAllocPanel/AmountText");
        if (mAAAmount2) // nullcheck
            mAAAmount2.GetComponent<Text>().text = AASecond.GetCurrentAmt().ToString() + '/' +
                AASecond.GetTargetAmt().ToString();
        
    }

    public void IncrementResource(string target, int index)
    {
        if(index == 0)
        {
            if(mFirstObjective.GetCurrentAmt() > 0) // Make sure there is a resource to allocate
            {
                if (target == "TS" && TSFirst.GetTargetAmt() > 0)
                {
                    TSFirst.IncrementCurrent();
                    mFirstObjective.DecrementCurrent();
                }
                else if (target == "AA" && AAFirst.GetTargetAmt() > 0)
                {
                    AAFirst.IncrementCurrent();
                    mFirstObjective.DecrementCurrent();
                }
            }
        }
        else if(index == 1)
        {
            if (mSecondObjective.GetCurrentAmt() > 0) // Make sure there is a resource to allocate
            {
                
                if (target == "TS" && TSSecond.GetTargetAmt() > 0)
                {
                    TSSecond.IncrementCurrent();
                    mSecondObjective.DecrementCurrent();
                }
                else if (target == "AA" && AASecond.GetTargetAmt() > 0)
                {
                    AASecond.IncrementCurrent();
                    mSecondObjective.DecrementCurrent();
                }
                    
            }
        }
        UpdateText();
    }

    public void DecrementResource(string target, int index)
    {
        if(target == "TS")
        {
            if(index == 0)
            {
                if (TSFirst.GetCurrentAmt() > 0)
                {
                    TSFirst.DecrementCurrent();
                    mFirstObjective.IncrementCurrent();
                }
            }
            else if(index == 1)
            {
                if (TSSecond.GetCurrentAmt() > 0)
                {
                    TSSecond.DecrementCurrent();
                    mSecondObjective.IncrementCurrent();
                }
            }
        }
        else if(target == "AA")
        {
            if (index == 0)
            {
                if (AAFirst.GetCurrentAmt() > 0)
                {
                    AAFirst.DecrementCurrent();
                    mFirstObjective.IncrementCurrent();
                }
            }
            else if (index == 1)
            {
                if (AASecond.GetCurrentAmt() > 0)
                {
                    AASecond.DecrementCurrent();
                    mSecondObjective.IncrementCurrent();
                }
            }
        }
        UpdateText();
    }


    private void UpdateText()
    {
        InstantiateCollected();
        InstantiateObjectives();
    }
    #endregion

    #region Utility functions
    public void DisplayCursor(bool display)
    {
        Cursor.visible = display;
    }
    #endregion
}
