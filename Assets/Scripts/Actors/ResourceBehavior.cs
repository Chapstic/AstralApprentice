using UnityEngine;
using System.Collections;

public class ResourceBehavior : MonoBehaviour {

    Quest mQuest; // Reference to active quest to track progress
    public Sprite ScannedSprite; // Enemy's default sprite
    private SpriteRenderer sRenderer; // Reference to the sprite renderer.
    private bool scanned;

	void Awake()
    {
        scanned = false;
        mQuest = GameManager.instance.GetQuest();
        sRenderer = transform.GetComponent<SpriteRenderer>();
    }
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Scan()
    {
        if(!scanned)
        {
            scanned = true;
            Increment();
            mQuest.CheckCompletion();
            if (sRenderer != null && ScannedSprite != null)
                sRenderer.sprite = ScannedSprite; // Set sprite to scanned sprite
        }
        
    }

    public bool GetScanned()
    {
        return scanned;
    }

    public void Increment()
    {
        if (gameObject.name == "GemResource")
        {
            mQuest.IncrementMainObjective("Gem", 1); // Only increments by 1, value is irelevant
        }
        if (gameObject.name == "DargradFruit")
        {
            mQuest.IncrementMainObjective("Fruit", 1);
        }
        if (gameObject.name == "MushroomResource")
        {
            mQuest.IncrementMainObjective("Mushroom", 1);
        }
        if (gameObject.name == "FlowerResourceOne")
        {
            mQuest.IncrementMainObjective("Flower", 1);
        }
        if (gameObject.name == "Flybot")
        {
            mQuest.IncrementMainObjective("Processor", 1);
        }
        if (gameObject.name == "Groundbot")
        {
            mQuest.IncrementMainObjective("Processor", 1);
        }
        if (gameObject.name == "BlueprintResource")
        {
            mQuest.IncrementMainObjective("Blueprint", 1);
        }
    }
}
