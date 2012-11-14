using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	#region Public Fields
	public float boundingScale = 0.5f;
	public float turnSpeed = 5f;
	public float sensitivity = 20f;
	//public float minXRotation = -361f;
	//public float maxXRotation = 361f;
	public float minYRotation = -60f;
	public float maxYRotation = 60f;
	//public bool travelBack = true;
	//public float travelCooldown = 1.5f;
	//public float travelSpeed = 0.5f;
	public bool snapToBoundingCircle = false;
	public Texture cursor = null;
	public Camera cameraUsed = null;
	public bool debug = false;
	#endregion
	
	#region Private Fields
	protected float centerX = Screen.width/2;
	protected float centerY =  Screen.height/2;
	protected float mouseX = 0f;
	protected float mouseY = 0f;
	protected float minRadius;
	protected float maxRadius;
	protected int frameCount = 0;
	protected float rotationX;
	protected float rotationY;
	protected float elapsedTime = 0f;
	protected bool travelling;
	#endregion
	
	#region Public Properties
	public Vector2 MousePosition
	{
		get { return new Vector2(mouseX, mouseY); }
		set { mouseX = value.x; mouseY = value.y; }
	}
	
	public Vector2 ScreenMousePosition
	{
		get 
		{ 
			return new Vector2(mouseX, Screen.height - mouseY);
		}
	}
	
	public Vector2 CenterScreen
	{
		get { return new Vector2(centerX, centerY); }
		set { centerX = value.x; centerY = value.y; }
	}
	
	public float MouseDistance
	{
		get { return Vector2.Distance (CenterScreen, MousePosition); }
	}
	
	public Vector2 MouseDirection
	{
		get { return (MousePosition - CenterScreen).normalized; }
	}
	#endregion
	
	public void Start()
	{
		Mathf.Clamp (boundingScale, 0f, 1f);
		
		Screen.lockCursor = true;
		
		MousePosition = CenterScreen;
		
		minRadius = centerY * boundingScale;
		maxRadius = Vector2.Distance (CenterScreen, new Vector2(Screen.width, Screen.height));
		
		rotationX = transform.localEulerAngles.y;
		rotationY = transform.localEulerAngles.x;
		
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	public void Update()
	{
		if (!Screen.lockCursor)
			Screen.lockCursor = true;
		MoveMouse ();
		
		if (MouseDistance >= minRadius)
			RotateSelf ();
	}
	
	public void FixedUpdate()
	{
		elapsedTime++;
		
		if (elapsedTime > 5)
		{
			Debug.Log ("Mouse Position: " + MousePosition);
		}
	}
	
	private Vector2 CalculateSpeed()
	{
		if (!snapToBoundingCircle)
		{
			Vector2 result = Vector2.zero;
			float percent = MouseDistance / maxRadius;
			
			result = (MouseDirection * turnSpeed) * percent;
			
			return result;
		}
		else
		{
			return (MouseDirection * turnSpeed);
		}
	}
	
	private void RotateSelf()
	{
		Vector2 speed = CalculateSpeed ();

		rotationX = transform.localEulerAngles.y + speed.x;
		
		rotationY += speed.y;
		rotationY = Mathf.Clamp (rotationY, minYRotation, maxYRotation);
			
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, 0);
		cameraUsed.transform.localEulerAngles = new Vector3(rotationY, cameraUsed.transform.localEulerAngles.y, 0);
	}

	void MoveMouse ()
	{
		mouseX += Input.GetAxis("Mouse X") * sensitivity;
		mouseX = Mathf.Clamp (mouseX, 0f, (float)Screen.width);
		mouseY += -Input.GetAxis("Mouse Y") * sensitivity;
		mouseY = Mathf.Clamp (mouseY, 0f, (float)Screen.height);
		
		if (snapToBoundingCircle)
		{
			if (MouseDistance > minRadius)
			{
				MousePosition = CenterScreen + MouseDirection * (minRadius + 0.05f);
			}
		}
		
		/*
		if (travelBack)
		{
			if (Input.GetAxis ("Mouse X") == 0 && Input.GetAxis ("Mouse Y") == 0)
			{
				if (!travelling)
				{
					elapsedTime += Time.deltaTime;
					if (elapsedTime >= travelCooldown)
					{
						elapsedTime = 0f;
						travelling = true;
					}
				}
				if (travelling)
				{
					if (MouseDistance > minRadius / 2)
						MousePosition += -MouseDirection * travelSpeed;
					if (MouseDistance <= minRadius/2)
						travelling = false;
				}
			}
			else
			{
				travelling = false;
				elapsedTime = 0f;
			}
		}
		*/
	}
	
	public void OnGUI()
	{
		if (cursor != null)
			GUI.DrawTexture (new Rect(mouseX - (cursor.width), mouseY - (cursor.height), cursor.width * 2, cursor.height * 2), cursor);
		
		if (debug)
		{
			
		}
	}
}

