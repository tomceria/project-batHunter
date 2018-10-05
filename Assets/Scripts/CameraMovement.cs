using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Camera thecamera;
	public GameObject CameraObj;
	public GameObject PlayerObj;
	public GameObject RightEdgeObj;
	private GameObject ScreenRightEdge;

	public static float CameraPosX;
	public static float PlayerOnCameraX;
	public static float CameraPosXPercent;
	public static float PlayerOnCameraXPercent;

	public static float MidToLeft;
	public static float MidToRight;
	public static float stuckLeft;
	public static float stuckRight;
	public static bool stuckLeftCheck;
	public static bool stuckRightCheck;
	private static float temporarySwitch;
	private static bool ScreenRightCheck;


	private int fullscreened = 0;

	// Use this for initialization
	void Start () {
		ScreenRightCheck = false;
		stuckLeftCheck = false;
		stuckRightCheck = false;
		if (thecamera.aspect == 0.75f)				//check if screen ratio is 3:4
			fullscreened = 1;
	}

	// Update is called once per frame
	void Update () {
		//SCALE CAMERA POSITION WITH BOUNDARIES
		PlayerOnCameraX = thecamera.WorldToViewportPoint(PlayerObj.transform.position).x;
		PlayerOnCameraXPercent = PlayerOnCameraX / 1;
		temporarySwitch = PlayerOnCameraXPercent;
		CameraPosXPercent = temporarySwitch;
		CameraPosX = CameraPosXPercent * 2.6f - 1.3f; //Max bound is 1.3, support 1:2 screens
		CameraObj.transform.position = new Vector3 (CameraPosX, CameraObj.transform.position.y, -10);

		//INSTANTIATE RIGHT EDGE CHECK AT START
		if (ScreenRightCheck == false) {
			ScreenRightEdge = Instantiate (RightEdgeObj, new Vector2(thecamera.ScreenToWorldPoint (CameraObj.transform.position).x, 0), Quaternion.identity) as GameObject;
			ScreenRightEdge.transform.position = new Vector2 (ScreenRightEdge.transform.position.x * -1, 0);
			ScreenRightCheck = true;
		}

		//STUCK RIGHT EDGE CHECK WITH CAMERA
		ScreenRightEdge.transform.parent = CameraObj.transform;

		//CACULATE DISTANCE BETWEEN SCREEN EDGES AND SCREEN CENTER (ASPECT RATIO SCALING)
		MidToLeft = thecamera.ScreenToWorldPoint (CameraObj.transform.position).x;
		MidToRight = Vector2.Distance(new Vector2 (0,CameraObj.transform.position.y) , ScreenRightEdge.transform.position);

		//BOUNDARIES
		if (fullscreened == 1) {					//Lock screen in place if screen ratio is 3:4
			CameraObj.transform.position = new Vector3 (0, CameraObj.transform.position.y, CameraObj.transform.position.z);
		}

		else if (MidToLeft <= -3.75f) {			
			if (stuckLeftCheck == false) {
				stuckLeft = CameraObj.transform.position.x;
				stuckLeftCheck = true;
			} 
			else if (stuckLeftCheck == true) {
				CameraObj.transform.position = new Vector3 (stuckLeft, CameraObj.transform.position.y, CameraObj.transform.position.z);
			}
		}
		else if (MidToRight >= 3.75f) {
			if (stuckRightCheck == false) {
				stuckRight = CameraObj.transform.position.x;
				stuckRightCheck = true;
			} 
			else if (stuckRightCheck == true) {
				CameraObj.transform.position = new Vector3 (stuckRight, CameraObj.transform.position.y, CameraObj.transform.position.z);
			}
		}
		else {
			stuckLeftCheck = false;
			stuckRightCheck = false;
		}


		//Debug.Log ("A= " + CameraPosX + "\nB= " + PlayerOnCameraX);
		//Debug.Log ("A%= " + CameraPosXPercent + "\nB%= " + PlayerOnCameraXPercent);

		/* NOTE FOR FUTURE PREFERENCES
		a = CameraX = thecamera.transform.position.x //Camera position (-2 -> 2)
		b = PlayerOnCameraX = thecamera.WorldToViewportPoint(Player.transform.position).x //Player on camera (0 -> 1) 
		a = -1	b = 0
		a = 0	b = 0.5
		a = 1	b = 1

		Amin = 0;
		Amax = 2;
		Bmin = 0;
		Bmax = 1;


		Bpercentage = b / Bmax     Ex: 0.5 / 1 = 0.5
		Bpercentage = Apercentage
		Apercentage = (a+2) / Amax      Ex: 0.5 = (a+1) / 2 => a =0
		=> a = Apercentage * Amax - 1
		*/


	}
}
