using UnityEngine;
using System.Collections;

public class AIBeacon : MonoBehaviour
{

    public GameObject AI;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Target Aquired!");
        AI.SendMessage("OnPlayerSpotted", other.gameObject);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Target Lost!");
        AI.SendMessage("OnPlayerLost");
    }
}
