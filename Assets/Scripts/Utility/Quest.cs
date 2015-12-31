using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest {

    private string mTitle;
    private bool mHasTimer;
    private float mTotalTime;

    private List<string> mDialogue;
    private List<Objective> mQuestObjectives;
    private List<Objective> mTSObjectives;
    private List<Objective> mAAObjectives;
    private Sprite mCampfireImage;

    public Quest(string title)
    {
        mTitle = title;
        mDialogue = new List<string>();
        mQuestObjectives = new List<Objective>();
        mTSObjectives = new List<Objective>();
        mAAObjectives = new List<Objective>();

    }

    public Sprite GetCampfireSprite()
    {
        return mCampfireImage;
    }

    public void SetCampfireSprite(Sprite spr)
    {
        mCampfireImage = spr;
    }
    #region Quest text and story
    public string GetTitle()
    {
        return mTitle;
    }

    public void AddDialogue(string dialogue)
    {
        if(mDialogue != null) // nullcheck
            mDialogue.Add(dialogue);
    }

    public List<string> GetDialogue()
    {
        return mDialogue;
    }

    public string GetDialogueLine(int index)
    {
        if (mDialogue != null) // nullcheck
            return mDialogue[index];
        else return null;
    }

    public int GetNumDialogueLines()
    {
        if (mDialogue != null) // nullcheck
            return mDialogue.Count;
        else return 0;
    }
    #endregion

    #region Quest timer
    public void SetTimer(float time, bool hasTimer)
    {
        mTotalTime = time;
        mHasTimer = hasTimer;
    }

    public float GetTotalTime()
    {
        return mTotalTime;
    }

    public bool HasTimer()
    {
        return mHasTimer;
    }
    #endregion

    #region Quest objectives
    public void AddMainObjective(Objective objective)
    {
        mQuestObjectives.Add(objective);
    }
    public List<Objective> GetMainObjectives()
    {
        return mQuestObjectives;
    }
    public Objective GetObjective(int index)
    {
        return mQuestObjectives[index];
    }
    public void IncrementMainObjective(string name, int val)
    {
        foreach (Objective obj in mQuestObjectives)
        {
            if (obj.GetName() == name)
            {
                obj.IncrementCurrent();
            }
        }
    }

    public void ClearObjectives()
    {
        mQuestObjectives.Clear();
    }
    public bool CheckCompletion()
    {
        int objCompleted = 0; // Reset to prevent double counting
        foreach(Objective obj in mQuestObjectives)
        {
            if(obj.IsComplete())
                objCompleted++;
        }

        if (objCompleted == mQuestObjectives.Count)
        {
            CompleteQuest();
            return true;
        }
        else
            return false;
    }
    #endregion

    // Misc.
    public void CompleteQuest()
    {
        GameManager.instance.EndPlaying();
    }

    #region TS Allocation objectives
    public void AddTSObjective(Objective objective)
    {
        mTSObjectives.Add(objective);
    }

    public void IncrementTSObjective(string name)
    {
        foreach (Objective obj in mTSObjectives)
        {
            if (obj.GetName() == name)
            {
                obj.IncrementCurrent();
            }
        }
    }

    public void DecrementTsObjective(string name)
    {
        foreach (Objective obj in mTSObjectives)
        {
            if (obj.GetName() == name)
            {
                obj.DecrementCurrent();
            }
        }
    }

    public Objective GetTSObjective(int index)
    {
        return mTSObjectives[index];
    }

    public bool TSIsComplete()
    {
        int completedCount = 0;
        foreach (Objective obj in mTSObjectives)
        {
            if (obj.IsComplete())
            {
                completedCount++;
            }
        }

        if (completedCount == mTSObjectives.Count)
            return true;
        else
            return false;
    }

    #endregion

    #region AA Allocation objectives
    public void AddAAObjective(Objective objective)
    {
        mAAObjectives.Add(objective);
    }
    public void IncrementAAObjective(string name)
    {
        foreach (Objective obj in mAAObjectives)
        {
            if (obj.GetName() == name)
            {
                obj.IncrementCurrent();
            }
        }
    }

    public Objective GetAAObjective(int index)
    {
        return mAAObjectives[index];
    }

    public bool AAIsComplete()
    {
        int completedCount = 0;
        foreach (Objective obj in mAAObjectives)
        {
            if (obj.IsComplete())
            {
                completedCount++;
            }
        }

        if (completedCount == mAAObjectives.Count)
            return true;
        else
            return false;
    }
    #endregion
}
