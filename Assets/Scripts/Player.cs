using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//Assigning GameObject
	public Camera thecamera;
    public GameObject projectile;
	public GameObject laser;
	public Animator shipstateAnim;			//Animator for "ShipState"
	public AudioSource sysAudioSource;

	public Game.info userInfo;				//Using info struct from Game.cs
	
	//Declaring Player's position
	public static float playerPositionX;
	public static float playerPositionY;
	private int firingState;
	private int coolingProcessAwait;

	// Use this for initialization
	void Start () {
		
		//Player initial stat
		userInfo.inventory = new WeaponDB.weapon[20];				//Initiating Player's card inventory
		WeaponDB.getCard("cone-stick", ref userInfo, 1, 1);
		WeaponDB.getCard("laser-once", ref userInfo, 2, 1);
		userInfo.type = 0;
		userInfo.hp = 100;
		gameObject.tag = "Player";
		userInfo.currentCard = 1;
		userInfo.heatMax = 200;
		userInfo.heat = userInfo.heatMax;
		userInfo.barrelPos = new Vector3 (0, 0.25f, 0);
		userInfo.weakShot = 0;
		
    }

	private void touchControl () {

	}
	
	// Update is called once per frame
	void Update () {


		

		//PLAYER POSITION
		playerPositionX = transform.position.x;
		playerPositionY = transform.position.y;

        //BASIC MOVEMENT
        /*
		Vector2 mousePos = thecamera.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, mousePos, 50 * Time.deltaTime); */

        //PROJECTILE FIRING (HOLDING)
		if (firingState == 1) {
			if (userInfo.inventory[userInfo.currentCard].delay <= 0 && userInfo.overheatState == 0 && (userInfo.weakShot == 0 || (userInfo.weakShot == 1 && userInfo.heat >= userInfo.inventory[userInfo.currentCard].heatDes))) {
				WeaponDB weaponDB = GameObject.Find("GameMaster").GetComponent<WeaponDB>();			//get components from the "WeaponDB" script in the GameObject "GameMaster"
				weaponDB.useCard (ref userInfo, userInfo.currentCard, gameObject);
				//WEAPON OVERHEAT TRIGGER
				if (userInfo.heat < 0) {
					//WeaponOverheated ();			TEMPORARY REMOVED
					userInfo.heat = 0;
				}
            }
        }

		//IN OVERHEAT STATE
		else if (userInfo.overheatState == 1) {
			if (userInfo.heat >= userInfo.heatMax) {		//Get out of overheat state when Heat reaches Max
				userInfo.overheatState = 0;
			}
		}

		// CONSOLE SHIP STATE (ANIMATOR)
		if (Mathf.RoundToInt(userInfo.heat) >= 0 && userInfo.heat < userInfo.heatMax && userInfo.overheatState == 1 && coolingProcessAwait == 1) {		//Heat reaches 0 => Initialize cooling process
			shipstateAnim.SetBool("startCooling", true);
			SystemAudioDB systemaudiodb = GameObject.Find("GameMaster").GetComponent<SystemAudioDB>();				//non-static void => get component required
			systemaudiodb.playSFX (sysAudioSource, "overheat2");
			coolingProcessAwait = 0;
		}
		if (userInfo.heat >= userInfo.heatMax) {
			shipstateAnim.SetBool("startCooling", false);
			shipstateAnim.SetBool("isCooling", false);
		}
		//

		//UPDATE PER FRAMES: WEAPON DELAY DECREASES (Can only shoot when Delay reaches 0)
		userInfo.inventory[userInfo.currentCard].delay -= Time.deltaTime * 60;
		if (userInfo.inventory[userInfo.currentCard].delay < 0)
			userInfo.inventory[userInfo.currentCard].delay = 0;
		//

		//UPDATE PER FRAMES: WEAPON COOLDOWN (OVERHEAT SYSTEM)
		if (firingState==0 || userInfo.weakShot==1)
			userInfo.heat += 15 *Time.deltaTime;
		if (userInfo.heat > userInfo.heatMax)	
			userInfo.heat = userInfo.heatMax;
		//

		//UPDATE PER FRAMES: WEAKSHOT checked
		if (userInfo.heat > userInfo.inventory[userInfo.currentCard].heatDes + 1) {
			userInfo.weakShot = 0;
		}
		else if (userInfo.heat <= 0)
			userInfo.weakShot = 1;
		//
	
	}

	public void OnFire() {
		if (userInfo.heat <= 0)
			firingState = 0;
		else
			firingState = 1;
	}

	public void OffFire() {
		firingState = 0;
		// PROJECTILE FIRING (TAPPING)
		userInfo.inventory[userInfo.currentCard].delay /= 2f;
		userInfo.inventory[userInfo.currentCard].heatAccel = 1;

	}

	//(TEMPORARY) Item Selection
	public void SelectItem1() {
		userInfo.currentCard = 1;
	}
	public void SelectItem2() {
		userInfo.currentCard = 2;
	}

	//DEBUG: Level Up
	public void WeaponLevelUp() {
		WeaponDB.getCard(userInfo.inventory[userInfo.currentCard].id, ref userInfo, userInfo.currentCard, 1);
	}

	//WEAPON OVERHEAT
	public void WeaponOverheated() {					//TODO: whole system overheat (apply to all cards in inventory)
		userInfo.heat = -100;
		userInfo.overheatState = 1;
		coolingProcessAwait = 1;						//Trigger for ConsoleState-Cooling
		shipstateAnim.SetTrigger("Overheat");
		shipstateAnim.SetBool("isCooling", true);
		SystemAudioDB systemaudiodb = GameObject.Find("GameMaster").GetComponent<SystemAudioDB>();				//non-static void => get component required
		systemaudiodb.playSFX (sysAudioSource, "overheat1");
	}
    
}
