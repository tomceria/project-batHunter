using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : MonoBehaviour {

	public Text DebuggingText;
	public GameObject thePlayer;
	public GameObject CameraObj;
	public GameObject testObj;
	public Camera thecamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Player player = GameObject.Find("Player").GetComponent<Player>();				//Get Player.userInfo
		DebuggingText.text = ("project-batHunter" +
		"\nby Luu Minh Hoang" +
		"\nX: " + Player.playerPositionX + 
		"\nY: " + Player.playerPositionY +
		"\nGame Time: " + Math.Round(LevelManager.gameTime,3) + 
		"\nScreenWidth: " + Screen.width + 
		"\nCamera X: " + CameraObj.transform.position.x + 
		"\n" +
		"\nHeat: " + player.userInfo.heat +
		"\nCard in use: " + player.userInfo.currentCard + 
		"\nOverheat State: " + player.userInfo.overheatState +
		"\nCurrent Card Info:" +
		"\nID: " + player.userInfo.inventory[player.userInfo.currentCard].id +
		"\nLevel: " + player.userInfo.inventory[player.userInfo.currentCard].level + 
		"\nPower: " + player.userInfo.inventory[player.userInfo.currentCard].power +
		"\nProjectile: " + player.userInfo.inventory[player.userInfo.currentCard].projectile +
		"\nDelay Max: " + player.userInfo.inventory[player.userInfo.currentCard].delayMax + 
		"\nDelay: " + player.userInfo.inventory[player.userInfo.currentCard].delay + 
		"\nHeat Cost: " + player.userInfo.inventory[player.userInfo.currentCard].heatDes + 
		"\nHeat Accel Base: " + player.userInfo.inventory[player.userInfo.currentCard].heatAccelBase + 
		"\nHeat Accel: " + player.userInfo.inventory[player.userInfo.currentCard].heatAccel + 
		"\n");
		//Debug.Log (thecamera.ScreenToWorldPoint (CameraObj.transform.position).x);
	}
}
