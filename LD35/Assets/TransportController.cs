using UnityEngine;
using System.Collections;

public class TransportController : MonoBehaviour
{
    public enum TransportType
    {
        ToTown,
        Up,
        Down
    }

    public TransportType Type;

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("OnContextEnter", gameObject);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        other.SendMessage("OnContextExit", gameObject);
    }

    public void Interact()
    {
        var levelloader = GlobalController.Instance.GetComponentInChildren<LevelLoader>();

        levelloader.CurrentLevel++;
        levelloader.CreateLevel(levelloader.CurrentLevel);
    }
}
