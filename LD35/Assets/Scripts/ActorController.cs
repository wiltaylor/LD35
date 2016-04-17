using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour
{
    public float HP;
    public float MaxHP;
    public float Mana;
    public float MaxMana;
    public float ManaRegenRate;
    public float HPRegenRate;
    public bool IsPlayer;
    public int GoldOnKill;
    public float DeathTimeout = 2f;
    public int Direction;
    public float Speed;
    public AudioClip DieSound;
    public AudioClip HitSound;

    private Animator _animator;
    private PlayerPersistData _playerPersistData;
    private Collider2D _collider;

    [HideInInspector]
    public bool IsDead;
    private SFXPlayer _sfxPlayer;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
        _sfxPlayer = GetComponent<SFXPlayer>();
        _collider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if (_playerPersistData.GamePaused || IsDead)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }

        if (HP <= 0)
        {
            if (!IsPlayer)
            {
                _playerPersistData.Gold += GoldOnKill;
                Destroy(gameObject, DeathTimeout);
                IsDead = true;
                _sfxPlayer.PlaySFX(DieSound);
                _animator.SetTrigger("Death");
                _collider.enabled = false;
                return;
            }
        }

        HP += HPRegenRate * Time.deltaTime;
        Mana += ManaRegenRate*Time.deltaTime;

        if (HP > MaxHP)
            HP = MaxHP;

        if (Mana > MaxMana)
            Mana = MaxMana;


        if (_animator == null)
            return;

        _animator.SetFloat("Speed", Speed);
        _animator.SetInteger("Direction", Direction);
    }

    public void OnDMG(int dmg)
    {
        _sfxPlayer.PlaySFX(HitSound);
        HP -= dmg;
    }

}
