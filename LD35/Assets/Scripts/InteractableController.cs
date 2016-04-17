using UnityEngine;
using System.Collections;

public class InteractableController : MonoBehaviour
{
    public string InteractableText;

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("OnContextEnter", gameObject);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        other.SendMessage("OnContextExit", gameObject);
    }
}
