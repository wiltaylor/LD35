using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    public float TimeToLive = 10f;
    public float Dmg = 10f;
    public float Speed = 10f;
    public Vector3 Direction;

    private Rigidbody2D _rigidbody;
    private PlayerPersistData _playerPersistData;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();
    }

    public void Update()
    {
        if (_playerPersistData.GamePaused)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        TimeToLive -= Time.deltaTime;
        if (TimeToLive <= 0)
        {
            DestroyObject(gameObject);
            return;
        }

        _rigidbody.AddRelativeForce(Direction * Speed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Actor")
        {
            other.SendMessage("OnDMG", Dmg);
            DestroyObject(gameObject);
        }

        if (other.tag == "Level")
        {
            DestroyObject(gameObject);
        }
    }
}
