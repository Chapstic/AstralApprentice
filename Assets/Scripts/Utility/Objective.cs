using UnityEngine;
using System.Collections;

// Class to hold data for objectives or tasks
public class Objective {

    private string mName;
    private int mCurrentAmount;
    private int mTargetAmount;
    private Sprite mSprite;

    public Objective(string name, int current, int target, string spritepath)
    {
        mName = name;
        mCurrentAmount = current;
        mTargetAmount = target;
        mSprite = Resources.Load<Sprite>("Sprites/Items/" + spritepath);
    }
    // Helper functions
    public string GetName()
    {
        return mName;
    }
    public void IncrementCurrent()
    {
        mCurrentAmount++;
    }

    public void DecrementCurrent()
    {
        mCurrentAmount--;
    }

    public int GetCurrentAmt()
    {
        return mCurrentAmount;
    }
    public int GetTargetAmt()
    {
        return mTargetAmount;
    }

    public Sprite GetSprite()
    {
        if (mSprite)
            return mSprite;
        return null;
    }

    public bool IsComplete()
    {
        if (mCurrentAmount >= mTargetAmount)
            return true;
        else
            return false;
    }
    
}
