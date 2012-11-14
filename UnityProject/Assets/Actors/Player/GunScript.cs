using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour 
{
	#region Public Fields
	public CameraMovement cameraScript;
	#endregion
	
	#region Private Fields
	
	#endregion
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		TransformGun ();
	}

	void TransformGun ()
	{
		Ray ray = cameraScript.cameraUsed.ScreenPointToRay (cameraScript.ScreenMousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit))
			transform.LookAt(hit.point);
		else
			transform.LookAt (ray.origin  + ray.direction * 1000);
	}
}
