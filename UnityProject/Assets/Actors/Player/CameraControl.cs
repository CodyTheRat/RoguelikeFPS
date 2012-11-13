using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	#region Public Fields
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float boundingScale = 0.5f;
	public float turnSpeed = 5.0f;
	public float sensitivity = 1.0f;
	public float minX = -359f;
	public float maxX = 360f;
	public float minY = -60f;
	public float maxY = 60f;
	public bool debug = false;
	public Texture cursor = null;
	#endregion
	
	#region Private Fields
	private int centerX = Screen.width/2;
	private int centerY = Screen.height/2;
	private float mouseX;
	private float mouseY;
	private Rect boundingBox;
	private Rect screen;
	private int frameCount = 0;
	#endregion
	
	private Vector2 MousePosition
	{
		get { return new Vector2(mouseX, mouseY); }
		set { mouseY = value.y; mouseX = value.y; }
	}
	
	void Start () 
	{
		if (boundingScale > 1f)
			boundingScale = 1f;
		if (boundingScale < 0f)
			boundingScale = 0f;
		boundingBox = CalculateBBox ();
		
		MousePosition = new Vector2(centerX, centerY);
		
		Screen.lockCursor = true;
		
		if (debug)
			LogSize ();
	}
	
	void Update () 
	{
		if (debug)			
			DebugUpdate ();
		
		if (centerX != Screen.width/2 || centerY != Screen.height/2)
		{
			centerX = Screen.width/2;
			centerY = Screen.height/2;
			boundingBox = CalculateBBox ();
			if (debug)
				LogSize ();
		}
		
		#region Cursor Movement
		mouseX += Input.GetAxis ("Mouse X") * sensitivity;
		if (mouseX < 0)
			mouseX = 0;
		if (mouseX > Screen.width)
			mouseX = Screen.width;
		
		mouseY += -Input.GetAxis ("Mouse Y") * sensitivity;
		if (mouseY < 0)
			mouseY = 0;
		if (mouseY > Screen.height)
			mouseY = Screen.height;
		#endregion
		
		transform.Rotate (CalculateSpeed (axes));
	}
	
	void FixedUpdate()
	{		
		#region Debug
		if (debug)
		{
			frameCount++;
			if (frameCount > 5)
			{
				Debug.Log("Mouse Outside Bounds: " + MouseOutsideBounds ());
				frameCount = 0;
				if (Screen.lockCursor == false)
					Screen.lockCursor = true;
			}
		}
		#endregion
	}
	
	void DebugUpdate ()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			if (boundingScale > 0)
				boundingScale -= 0.05f;
			
			boundingBox = CalculateBBox ();
			LogSize ();
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			if (boundingScale < 1)
				boundingScale += 0.05f;
			
			boundingBox = CalculateBBox ();
			LogSize ();
		}
		if (Input.GetAxis ("Fire1") != 0)
		{
			//TODO: DoDebugStuff();
		}
	}
	
	void OnGUI ()
	{
		if (debug)
			drawDebugGUI ();
		
		if (cursor != null)
			GUI.DrawTexture (new Rect(mouseX - (cursor.width), mouseY - (cursor.height), cursor.width * 2, cursor.height * 2), cursor);
	}
	
	private void drawDebugGUI ()
	{
		GUI.Box(boundingBox, "");
	}
	
	public bool MouseOutsideBounds()
	{
		if (boundingBox.Contains (MousePosition))
			return false;
		else
			return true;
	}
	
	private Vector3 CalculateSpeed(RotationAxes axes)
	{
		Vector3 result = Vector3.zero;
		if (!MouseOutsideBounds ())
			return result;
		
		Vector3 turnSpeed = new Vector3(this.turnSpeed, this.turnSpeed, 0);
		
		switch(axes)
		{
		case RotationAxes.MouseXAndY:
			return result;
		case RotationAxes.MouseX:
			return result;
		case RotationAxes.MouseY:
			return result;
		}
		
		return result;
	}
	
	private void LogSize()
	{
			Debug.Log ("CenterX: " + centerX);
			Debug.Log ("CenterY: " + centerY);
			Debug.Log ("BoundingBox: " + CalculateBBox ());
	}
	
	private Rect CalculateBBox()
	{
		screen = new Rect(0, 0, Screen.width, Screen.height);
		return new Rect(centerX - (centerX * boundingScale), centerY - (centerY * boundingScale), //X, Y
						(centerX * boundingScale) * 2, (centerY * boundingScale) * 2); //Width, Height	
	}
}
