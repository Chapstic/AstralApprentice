using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour {

    private Transform HealthSpawn;
    public Canvas pHealthBar;	   // Health bar prefab
    private Rigidbody2D mHealthObj;      // Instantiated health bar

	// Use this for initialization
	public virtual void Awake () {
        HealthSpawn = transform.Find("healthSpawn");
        if(HealthSpawn)
        {
            Debug.Log("Found health spawn");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected abstract void Move();

    // public abstract void TakeDamage();

    void UpdateHealth()
    {

    }
}
