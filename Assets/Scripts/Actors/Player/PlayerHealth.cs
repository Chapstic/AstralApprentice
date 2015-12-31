using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float mCurrentHealth;
    private float mMaxHealth;
    // Gameplay damage timer variables
    public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
    public float lastHitTime = 0.0f;            // Tracking when the player was last hit.
    // Visual feedback references
    public GameObject mHealthBar;
    private Image mHealthSprite;
    void Awake()
    {
        // Instantiate health values
        mMaxHealth = (float)GameManager.instance.GetPlayerStats().HP;
        mCurrentHealth = mMaxHealth;
        // Instantiate visual feedback references
        mHealthBar = GameObject.Find("HUD/PlayerInfo/HPBar");
        if(mHealthBar) // nullcheck
        {
            mHealthSprite = mHealthBar.GetComponent<Image>();
        }
    }
    // Use this call to check for collisions, because of Prime31 CharacterController2D
    public void TriggerEnter(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.tag);
        if (col.tag == "Enemy")
        {
            if (Time.time > lastHitTime + repeatDamagePeriod)
            {
                if (mCurrentHealth > 0f)
                {
                    if (col.gameObject.name == "BeeSwarm")
                        TakeDamage("BeeSwarm");
                    if (col.gameObject.name == "VWTentacles")
                        TakeDamage("VWTentacles");
                    if (col.gameObject.name == "Flybot")
                        TakeDamage("Flybot");
                    if (col.gameObject.name == "Groundbot")
                        TakeDamage("Groundbot");
                }
            }
        }
    }

    // Calculate the player's health
    void TakeDamage(string EnemyType)
    {
        // Deal damage based on enemy type
        switch(EnemyType)
        {
            case "BeeSwarm":
                mCurrentHealth -= 10;
                break;
            case "VWTentacles":
                mCurrentHealth -= 25;
                break;
            case "Flybot":
                mCurrentHealth -= 25;
                break;
            case "Groundbot":
                mCurrentHealth -= 25;
                break;
        }

        lastHitTime = Time.time; // Update when the player was last hit
        UpdateHealthBar(); // Update what the health bar looks like

        // Check to see if player needs to be killed
        if(mCurrentHealth <= 0) 
            StartCoroutine(KillPlayer());
    }

    // Visual feedback
    public void UpdateHealthBar()
    {
        if (mHealthSprite)
        { // Nullcheck healthBar image
            mHealthSprite.fillAmount = (float)mCurrentHealth / mMaxHealth; // Update health bar
        }
    }

    // Reload level after doing things
    IEnumerator KillPlayer()
    {
        Debug.Log("Killing player delay begin");
        yield return new WaitForSeconds(2);
        Debug.Log("Killing player now");
        Application.LoadLevel(Application.loadedLevel);
    }
}

