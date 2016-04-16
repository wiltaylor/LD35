using System;
using UnityEngine;
using System.Collections;

public class WalkingAI : MonoBehaviour
{
    public float Speed = 1f;
    public float Damage = 1f;
    public float DamageTimeout = 1f;

    private GameObject target;
    private float _damagetimer = 0f;
	
	public void Update ()
	{
	    _damagetimer -= Time.deltaTime;

	    if (_damagetimer < 0)
	        _damagetimer = 0;

        if (target == null)
	        return;

	    var targetpos = target.transform.position;

	    if (targetpos.x < transform.position.x)
	    {
	        transform.position = new Vector3(transform.position.x - Speed * Time.deltaTime, transform.position.y, transform.position.z);
	    }

        if (targetpos.x > transform.position.x)
        {
            transform.position = new Vector3(transform.position.x + Speed * Time.deltaTime, transform.position.y, transform.position.z);
        }

        if (targetpos.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Speed * Time.deltaTime, transform.position.z);
        }

        if (targetpos.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime, transform.position.z);
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (Math.Abs(_damagetimer) > -0.01f)
            return;

        _damagetimer = DamageTimeout;
        other.SendMessage("OnDMG", Damage);
    }

    public void OnPlayerSpotted(GameObject player)
    {
        target = player;
    }

    public void OnPlayerLost()
    {
        target = null;
    }
}
