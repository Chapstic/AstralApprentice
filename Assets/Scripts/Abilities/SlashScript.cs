using UnityEngine;
using System.Collections;

public class SlashScript : MonoBehaviour {

    private SpriteRenderer spriteController;
    public Sprite startup;
    public Sprite activeBegin;
    public Sprite activeEnd;
    private bool isActive = false;
    private float frameCount = 0.5f;
    private int slashStrength = 3;


	void Start () {
        spriteController = GetComponent<SpriteRenderer>();
        spriteController.sprite = startup;
		Destroy (gameObject, 0.5f);
	}

    void Update()
    {
        transform.position = transform.parent.transform.position;
        if (frameCount < 0.25 && frameCount > 0.1)
        {
            isActive = true;
            spriteController.sprite = activeBegin;
        }
        else if (frameCount < 0.1)
        {
            isActive = true;
            spriteController.sprite = activeEnd;
        }
        frameCount -= 1 * Time.deltaTime;
    }

	void OnTriggerStay2D (Collider2D col)
	{
		// If it hits an enemy...
		if (col.tag == "Enemy" && isActive) {
			// ... find the Enemy script and call the Hurt function.
			if (col.gameObject.name == "VWBody") {
                if (!col.transform.parent.gameObject.GetComponent<VWAIScript>().isDead())
                {
                    col.gameObject.transform.parent.gameObject.GetComponent<VWAIScript>().Hurt(slashStrength);
                    Destroy(gameObject);
                }
			}
            if (col.gameObject.name == "BeeSwarm")
            {
                col.gameObject.GetComponent<BeeSwarmAIScript>().Hurt(slashStrength);
                Destroy(gameObject);
            }
            if (col.gameObject.name == "BeeHive")
            {
                col.gameObject.GetComponent<BeeHiveSpawnScript>().Hurt(slashStrength);
                Destroy(gameObject);
            }
            if (col.gameObject.name == "Flybot") 
            {
                col.gameObject.GetComponent<FlybotAIScript>().Hurt(slashStrength);
                Destroy(gameObject);
            }
            if (col.gameObject.name == "Groundbot")
            {
                col.gameObject.GetComponent<GroundbotAIScript>().Hurt(slashStrength);
                Destroy(gameObject);
            }
			// Call the explosion instantiation.
			//OnExplode ();

			// Destroy the rocket.
			//Destroy (gameObject);
		} else {
			//Destroy (gameObject);
		}
	}
}