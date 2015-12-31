using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VWAIScript : EnemyBase
{

	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public int maxHP = 2;				// Enemies max health
	public int HP = 2;					// How many times the enemy can be hit before it dies.
    private bool dead = false;			// Whether or not the enemy is dead.
    private float hitStun = 0;
    // Visuals
    private SpriteRenderer ren;			// Reference to the sprite renderer.
    private Image healthBar;
	public Sprite normalEnemy;			// Enemy's default sprite
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
    public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
    // Sound
    public AudioClip HitClip;            // An audioclip that is played when the enemy is hit.
	public AudioClip DeathClip;		    // An audioclip that is played when the enemy dies.
	// Values for movement
	private bool isHit = false;
	private float homeX;
	private float homeY;
	private float amplitudeX = 5.0f;
	private float amplitudeY = 1.0f;
	private float omegaX = 1.0f;
	private float omegaY = 2.0f;
	private float index;

	public override void Awake()
	{
		// Setting up the references.
		//use this is the enemy is composed of multiple parts (hitboxes and hurtboxes)
		//ren = transform.Find("body").GetComponent<SpriteRenderer>();

        base.Awake(); // "Super" calling parent's Awake
		ren = transform.GetComponent<SpriteRenderer>();
        // Load sprites
        normalEnemy = Resources.Load<Sprite>("Sprites/Units/VyvosWhiptricFloatOne002");
        damagedEnemy = Resources.Load<Sprite>("Sprites/Units/VyvosWhiptricDamagedOne002");
        deadEnemy = Resources.Load<Sprite>("Sprites/Units/VyvosWhiptricDeathOne001");

		homeX = gameObject.transform.position.x;
		homeY = gameObject.transform.position.y;
        healthBar = transform.FindChild("HealthCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
	}

    public bool isDead()
    {
        return dead;
    }

	void FixedUpdate ()
	{
		//Maybe add this back in if we want to avoid enemies running into objects
		// Create an array of all the colliders in front of the enemy.
		/*Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders.
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
				break;
			}
		}*/

		// Set the enemy's velocity to moveSpeed in the x direction.
		//GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);	

		if (!dead)
        {
            Move();

            if (isHit)
            {
                hitStun += Time.deltaTime;
                if (hitStun > 0.5f)
                {
                    ren.sprite = normalEnemy;
                    isHit = false;
                    hitStun = 0;
                }
            }
        }
	}

	protected override void Move() 
    {
		index += Time.deltaTime;
		float x = amplitudeX*Mathf.Cos (omegaX*index);
		float y = amplitudeY*Mathf.Sin (omegaY*index);
		transform.localPosition= new Vector3(homeX + x, homeY + y, 0);
	}

	public void Hurt(int damage)
	{
        HP -= damage; // Reduce the number of hit points by one.
		isHit = true;
		// If the enemy has lost health and has a damagedEnemy sprite...
		if (HP < maxHP && damagedEnemy != null) {
            SoundManager.instance.PlaySingle(HitClip); // Play damaged sound clip
            ren.sprite = damagedEnemy; // set the sprite renderer's sprite to be the damagedEnemy sprite.
            if(healthBar) // Nullcheck healthBar image
                healthBar.fillAmount = (float)HP / (float)maxHP; // Update health bar
		}
		// If the enemy has zero or fewer hit points and isn't dead yet...
		if (HP <= 0 && !dead) {
            // Kill this enemy
			Death ();
		}
	}

	void Death()
	{
        // Nullcheck deadEnemySprite, and then set sprite to dead
		if (deadEnemy) 
		    ren.sprite = deadEnemy;

		// Set dead to true.
		dead = true;

        // Set tag and layer to decoration
        transform.tag = "Decoration";
        gameObject.layer = 8; // the 8th layer is decoration

        GetComponent<Rigidbody2D>().gravityScale = 1;
        SoundManager.instance.PlaySingle(DeathClip); // Play dying sound when enemy dies
        Image HealthBG = transform.FindChild("HealthCanvas").FindChild("HealthBG").GetComponent<Image>();
        HealthBG.enabled= false; // Disable health bar image

		// Find all of the colliders on the gameobject and set them all to be triggers.
		/*Collider2D[] cols = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}*/

		// Create a vector that is just above the enemy.
		/* Reinstate if we want to spawn the resource somewhere other than the enemies dead body
		 * Vector3 resourcePos;
		resourcePos = transform.position;
		resourcePos.y += 1.5f;
		*/

		// Instantiate any resource prefabs at this point.
		//Instantiate(RESOURCE, resourcePos, Quaternion.identity);
	}

	public void Flip()
	{
		//might be useful later
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
