using UnityEngine;

public class SpeechBox : MonoBehaviour
{
    public string Speaker;
    public string[] Text;
    public Sprite Image;
    public float Delay = 1f;
    public string Name;

    private LevelLoader _levelLoader;
    private PlayerPersistData _playerdata;

    public void Start ()
	{
	    _levelLoader = GlobalController.Instance.GetComponentInChildren<LevelLoader>();
        _playerdata = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();

        if (!_playerdata.CompletedChatBoxes.Contains(Name)) return;
        Destroy(gameObject);
	}

    public void Update()
    {
        if (Delay <= 0f)
        {
            _playerdata.CompletedChatBoxes.Add(Name);
            _levelLoader.StartChatSession(Text, Speaker, Image);
            Destroy(gameObject);
            return;
        }

        Delay -= Time.deltaTime;
    }
	
}
