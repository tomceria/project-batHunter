using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Camera thecamera;
	public Game.info userInfo;				//Using info struct from Game.cs
	//Touchscreen System declaring
	Vector2 playerPos = new Vector2 (0f, 0f); //Temporary position vector
	Vector2 touchStartPos = new Vector2 (0f, 0f); //Touch starting position vector
	private static int touchSwitch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//TOUCHSCREEN MOVEMENT		
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began && thecamera.ScreenToWorldPoint (Input.GetTouch (0).position).y >= -3.65f && userInfo.heat >= 0) { //Condition to start Touch
			touchSwitch = 1;
			touchStartPos = thecamera.ScreenToWorldPoint (Input.GetTouch (0).position);					//Declaring starting touch Position
		}
		else if (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (0).phase == TouchPhase.Stationary) && touchSwitch == 1 && userInfo.heat >= 0) { //Condition to assign Temporary Position
			Vector2 touchPos = thecamera.ScreenToWorldPoint (Input.GetTouch (0).position);				//Declaring touchPosition, varies throughout the function
			playerPos = playerPos + (touchPos - touchStartPos);
			if (playerPos.y < -3.65f)							// Keep player from going through Ship Console
				playerPos.y = -3.65f;
			else if (playerPos.y > 5f)							// Keep player from going off top edge
				playerPos.y = 5f;
			if (Mathf.Abs(playerPos.x) > 3.79f)					// Keep player from going off the side edges
				playerPos.x = (playerPos.x>0)?3.79f:-3.79f;			
			touchStartPos = touchPos;							// Reset touchStartPos
		} 
		else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) { //Condition to end Temporary Position assigning
			touchSwitch = 0;
		}
		gameObject.transform.position = Vector2.Lerp (gameObject.transform.position, playerPos, 5 * Time.deltaTime); //Player lerping to assigned Temporary Position
		//
	}
}
