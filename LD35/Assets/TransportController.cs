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
        
    }
}
