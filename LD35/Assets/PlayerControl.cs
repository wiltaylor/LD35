using UnityEngine;

public class PlayerControl : MonoBehaviour {


    public float PixelToUnits = 40f;
    public Camera Camera;
    public GameObject Projectile;
    public float MoveSpeed = 1f;
    
    private Vector3 _direction;
    private GameObject _contextObject;

    private ActorController _actorController;

    public void Start ()
    {
        _actorController = GetComponent<ActorController>();
    }

    public void OnContextEnter(GameObject obj)
    {
        _contextObject = obj;
    }

    public void OnContextExit(GameObject obj)
    {
        if (_contextObject == obj)
            _contextObject = null;
    }

    public void Update ()
    {
        _actorController.Speed = 0;


        if (Input.GetButtonUp("Attack"))
	    {
            var obj = Instantiate(Projectile);
	        var controller = obj.GetComponent<ProjectileController>();
	        obj.transform.position = transform.position;
	        controller.Direction = _direction;
	    }

        if (Input.GetButtonUp("Use") && _contextObject != null)
        {
            _contextObject.SendMessage("Interact");
        }

	    if (Input.GetAxis("Vertical") > 0.1f)
	    {
	        transform.position = new Vector3(transform.position.x, transform.position.y + MoveSpeed * Time.deltaTime);
            _direction = Vector3.up;
	        _actorController.Speed = MoveSpeed;
	        _actorController.Direction = 1;

	    }

        if (Input.GetAxis("Vertical") < -0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - MoveSpeed * Time.deltaTime);
            _direction = Vector3.down;
            _actorController.Speed = MoveSpeed;
            _actorController.Direction = 0;
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.position = new Vector3(transform.position.x + MoveSpeed * Time.deltaTime, transform.position.y);
            _direction = Vector3.right;
            _actorController.Speed = MoveSpeed;
            _actorController.Direction = 3;
        }

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.position = new Vector3(transform.position.x - MoveSpeed * Time.deltaTime, transform.position.y);
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
