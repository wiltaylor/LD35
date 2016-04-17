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

    public int Direction;
    public float Speed;

    private Animator _animator;
    private PlayerPersistData _playerPersistData;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
    }

    public void Update()
    {
        if (_playerPersistData.GamePaused)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }
            

        if (HP <= 0)
        {
            if (!IsPlayer)
            {
                _playerPersistData.Gold += GoldOnKill;
                Destroy(gameObject);
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
        HP -= dmg;
    }

}
