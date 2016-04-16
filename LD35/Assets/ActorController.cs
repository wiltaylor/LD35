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

    public void Update()
    {
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
    }

    public void OnDMG(int dmg)
    {
        HP -= dmg;
    }

}
