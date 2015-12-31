using UnityEngine;
using System.Collections;

public class BeeSwarmAIScript : EnemyBase {

    public BeeHiveSpawnScript parent;
    public GameObject player;

    public int maxHP = 1;				// Enemies max health
    public int HP = 1;					// How many times the enemy can be hit before it dies.
    private bool dead = false;			// Whether or not the enemy is dead.
    private AudioClip MoveAudio;		    // An audioclip that is played when the enemy dies.

    private static float WAIT = 2.5f;
    private static float GO = 1.0f;
    private static float SPEED = 3.0f;
    private float timer = 0;
    private bool moving = false;

	// Use this for initialization
	void Awake () {
        base.Awake(); // Call base classe's awake (Super)
        player = GameObject.Find("Player");
        MoveAudio = Resources.Load("Audio/Units/Bees/BeesBuzzing") as AudioClip;
	}

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (!moving)
            {
                timer += 1 * Time.deltaTime;
                if (timer > WAIT)
                {
                    timer = 0;
                    moving = true;
                }
            }
            else
                Move();
        }
    }

    public bool isDead()
    {
        return dead;
    }

    protected override void Move() 
    {
        float step = SPEED * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        timer += 1 * Time.deltaTime;
        if (timer > GO)
        {
            timer = 0;
            moving = false;
        }
        if (MoveAudio) // nullcheck
            //SoundManager.instance.PlaySingle(MoveAudio);
            AudioSource.PlayClipAtPoint(MoveAudio, transform.position);
    }

    public void Hurt(int damage)
    {
        // Reduce the number of hit points by one.
        HP -= damage;
        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
        {
            // ... call the death function.
            Death();
        }
    }

    void Death()
    {
        parent.decrementSwarm();

        // Find all of the sprite renderers on this object and it's children.
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Disable all of them sprite renderers.
        foreach (SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }

        // Set dead to true.
        dead = true;

        // Play dying sound when enemy dies
        SoundManager.instance.PlaySingle(MoveAudio);

        Destroy(gameObject);
    }
}
