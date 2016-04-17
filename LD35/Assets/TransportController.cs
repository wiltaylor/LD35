using UnityEngine;

public class TransportController : MonoBehaviour
{
    public enum TransportType
    {
        Up,
        Down
    }

    public TransportType Type;

    public void Interact()
    {
        var levelloader = GlobalController.Instance.GetComponentInChildren<LevelLoader>();

        if (Type == TransportType.Down)
        {
            levelloader.CurrentLevel++;
            levelloader.CreateLevel(levelloader.CurrentLevel);
        }
        else
        {
            levelloader.OpenLevelSelect();
        }
    }
}
