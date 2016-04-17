using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public AudioClip ShootSFX;
    public AudioClip PlayerDie;
    public float PixelToUnits = 40f;
    public Camera Camera;
    public GameObject Projectile;
    public float MoveSpeed = 1f;
    public float AttackCoolDown = 1f;

    [HideInInspector]
    public string ContextText = "";
    
    private Vector3 _direction = Vector3.down;
    private GameObject _contextObject;
    public PlayerGUIController GuiController;

    private ActorController _actorController;
    private PlayerPersistData _playerPersistData;
    private float _currentAttackCoolDown = 0f;
    private UpgradeTable _upgradeTable;
    private SFXPlayer _sfxPlayer;
    private bool _playedDeadSound;

    public void Start ()
    {
        _actorController = GetComponent<ActorController>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
        _upgradeTable = GlobalController.Instance.GetComponentInChildren<UpgradeTable>();
        _sfxPlayer = GetComponent<SFXPlayer>();
    }

    public void OnContextEnter(GameObject obj)
    {
        _contextObject = obj;
        var controller = obj.GetComponent<InteractableController>();

        if (controller == null)
            return;

        ContextText = controller.InteractableText;
    }

    public void RecalculateStats()
    {
        _playerPersistData.DamageModifier = _upgradeTable.DMGUpgradeModifier[_playerPersistData.DamageRank];
        _playerPersistData.HPMax = _upgradeTable.HPUpgradeModifier[_playerPersistData.HPRank];
        _playerPersistData.HPRechargeRate = _upgradeTable.HPRegenRate[_playerPersistData.HPRank];
        _playerPersistData.SpeedModifier = _upgradeTable.SpeedUpgradeModifier[_playerPersistData.SpeedRank];

        _actorController.MaxHP = _playerPersistData.HPMax;
        _actorController.HP = _actorController.MaxHP;

        _actorController.HPRegenRate = _playerPersistData.HPRechargeRate;
    }

    public void OnContextExit(GameObject obj)
    {
        if (_contextObject == obj)
            _contextObject = null;

        ContextText = "";
    }

    public void Update ()
    {

        if (_contextObject == null)
            ContextText = "";

        _actorController.Speed = 0;

        if (_playerPersistData.DirtyData)
        {
            _playerPersistData.DirtyData = false;
            RecalculateStats();
        }

        if (!_playerPersistData.GamePaused)
        {
            _currentAttackCoolDown -= Time.deltaTime*_playerPersistData.SpeedModifier;
            if (_currentAttackCoolDown < 0f)
                _currentAttackCoolDown = 0f;
        }

        if (_actorController.HP <= 0)
        {
            _playerPersistData.GamePaused = true;
            GuiController.ShowGameOver();

            if (!_playedDeadSound)
            {
                _playedDeadSound = true;
                _sfxPlayer.PlaySFX(PlayerDie);
            }
        }

        if (Input.GetButtonUp("Attack") && !_playerPersistData.GamePaused && _currentAttackCoolDown <= 0f)
	    {
            var obj = Instantiate(Projectile);
	        var controller = obj.GetComponent<ProjectileController>();
	        obj.transform.position = transform.position;
	        controller.Direction = _direction;
	        controller.Dmg = _playerPersistData.DamageModifier;
	        _currentAttackCoolDown = AttackCoolDown;
            _sfxPlayer.PlaySFX(ShootSFX);


        }

        if (Input.GetButtonUp("Use") && _contextObject != null && !_playerPersistData.GamePaused)
        {
            _contextObject.SendMessage("Interact");
        }

        if (Input.GetButtonUp("Menu"))
        {
            _playerPersistData.GamePaused = true;
            GuiController.ShowGameMenuScreen();
        }

	    if (Input.GetAxis("Vertical") > 0.1f && !_playerPersistData.GamePaused)
	    {
	        transform.position = new Vector3(transform.position.x, transform.position.y + (MoveSpeed * _playerPersistData.SpeedModifier) * Time.deltaTime);
            _direction = Vector3.up;
	        _actorController.Speed = MoveSpeed;
	        _actorController.Direction = 1;

	    }

        if (Input.GetAxis("Vertical") < -0.1f && !_playerPersistData.GamePaused)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (MoveSpeed * _playerPersistData.SpeedModifier) * Time.deltaTime);
            _direction = Vector3.down;
            _actorController.Speed = MoveSpeed;
            _actorController.Direction = 0;
        }

        if (Input.GetAxis("Horizontal") > 0.1f && !_playerPersistData.GamePaused)
        {
            transform.position = new Vector3(transform.position.x + (MoveSpeed * _playerPersistData.SpeedModifier) * Time.deltaTime, transform.position.y);
            _direction = Vector3.right;
            _actorController.Speed = MoveSpeed;
            _actorController.Direction = 3;
        }

        if (Input.GetAxis("Horizontal") < -0.1f && !_playerPersistData.GamePaused)
        {
            transform.position = new Vector3(transform.position.x - (MoveSpeed * _playerPersistData.SpeedModifier) * Time.deltaTime, transform.position.y);
            _direction = Vector3.left;
            _actorController.Speed = MoveSpeed;
            _actorController.Direction = 2;
        }

	    var camx = RountToNearestPixel(transform.position.x);
	    var camy = RountToNearestPixel(transform.position.y);

        Camera.transform.position = new Vector3(camx, camy, -32f);

    }

    public float RountToNearestPixel(float unityUnits)
    {
        var valueInPixels = unityUnits * PixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        var roundedUnityUnits = valueInPixels * (1 / PixelToUnits);
        return roundedUnityUnits;
    }
}
