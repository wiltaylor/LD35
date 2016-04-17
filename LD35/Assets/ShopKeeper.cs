using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour
{
    public void Interact()
    {
        GlobalController.Instance.GetComponentInChildren<LevelLoader>().OpenShop();
    }
}
