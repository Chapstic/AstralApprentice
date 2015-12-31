using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

    [HideInInspector]
    public bool facingRight = true;			// For determining which way the player is currently facing.
    // Player state variables
    private GameManager.PlayerState mPlayerState;
    // Reference to player controller script
    public PlayerController mPlayerController;
    // Audio clip variables
    public AudioClip mMissileClip;       
    public AudioClip mSlashClip;       
    public AudioClip mScanClip;        
    public AudioClip mJumpClip;

    // Scan member variables
    public Rigidbody2D ScanTarget;
    private Transform scanSpawn; // Can probably delete this variable and hard code the spot w/ local axis
    private Rigidbody2D scanInstance;

    // Missile member variables
    public float mMissileCD = 1.0f;        // Magic Missile cooldown time
    private float mMissileNextFire = 0.0f; // Game time that magic missile can be fired after
    public Rigidbody2D magicMissile;	   // magicMissile prefab
    private Transform missileSpawn;		   // position from which to spawn missiles
    private float speed = 20f;			   // missile speed

    //    Slash member variables
    public float mSlashCD = 0.5f;        // Magic Missile cooldown time
    private float mSlashNextFire = 0.0f; // Game time that magic missile can be fired after
    public Rigidbody2D mSlashRigidBody;
    private Transform slashSpawn;

    // Sprite animation/switching
    private Animator _animator;

	// Use this for initialization
	void Awake () 
    {
        // Initialize animator
        _animator = GetComponent<Animator>();

        // Initialize player state variables
        facingRight = true;
        mPlayerState = GameManager.instance.GetPlayerState();
        // Initialize components
        mPlayerController = GetComponent<PlayerController>();
        // Setting up references.
        slashSpawn = transform.Find("slashSpawn");
        missileSpawn = transform.Find("missileSpawn");
        scanSpawn = transform.Find("scanSpawn");
	}

    void Update()
    {
        mPlayerState = GameManager.instance.GetPlayerState(); // Check the player's state from game manager
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            facingRight = true;
        }
        // Default state of player is ALIVE. They can Scan, Missile, and Slash
        if (mPlayerState == GameManager.PlayerState.ALIVE)
        {
            // Scanning action, currently bound to Z key.
            if (Input.GetButtonDown("Scan"))
            {
                Scan();
            }
            // Magic missile ability, currently bound to X key. Player must be grounded, and time must be past cooldown
            if (Input.GetButtonDown("MagicMissile") && Time.time > mMissileNextFire)
            {
                Missile();
                // Metrics
                GameManager.instance.mPlayerStats.TotalMissilesFired++;
            }
            //   If the slash button is pressed down (C), slash in front of the player
            if (Input.GetButtonDown("Slash") && Time.time > mSlashNextFire)
            {
                Slash();
                GameManager.instance.mPlayerStats.TotalSlashes++;
            }
        }
        // Player is in scanning mode. Must exit scanning.
        else if (mPlayerState == GameManager.PlayerState.SCAN)
        {
            if (Input.GetButtonDown("Scan"))
            {
                mPlayerController.enabled = true; // Enable player controller script to enable movement
                Destroy(scanInstance);
                SoundManager.instance.PlaySingle(mScanClip);
                GameManager.instance.SetPlayerState(GameManager.PlayerState.ALIVE);
            }
        }
    }

    private void Scan()
    {
        _animator.Play(Animator.StringToHash("Scan"));
        // Disable the player controller script to prevent movement
        mPlayerController.enabled = false; 
        // Create a "scanner" object in the scene
        scanInstance = Instantiate(ScanTarget, scanSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
        scanInstance.transform.parent = transform;
        SoundManager.instance.PlaySingle(mScanClip);
        // Transition to scanning state for player
        GameManager.instance.SetPlayerState(GameManager.PlayerState.SCAN);
    }

    private void Missile()
    {
        mMissileNextFire = Time.time + mMissileCD; // The next time the player can fire a missile is current time + cd value;

        // The line below is supposed to disable movement temporarily while the missile is flying
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        // Spawn a missile depending on direction
        if (facingRight)
        {
            Rigidbody2D missileInstance = Instantiate(magicMissile, missileSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            missileInstance.velocity = new Vector2(speed, 0);
        }
        else
        {
            Rigidbody2D missileInstance = Instantiate(magicMissile, missileSpawn.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
            missileInstance.velocity = new Vector2(-speed, 0);
        }
        // Play missile sound effect
        SoundManager.instance.PlaySingle(mMissileClip);
    }

    private void Slash()
    {
        mSlashNextFire = Time.time + mSlashCD; // The next time the player can fire a missile is current time + cd value;

        if (facingRight)
        {
            Rigidbody2D slashInstance = Instantiate(mSlashRigidBody, slashSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            slashInstance.transform.parent = slashSpawn;
        }
        else
        {
            Rigidbody2D slashInstance = Instantiate(mSlashRigidBody, slashSpawn.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
            slashInstance.transform.parent = slashSpawn;
        }
        SoundManager.instance.PlaySingle(mSlashClip);
    }
}
