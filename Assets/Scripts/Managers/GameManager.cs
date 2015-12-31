using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // Singleton pattern
    public static GameManager instance = null;
    // Player state tracking
    public enum PlayerState { ALIVE, DEAD, SCAN };
    private PlayerState mPlayerState;
    // Game state variables
    public enum GameState { GAMEBEGIN, QUEST, PLAYING, ENDPLAYING, ALLOCATION, GAMEOVER };
    private GameState mCurrentState;
    // Level variable tracking next level to facilitate state changes
    public enum Level : int { FOREST, UNMARS1, UNMARS2, INDUSTRIAL, END };
    private Level mNextLevel;
    // Campfire specific variables
    private GameObject mQuestPanel;
    private GameObject mAllocPanel;
    private GameObject mCollectedText;
    // Variables shared across all levels
    private float timer;
    private Quest mQuest;
    // Variables for levels
    public PlayerStats mPlayerStats;
    public float FriendlyForrestTime = 0.0f;
    public float UnMars1Time = 100.0f;
    public float UnMars2Time = 150.0f;
    public float IndustrialTime = 300.0f;

    public Sprite FFCampfireImage;
    public Sprite UnMarsCampfireImage;
    public Sprite MFCampfireImage;

	// Use this for initialization
	void Start () 
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        // Manager object stays between scene loads
        DontDestroyOnLoad(gameObject);

        mPlayerStats = new PlayerStats();

        FFCampfireImage = Resources.Load<Sprite>("Sprites/Landscape/FFCampfire") as Sprite;
        UnMarsCampfireImage = Resources.Load<Sprite>("Sprites/Landscape/UnMarsCampfire") as Sprite;
        MFCampfireImage = Resources.Load<Sprite>("Sprites/Landscape/MFCampfire") as Sprite;
	}

    // Update is called once per frame
    void Update()
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (mQuest != null && mQuest.HasTimer() && mCurrentState == GameState.PLAYING)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                EndPlaying();
        }
    }

    public void Playing()
    {
        MenuManager.instance.DisplayCursor(false);
        switch(mNextLevel)
        {
            case Level.FOREST:
                FriendlyForest();
                break;
            case Level.UNMARS1:
                UnMars(1);
                break;
            case Level.UNMARS2:
                UnMars(2);
                break;
            case Level.INDUSTRIAL:
                Industrial();
                break;
            case Level.END:
                EndLevel();
                break;
            default:
                Debug.Log("Undefined End case");
                break;
        }
    }

    public void EndPlaying()
    {
        // If the player has completed the level, load campfire
        switch(mNextLevel)
        {
            case Level.FOREST:
                EndFriendlyForest();
                break;
            case Level.UNMARS1:
                EndUnMars(1);
                break;
            case Level.UNMARS2:
                EndUnMars(2);
                break;
            case Level.INDUSTRIAL:
                EndIndustrial();
                break;
            default:
                Debug.Log("Undefined End case");
                break;
        }
        // Load campfire scene and switch to allocation mode
        Application.LoadLevel("Campfire");
        mCurrentState = GameState.ALLOCATION;
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
            CampfireMenu();
    }

    public void Allocate()
    {
        // Allocate upgrades here
        if (mQuest.TSIsComplete())
            mPlayerStats.UpgradeTS();
        if (mQuest.AAIsComplete())
            mPlayerStats.UpgradeAA();
        mCurrentState = GameState.QUEST;
        Application.LoadLevel("Campfire");
    }

    public void CampfireMenu()
    {
        MenuManager.instance.DisplayCursor(true);
        // Display quest menu only if game state is quest
        if (mCurrentState == GameState.QUEST)
        {
            // Instantiate quest info based on level
            switch (mNextLevel)
            {
                case Level.FOREST:
                    QuestFriendlyForest();
                    break;
                case Level.UNMARS1:
                    QuestUnMars(1);
                    break;
                case Level.UNMARS2:
                    QuestUnMars(2);
                    break;
                case Level.INDUSTRIAL:
                    QuestIndustrial();
                    break;
                case Level.END:
                    EndLevel();
                    break;
                default:
                    Debug.Log("Undefined Quest case");
                    break;
            }
            MenuManager.instance.QuestMode(); // Display quest info in menu manager
        }

        if (mCurrentState == GameState.ALLOCATION)
        {
            mPlayerStats.FirstObjectiveTotal = mQuest.GetMainObjectives()[0].GetCurrentAmt();
            mPlayerStats.SecondObjectiveTotal = mQuest.GetMainObjectives()[1].GetCurrentAmt();
            MenuManager.instance.AllocMode();
        }
    }

    #region Friendly Forest level
    // Functions to begin and end Friendly Forest
    public void QuestFriendlyForest()
    {
        // Quest creation. Maybe read from file in the future
        mQuest = new Quest("Friendly Forest"); // Create a new quest w/ title parameter
        mQuest.SetTimer(0, false); // There is no timer for this quest
        mQuest.AddDialogue("Apprentice, I need you to go out and scan objects by pressing 'Z'"); // Read from text file in future
        mQuest.AddDialogue("I've given you magic missiles (X) and slashes (C)"); // Read from text file in future
        mQuest.AddDialogue("You'll have all the time you need. "); // Read from text file in future
        mQuest.AddDialogue("I'm going to go meet with the King."); // Read from text file in future
        mQuest.AddMainObjective(new Objective("Mushroom", 0, 1, "FairyCircleMushroom001")); // Set objectives or ways to complete quest
        mQuest.AddMainObjective(new Objective("Flower", 0, 3, "JuniperFlowerOne001")); // Set objectives or ways to complete quest
        // TS Goals in allocation
        mQuest.AddTSObjective(new Objective("Mushroom", 0, 1, "FairyCircleMushroom001"));
        mQuest.AddTSObjective(new Objective("Flower", 0, 1, "JuniperFlowerOne001"));
        // AA Goals in allocation
        mQuest.AddAAObjective(new Objective("Mushroom", 0, 0, "FairyCircleMushroom001"));
        mQuest.AddAAObjective(new Objective("Flower", 0, 2, "JuniperFlowerOne001"));
        // Set campfire
        mQuest.SetCampfireSprite(FFCampfireImage);
    }

    public void FriendlyForest()
    {
        // Quest creation happened in awake() 
        // Level loading and state transition
        Application.LoadLevel("FriendlyForestDay1");
        mCurrentState = GameState.PLAYING;
        SoundManager.instance.PlayMusic("Forest");
    }

    public void EndFriendlyForest()
    {
        mNextLevel = Level.UNMARS1;
        mCurrentState = GameState.ALLOCATION;
        Application.LoadLevel("Campfire");
    }
    #endregion

    #region UnMars level
    // Functions to begin and end UnMars
    public void QuestUnMars(int day)
    {
        if(day == 1)
        {
            mQuest = new Quest("UnMars Day 1");
            mQuest.SetTimer(UnMars1Time, true);
            mQuest.AddDialogue("Well met Apprentice, I see you've collected everything required of you in the forest. Well done.");
            mQuest.AddDialogue("My meeting with the King went… unfavorably…");
            mQuest.AddDialogue("Some people just can’t handle criticism...");
            mQuest.AddDialogue("I might have overdone it when I called him a 'thoroughly incompetent fool.'");
            mQuest.AddDialogue("Anyway, long story short the Royal Guard is after me and consequently you as well.");
            mQuest.AddDialogue("Never fear, however, for I have a plan.");
            mQuest.AddDialogue("You’ll just have to move from planet to planet quickly until we can reunite.");
            mQuest.AddDialogue("Now then, make haste to Unmars! You should be safe there for a day or so.");
            mQuest.AddDialogue("Oh! And while you’re there I need you to collect some things for me.");
            mQuest.AddDialogue("One fruit of the Dargrad tree and if you can find them, some Unmartian gems.");
            mQuest.AddMainObjective(new Objective("Gem", 0, 1, "GemResourceSprite001"));
            mQuest.AddMainObjective(new Objective("Fruit", 0, 2, "DargradFruit001"));
            // TS Objectives
            mQuest.AddTSObjective(new Objective("Gem", 0, 1, "GemResourceSprite001"));
            mQuest.AddTSObjective(new Objective("Fruit", 0, 1, "DargradFruit001"));
            // AA Objectives
            mQuest.AddAAObjective(new Objective("Gem", 0, 0, "GemResourceSprite001"));
            mQuest.AddAAObjective(new Objective("Fruit", 0, 1, "DargradFruit001"));

            mQuest.SetCampfireSprite(UnMarsCampfireImage);

        }
        else if (day == 2)
        {
            mQuest = new Quest("UnMars Day 2");
            mQuest.SetTimer(UnMars2Time, true); // Temporary to skip UnMars2
            mQuest.AddDialogue("The techno sorceress has asked you to Scan 3 resources total. You will have 100 seconds (1 min 40 seconds)");
        }
    }

    public void UnMars(int day)
    {
        mCurrentState = GameState.PLAYING;
        timer = mQuest.GetTotalTime();
        Application.LoadLevel("UnmarsDay1");
        SoundManager.instance.PlayMusic("UnMars");
    }

    public void EndUnMars(int day)
    {
        if (day == 1)
        {
            // mNextLevel = Level.UNMARS2;
            mNextLevel = Level.INDUSTRIAL; // SKIPPING UNMARS DAY2 TEMP
            GameManager.instance.mPlayerStats.TotalTimeUnmars = (int)UnMars1Time -(int)timer; // Metrics
            Application.LoadLevel("Campfire");
            Debug.Log("End of UnMars Day 1");
        }
        else if (day == 2)
        {
            mNextLevel = Level.INDUSTRIAL;
            Application.LoadLevel("Campfire");
            Debug.Log("End of UnMars Day 2");
        }
        else
        {
            Debug.Log("Invalid day");
        }
    }
    #endregion

    #region Industrial level
    public void QuestIndustrial()
    {
        mQuest = new Quest("Industrial Planet");
        mQuest.SetTimer(IndustrialTime, true);
        mQuest.AddDialogue("Alright then, you can’t afford to tarry there any longer, I fear the Royal Guard have picked up your trail.");
        mQuest.AddDialogue("I’m sending you on to one more location before we rendezvous. Your next destination is an abandoned mining facility on Praxa 4.");
        mQuest.AddDialogue("The facility has been defunct for years, but there are some remains that are worth salvaging.");
        mQuest.AddDialogue("Particularly, the processors the company used in their robots have a dynamic metal in them that is exceedingly useful.");
        mQuest.AddDialogue("According to my information, there should also be some blueprints in the facility to a very intriguing machine called a Slipway Generator.");
        mQuest.AddDialogue("Bring me these blueprints and about 4 of the processors.");
        // Quest objective setting
        mQuest.AddMainObjective(new Objective("Processor", 0, 4, "PraxaProcessor001"));
        mQuest.AddMainObjective(new Objective("Blueprint", 0, 1, "SlipwayBlueprintSprite001"));
        // TS Objectives
        mQuest.AddTSObjective(new Objective("Processor", 0, 4, "PraxaProcessor001"));
        mQuest.AddTSObjective(new Objective("Blueprint", 0, 1, "SlipwayBlueprintSprite001"));
        // AA Objectives
        mQuest.AddAAObjective(new Objective("Processor", 0, 2, "PraxaProcessor001"));
        mQuest.AddAAObjective(new Objective("Blueprint", 0, 0, "SlipwayBlueprintSprite001"));

        mQuest.SetCampfireSprite(MFCampfireImage);
    }
    public void Industrial()
    {
        mCurrentState = GameState.PLAYING;
        timer = mQuest.GetTotalTime();
        SoundManager.instance.PlayMusic("Industrial");
        Application.LoadLevel("MiningFacilityDay1");
    }

    public void EndIndustrial()
    {
        mCurrentState = GameState.ENDPLAYING;
        GameManager.instance.mPlayerStats.TotalTimeIndustrial = (int)IndustrialTime - (int)timer;
        // End of game load next level
        Application.LoadLevel("Campfire");
        mNextLevel = Level.END;

    }
    #endregion
    
    public void EndLevel()
    {
        Application.LoadLevel("CreditsScreen");
    }

    #region Helper functions
    // Helper functions for timer functionality
    public int getTimeLeft()
    {
        int seconds = (int)timer;
        return seconds;
    }
    // Helper functions for player state tracking
    public PlayerState GetPlayerState()
    {
        return mPlayerState;
    }

    public void SetPlayerState(PlayerState state)
    {
        mPlayerState = state;
    }
    // Helper functions for game state tracking
    public GameState GetGameState()
    {
        return mCurrentState;
    }

    public void SetGameState(GameState state)
    {
        mCurrentState = state;
    }
    // Helper functions for level state tracking
    public Level GetNextLevel()
    {
        return mNextLevel;
    }

    public void SetNextLevel(Level level)
    {
        mNextLevel = level;
    }

    // Metrics helper
    public PlayerStats GetPlayerStats()
    {
        return mPlayerStats;
    }
    // Helper function for quests
    public Quest GetQuest()
    {
        return mQuest;
    }
    #endregion

}
