using UnityEngine;
using System.Collections;

public class MagicMissileScript : MonoBehaviour {

    private int missileStrength;

	void Start () {
		Destroy (gameObject, 1);
        missileStrength = GameManager.instance.mPlayerStats.GetMissileStrength();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		// ... find the Enemy script and call the Hurt function.
		if (col.gameObject.name == "VWBody") 
        {
            if (!col.transform.parent.gameObject.GetComponent<VWAIScript>().isDead())
            {
                col.gameObject.transform.parent.gameObject.GetComponent<VWAIScript>().Hurt(missileStrength);
                Destroy(gameObject);
            }
		}
        if (col.gameObject.name == "BeeSwarm")
        {
            col.gameObject.GetComponent<BeeSwarmAIScript>().Hurt(missileStrength);
        }
        if (col.gameObject.name == "BeeHive")
        {
            col.gameObject.GetComponent<BeeHiveSpawnScript>().Hurt(missileStrength);
        }
        if (col.gameObject.name == "Flybot")
        {
            col.gameObject.GetComponent<FlybotAIScript>().Hurt(missileStrength);
        }
        if (col.gameObject.name == "Groundbot")
        {
            col.gameObject.GetComponent<GroundbotAIScript>().Hurt(missileStrength);
        }

        // If it collides with an enemy, destroy on collision
        if(col.gameObject.tag == "Enemy")
		    Destroy (gameObject);
	}
}
