using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    public float TimeToLive = 10f;
    public float Dmg = 10f;
    public float Speed = 10f;
    public Vector3 Direction;
    public float CreationOffset = 0.32f;

    private Rigidbody2D _rigidbody;
    private PlayerPersistData _playerPersistData;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerPersistData = GlobalController.Instance.GetComponentInChildren<PlayerPersistData>();

        if(Direction == Vector3.zero)
            Direction = Vector3.down;

        if(Direction == Vector3.down)
            transform.position = new Vector3(transform.position.x, transform.position.y - CreationOffset, transform.position.z);

        if (Direction == Vector3.up)
            transform.position = new Vector3(transform.position.x, transform.position.y + CreationOffset, transform.position.z);

        if (Direction == Vector3.left)
            transform.position = new Vector3(transform.position.x - CreationOffset, transform.position.y, transform.position.z);

        if (Direction == Vector3.right)
            transform.position = new Vector3(transform.position.x + CreationOffset, transform.position.y, transform.position.z);
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
