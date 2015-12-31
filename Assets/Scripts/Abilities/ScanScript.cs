using UnityEngine;
using System.Collections;

public class ScanScript : MonoBehaviour {

    public float scanSensitivity;
    private AudioClip SuccessSound;

    private bool onResource;
    private GameObject targetResource;

    void Start()
    {
        onResource = false;
        gameObject.layer = 0; // Initialize layer to default so resources trigger properly
        SuccessSound = Resources.Load("Audio/Scanning/SuccessMallets") as AudioClip;
        scanSensitivity = GameManager.instance.mPlayerStats.GetScanSpeed();
    }

	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Translate scanner based on horizontal
        transform.Translate(scanSensitivity * h * Time.deltaTime, 0.0f , 0.0f);
        // Translate scanner based on vertical
        transform.Translate(0.0f, scanSensitivity * v * Time.deltaTime, 0.0f);

        if (Input.GetButtonDown("Scan"))
        {
            if (onResource && targetResource) // Player scanned successfully and nullcheck
            {
                if(!targetResource.GetComponent<ResourceBehavior>().GetScanned()) // Has this resource been scanned?
                {
                    targetResource.GetComponent<ResourceBehavior>().Scan();
                    if (SuccessSound) // nullcheck
                        SoundManager.instance.PlaySingle(SuccessSound);
                }
            }
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Resource")
        {
            onResource = true;
            Debug.Log("On resource");
            targetResource = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Resource")
        {
            onResource = false;
            Debug.Log("Off resource");
            targetResource = null;
        }
    }
}
