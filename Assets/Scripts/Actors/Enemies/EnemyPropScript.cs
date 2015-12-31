using UnityEngine;
using System.Collections;

public class EnemyPropScript : MonoBehaviour
{
	public float moveSpeed = 0f;		// The speed the enemy moves at.
	public int maxHP = 2;				// Enemies max health
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite normalEnemy;			// Enemy's default sprite
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies.

	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private bool dead = false;			// Whether or not the enemy is dead.
	private int hitStun = 0;


	void Awake()
	{
		// Setting up the references.
		//use this is the enemy is composed of multiple parts (hitboxes and hurtboxes)
		//ren = transform.Find("body").GetComponent<SpriteRenderer>();
		ren = transform.GetComponent<SpriteRenderer>();
		normalEnemy = ren.sprite;
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
		GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);	

		if (ren.sprite == damagedEnemy) {
			hitStun++;
			if (hitStun > 6) {
				ren.sprite = normalEnemy;
			}
		}
	}

	public void Hurt(int damage)
	{
		// Reduce the number of hit points by one.
		HP -= damage;
		// If the enemy has lost health and has a damagedEnemy sprite...
		if (HP < maxHP && damagedEnemy != null) {
			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
			ren.sprite = damagedEnemy;
		}

		// If the enemy has zero or fewer hit points and isn't dead yet...
		if (HP <= 0 && !dead) {
			// ... call the death function.
			Death ();
		}
	}

	void Death()
	{
		// Find all of the sprite renderers on this object and it's children.
		/*SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}*/

		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		if (deadEnemy != null) {
			ren.enabled = true;
			ren.sprite = deadEnemy;
		}

		// Set dead to true.
		dead = true;

		// Find all of the colliders on the gameobject and set them all to be triggers.
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}

		// Play a random audioclip from the deathClips array.
		// Re-add when we have death sounds
		/*int i = Random.Range(0, deathClips.Length);
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position);*/

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
