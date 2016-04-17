using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndTriangle : MonoBehaviour
{
    public void Interact()
    {
        SceneManager.LoadScene("Credits");
    }
}
