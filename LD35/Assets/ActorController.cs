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
    public int XPOnKill;

    public int Direction;
    public float Speed;

    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        Debug.Log(string.Format("Speed: {0} Direction: {1}", Speed, Direction));

        if (HP <= 0)
        {
            if (!IsPlayer)
            {
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
