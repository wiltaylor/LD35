using UnityEngine;

public class AIBeacon : MonoBehaviour
{

    public GameObject AI;

    public void OnTriggerEnter2D(Collider2D other)
    {
        AI.SendMessage("OnPlayerSpotted", other.gameObject);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        AI.SendMessage("OnPlayerLost");
    }
}
