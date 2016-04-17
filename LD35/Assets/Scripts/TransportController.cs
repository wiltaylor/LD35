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
        var playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();

        if (Type == TransportType.Down)
        {
            if (levelloader.CurrentLevel == 0 && playerPersistData.LowestLevelVisited != 0)
            {
                levelloader.OpenLevelSelect();
            }
            else
            {
                levelloader.CurrentLevel++;
                levelloader.CreateLevel(levelloader.CurrentLevel);
            }

        }
        else
        {
            levelloader.OpenLevelSelect();
        }
    }
}
