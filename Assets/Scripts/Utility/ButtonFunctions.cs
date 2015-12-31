using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour {

    public float fadeIn, fadeOut;
    private AudioClip ButtonSound;
    private AudioClip WalkyTalkyPress;

    public void StartGame()
    {
        // TO DO: This is where the beginning exposition stuff would go (GameState.GAMEBEGIN)
        // Start the player off with campfire + quest for now
        Application.LoadLevel(Application.loadedLevel + 1);
        GameManager.instance.SetGameState(GameManager.GameState.QUEST);
        GameManager.instance.SetNextLevel(GameManager.Level.FOREST);

        ButtonSound = Resources.Load<AudioClip>("Audio/Menu/LightPlink") as AudioClip;
        WalkyTalkyPress = Resources.Load<AudioClip>("Audio/Menu/WalkyTalkyPress") as AudioClip;
    }

    public void BeginLevel()
    {
        GameManager.instance.Playing();
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void EndLevel()
    {
        GameManager.instance.EndPlaying();
    }

    public void RestartCurrent()
    {
        Application.LoadLevel(Application.loadedLevel);
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void QuitGame()
    {
        Application.Quit();
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    #region Title button functions
    public void ReturnToMain()
    {
        MenuManager.instance.DisplayMain();
    }

    public void DisplayInstructions()
    {
        MenuManager.instance.DisplayInstructions();
    }

    public void DisplayCredits()
    {
        MenuManager.instance.DisplayCredits();
    }

    #endregion

    #region Quest button functions
    public void NextDialogue()
    {
        MenuManager.instance.NextDialogue();
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }
    public void PreviousDialogue()
    {
        MenuManager.instance.PreviousDialogue();
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    #endregion

    #region Allocation button functions
    public void IncrementAA(int index)
    {
        MenuManager.instance.IncrementResource("AA", index);
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void IncrementTS(int index)
    {
        MenuManager.instance.IncrementResource("TS", index);
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void DecrementAA(int index)
    {
        MenuManager.instance.DecrementResource("AA", index);
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void DecrementTS(int index)
    {
        MenuManager.instance.DecrementResource("TS", index);
        if (ButtonSound)
            SoundManager.instance.PlaySingle(ButtonSound);
    }

    public void ConfirmAllocation()
    {
        // Read choices in
        if (WalkyTalkyPress)
            SoundManager.instance.PlaySingle(WalkyTalkyPress);
        GameManager.instance.Allocate();
    }
    #endregion
}
