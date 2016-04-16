using System;
using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


    public float PixelToUnits = 40f;
    public Camera Camera;

    public void Start ()
    {
	
	}
	
	public void Update ()
    {


	    if (Input.GetAxis("Vertical") > 0.1f)
	    {
	        transform.position = new Vector3(transform.position.x, transform.position.y + 1f * Time.deltaTime);
	    }

        if (Input.GetAxis("Vertical") < -0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1f * Time.deltaTime);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.position = new Vector3(transform.position.x + 1f * Time.deltaTime, transform.position.y);
        }

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.position = new Vector3(transform.position.x - 1f * Time.deltaTime, transform.position.y);
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
