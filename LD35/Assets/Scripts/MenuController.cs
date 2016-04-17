using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject HowToPlayObject;
    public GameObject OptionsObject;
    public GameObject ExitConfirmationObject;
    public GameObject ExitButton;

    public string NewGameLevel = "";
    public bool HideExitInEditor = false;

    private LevelLoader _levelloader;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WebGLPlayer ||
            Application.platform == RuntimePlatform.WindowsWebPlayer || (HideExitInEditor && Application.isEditor))
        {
            ExitButton.SetActive(false);
        }

        _levelloader = GlobalController.Instance.GetComponentInChildren<LevelLoader>();
    }

    public void OnNewGame()
    {

        if (ExitConfirmationObject.activeInHierarchy)
            return;

        _levelloader.ResetPlayer();
        _levelloader.CreateLevel(0);
    }

    public void OnHowToPlay()
    {
        if (ExitConfirmationObject.activeInHierarchy)
            return;


        HowToPlayObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnOptions()
    {
        if (ExitConfirmationObject.activeInHierarchy)
            return;

        OptionsObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnYesExit()
    {
        Application.Quit();
    }

    public void OnNoExit()
    {
        ExitConfirmationObject.SetActive(false);
    }

    public void OnExit()
    {
        ExitConfirmationObject.SetActive(true);
    }

    public void OnCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
