using System;
using UnityEngine;
using System.Collections;

public class WalkingAI : MonoBehaviour
{
    public float Speed = 1f;
    public float Damage = 1f;
    public float DamageTimeout = 1f;
    public AudioClip SpottedSFX;

    private GameObject target;
    private float _damagetimer = 0f;

    private ActorController _actorController;
    private PlayerPersistData _playerPersistData;
    private SFXPlayer _sfxPlayer;

    public void Start()
    {
        _actorController = GetComponent<ActorController>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
        _sfxPlayer = GetComponent<SFXPlayer>();
    }
	
	public void Update ()
	{
        if (_playerPersistData.GamePaused || _actorController.IsDead)
            return;

        _damagetimer -= Time.deltaTime;

	    if (_damagetimer < 0)
	        _damagetimer = 0;

	    _actorController.Speed = 0;

        if (target == null)
	        return;

        var targetpos = target.transform.position;
        
        if (targetpos.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Speed * Time.deltaTime, transform.position.z);
            _actorController.Speed = Speed;
            _actorController.Direction = 0;
        }

        if (targetpos.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime, transform.position.z);
            _actorController.Speed = Speed;
            _actorController.Direction = 1;
        }

        if (targetpos.x < transform.position.x)
        {
            transform.position = new Vector3(transform.position.x - Speed * Time.deltaTime, transform.position.y, transform.position.z);
            _actorController.Speed = Speed;
            _actorController.Direction = 2;
        }

        if (targetpos.x > transform.position.x)
        {
            transform.position = new Vector3(transform.position.x + Speed * Time.deltaTime, transform.position.y, transform.position.z);

            _actorController.Speed = Speed;
            _actorController.Direction = 3;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (_actorController.IsDead)
            return;

        if (Math.Abs(_damagetimer) > -0f)
            return;

        _damagetimer = DamageTimeout;

        other.SendMessage("OnDMG", Damage);
    }

    public void OnPlayerSpotted(GameObject player)
    {
        if (_actorController.IsDead)
            return;

        target = player;
        _sfxPlayer.PlaySFX(SpottedSFX);
    }

    public void OnPlayerLost()
    {
        target = null;
    }
}
