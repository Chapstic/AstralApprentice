using UnityEngine;
using System.Collections;

public class BeeHiveSpawnScript : MonoBehaviour {

    private static int MAXSWARMS = 3;
    private static float SPAWNRATE = 5;

    private BeeHiveDetectionScript detector;
    private float spawn = 0;
    private int numSwarms = 0;

    public int maxHP = 2;				// Enemies max health
    public int HP = 2;					// How many times the enemy can be hit before it dies.
    private bool dead = false;
    private bool isHit = false;
    private float hitStun = 0;
    public Sprite normalEnemy;			// Enemy's default sprite
    public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
    public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
    private SpriteRenderer ren;			// Reference to the sprite renderer.

    public Transform BeeSwarm;

	// Use this for initialization
	void Awake () {
        detector = GetComponentsInChildren<BeeHiveDetectionScript>()[0];
        ren = transform.GetComponent<SpriteRenderer>();
        normalEnemy = ren.sprite;
        transform.name = "BeeHive";
	}

    public bool isDead()
    {
        return dead;
    }

    public void Hurt(int damage)
    {
        // Reduce the number of hit points by one.
        HP -= damage;
        isHit = true;
        // If the enemy has lost health and has a damagedEnemy sprite...
        if (HP < maxHP && damagedEnemy != null)
        {
            // ... set the sprite renderer's sprite to be the damagedEnemy sprite.
            ren.sprite = damagedEnemy;
        }
        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
        {
            // ... call the death function.
            Death();
        }
    }

    void Death()
    {
        // Find all of the sprite renderers on this object and it's children.
        Transform[] swarms = GetComponentsInChildren<Transform>();

        // Disable all of them sprite renderers.
        foreach (Transform s in swarms)
        {
            if (s.name == "BeeSwarm")
            {
                s.gameObject.GetComponent<BeeSwarmAIScript>().Hurt(1);
            }
        }

        ren.sprite = deadEnemy;

        // Set dead to true.
        dead = true;

        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (detector.isInRange())
        {
            spawn += 1 * Time.deltaTime;
            if (spawn > SPAWNRATE)
            {
                spawn = 0;
                Spawn();
            }
        }
        if (isHit)
        {
            hitStun += Time.deltaTime;
            if (hitStun > 0.25f)
            {
                ren.sprite = normalEnemy;
                isHit = false;
                hitStun = 0;
            }
        }
	}

    public void decrementSwarm()
    {
        numSwarms--;
    }

    void Spawn()
    {
        if (numSwarms < MAXSWARMS)
        {
            numSwarms++;
            Transform swarm = (Transform) Instantiate(BeeSwarm, gameObject.transform.position, Quaternion.identity);
            swarm.name = "BeeSwarm";
            swarm.GetComponent<BeeSwarmAIScript>().parent = this;
        }
    }
}
