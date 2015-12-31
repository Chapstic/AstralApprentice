using UnityEngine;
using System.Collections;

public class BeeHiveDetectionScript : MonoBehaviour {

    private bool inRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }

    public bool isInRange()
    {
        return inRange;
    }
}
