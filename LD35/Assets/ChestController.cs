using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{

    public int MaxGold = 1000;
    public int MinGold = 500;

    public void Interact()
    {
        GlobalController.Instance.GetComponentInChildren<PlayerPersistData>().Gold += Random.Range(MinGold, MaxGold);
        Destroy(gameObject);
    }
}
