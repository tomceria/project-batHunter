using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Game.info userInfo;				//Using info struct from Game.cs

	public float screenEdge = 4.5f;
	public string enemyID;
	public string enemyBatch;
	private System.Random rnd = new System.Random();
	public float[] enemyMod = new float[10];		//Value determined by EnemySpawner to change batch's behaviour
	public float[] enemyVar = new float[10];		//Value changing throughout the process for batch's functions
	public static int enemyCount = 0;

	// Use this for initialization
	
	void Start () {
		enemyCount++;
		Initiate ();
	}
	void Initiate () {
		// Enemy properties
		userInfo.type = 2;
		userInfo.inventory = new WeaponDB.weapon[20];				//Initiating Enemy's card inventory
		gameObject.tag = "Enemy";
		userInfo.currentCard = 1;				//TEMP
		userInfo.heatMax = 100;
		//startFirePattern (int slotID, int gap, int randomGap, int continuous, int continuousGap, int randomContinuous)
		switch (enemyID) {
			// TODO: all cardLevel = stageLevel
			case "tameBat": {				// Tame Bat
				WeaponDB.getCard("enemy-cone-stick", ref userInfo, 1, 1);
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				StartCoroutine (startFirePattern (1, 1000, 7000, 1, 0, 0));
				break;
			}
			case "heatemitBat": {				// Heat-Emit Bat
				WeaponDB.getCard("enemy-surround-ball", ref userInfo, 1, 8);
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, 0, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.red;
				StartCoroutine (startFirePattern (1, 1000, 10000, 3, 100, 0));
				break;
			}
			case "accurateBat": {				// Accurate Bat
				WeaponDB.getCard("enemy-cone-ball-targetPlayer", ref userInfo, 1, 1);
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				StartCoroutine (startFirePattern (1, 1000, 7000, 1, 0, 0));
				break;
			}
			case "burstyBat": {				// Bursty Bat
				WeaponDB.getCard("enemy-burst-ball-targetPlayer", ref userInfo, 1, 6);
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
				StartCoroutine (startFirePattern (1, 5000, 7000, 1, 0, 0));
				break;
			}
			case "random3Bat": {				// Random3 Bat
				WeaponDB.getCard("enemy-burst-ball-targetPlayer", ref userInfo, 1, 4);
				userInfo.hp = 3f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.green;
				StartCoroutine (startFirePattern (1, 2000, 8000, 2, 250, 0));
				break;
			}
			case "tankyannoyingBat": {				// Tanky annoying bat
				WeaponDB.getCard("enemy-burst-ball-targetPlayer", ref userInfo, 1, 6);
				userInfo.hp = 20f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
				StartCoroutine (startFirePattern (1, 3000, 0, 3, 250, 0));
				break;
			}
			case "surroundBat": {				// Surround bat
				WeaponDB.getCard("enemy-surround-ball", ref userInfo, 1, 6);
				userInfo.hp = 3f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
				StartCoroutine (startFirePattern (1, 3000, 7000, 1, 0, 0));
				break;
			}
		}
		//userInfo.heat = userInfo.heatMax;
		//

		// Enemy movement variable (Startup)
		switch (enemyBatch) {
			// Zig-Zag Approaching
			case "autoZigzagPlayer": {
				//enemyVar[1]: player position x
				//enemyVar[2]: player position y
				enemyVar[3] = 1000;					// Increasing time, instantly move after spawning
				enemyVar[5] = 0;					//5; 6: Random value, enemy move toward near player
				enemyVar[6] = 0;
				break;
			}
			// Random movement
			case "autoRandomXY": {
				//enemyVar[1]: destination position x
				//enemyVar[2]: destination position y
				enemyVar[3] = 1000;					// Increasing time, instantly move randomly after spawning
				enemyVar[4] = 0;					// Refresh rate time gap. Starts as 0 as enemy moves immediately after spawning, update afterward
				break;
			}

			// Sin-Wave & Curve:
			case "sinRightDes":
			case "sinLeftDes":
			case "sinRightAsc":
			case "sinLeftAsc":
			case "sinDown":
			case "curveRight":
			case "curveLeft": {		//Apply for y=sin(x), y=(x/a)^b
				enemyVar[1] = 0;		//static x
				enemyVar[2] = 0;		//static y
				enemyVar[3] = transform.position.x;			//Spawn position; both static x and y will calculate y=sin(x) separately, + currentPos afterward in here
				enemyVar[4] = transform.position.y;
				break;
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		/*
		DEMOshootTimer += 30 * Time.deltaTime;
		if (DEMOshootTimer >= 30) {
			WeaponDB weaponDB = GameObject.Find("GameMaster").GetComponent<WeaponDB>();
			weaponDB.useCard (ref userInfo, userInfo.currentCard, gameObject);
			DEMOshootTimer = 0;
		}
		*/
		// TEMP
		userInfo.heat = userInfo.heatMax;

		//Enemy movement
		switch (enemyBatch) {

			/*
			AUTO PATHING
			mod 1: Movement speed
			*/
			// Straight-up Approaching Player
			case "autoGotoPlayer": {
				Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
				transform.position = Vector2.MoveTowards (gameObject.transform.position, playerComponent.transform.position, enemyMod[1] * Time.deltaTime);
				
			}
			break;
			// Approaching Zig-Zag Style (Random near player)
			// (The goal is to make enemy move towards the outline of player's radius, radius reduce every step)
			/*
			mod 2: Refresh rate (second)
			mod 3: Player approaching radius
			mod 4: Player approaching radius' decrement
			Default: 5, 0.5, 3, 0.5
			*/
			case "autoZigzagPlayer": {
				Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
				enemyVar[3] += 1f * Time.deltaTime;				// Increase by time, reset when reaching refresh rate time
				if (enemyVar[3] >= enemyMod[2]) {		// Start immediately after spawning (enemyVar[3] = 1000)
					enemyMod[3] -= enemyMod[4];			// Decrease radius (WARNING: ENEMYMOD IS NOT SUPPOSED TO BE CHANGED THROUGHOUT THE PROCESS, TO BE CHANGED)
					// Update player's position
					enemyVar[1] = playerComponent.transform.position.x;
					enemyVar[2] = playerComponent.transform.position.y;
					// Update random position
					enemyVar[5] = rnd.Next (-2, 2);
					enemyVar[6] = rnd.Next (-2, 2);
					//Reset timer
					enemyVar[3] = 0;
				}
				// Approach player until reaching player's radius (calculate by distance between enemy and player)
				if (Vector2.Distance(transform.position, new Vector2 (enemyVar[1], enemyVar[2])) > enemyMod[3]) {
					transform.position = Vector2.MoveTowards (gameObject.transform.position, new Vector2 (enemyVar[1] + enemyVar[5], enemyVar[2] + enemyVar[6]), enemyMod[1] * Time.deltaTime);
				}
			}
			break;
			// Random movement
			/*
			mod 2: Refresh rate (second)
			mod 3: Refresh rate random gap (second)
			mod 4: Max random X
			mod 5: Max random Y
			Default: 1, 2, 1, 5, 5
			*/
			case "autoRandomXY": {
				enemyVar[3] += 1f * Time.deltaTime;				// Increase by time, reset when reaching refresh rate time
				if (enemyVar[3] > enemyMod[2] + enemyVar[4]) {		// Check if Time > Refresh Rate + Refresh Rate Random Gap (enemyVar4)
					// Update destination position
					do {	// Do boundaries check
						enemyVar[1] = transform.position.x + rnd.Next (-1 * (int)enemyMod[4], (int)enemyMod[4]);
						enemyVar[2] = transform.position.y + rnd.Next (-1 * (int)enemyMod[5], (int)enemyMod[5]);
					}
					while (boundaryCheck (enemyVar[1], enemyVar[2]) == 0);
					// Update Refresh rate random gap
					int rrRandomGap = (int)(enemyMod[3] * 1000);			// Convert to milisecond (avoid INT restriction of rnd.Next)
					enemyVar[4] = rnd.Next (0, rrRandomGap) / 1000f;		// Convert to second
					//Reset timer
					enemyVar[3] = 0;
				}
				transform.position = Vector2.MoveTowards (gameObject.transform.position, new Vector2 (enemyVar[1], enemyVar[2]), enemyMod[1] * Time.deltaTime);
			}
			break;

			/* 
			APPROACH FROM EDGE - LINKED WITH "RANDOM MOVEMENT"
			mod 0: Halt position (on X-axis if Left/Right or Y-axis if Top/Bottom)
			mod 1: Movement speed
			mod 2, 3, 4, 5 same as Random movement
			*/
			// Approaching from Top / Bottom
			case "linkedToY-autoRandomXY": {
				transform.position = Vector2.Lerp (transform.position, new Vector2 (transform.position.x, enemyMod[0]), enemyMod[1] * Time.deltaTime);
				if (transform.position.y <= (enemyMod[0] + 0.25f)) {
					enemyBatch = "autoRandomXY";
					Initiate();
				}
			}
			break;
			// Approaching from Left / Right edges
			case "linkedToX-autoRandomXY": {
				transform.position = Vector2.Lerp (new Vector2 (transform.position.x, enemyMod[0]), transform.position, enemyMod[1] * Time.deltaTime);
				if (transform.position.y <= (enemyMod[0] + 0.25f)) {
					enemyBatch = "autoRandomXY";
					Initiate();
				}
			}
			break;

			/*
			STRAIGHT LINE
			mod 1: Vertical movement speed			//Vertical movement speed = 0 => straight horizontal movement
			mod 2: Descending speed					//Descending speed = 0 => straight vertical movement
			Default: 1, 0.1
			*/
			// Straight Line: Left to Right
			case "straightRight": {						
				transform.position = new Vector2 (transform.position.x + (enemyMod[1] * Time.deltaTime), transform.position.y - (enemyMod[2] * Time.deltaTime));
				if (transform.position.x > screenEdge)
					enemyBatch = "straightLeft";			// Switch direction
			}
			break;
			// Straight Line: Right to Left
			case "straightLeft": {
				transform.position = new Vector2 (transform.position.x - (enemyMod[1] * Time.deltaTime), transform.position.y - (enemyMod[2] * Time.deltaTime));
				if (transform.position.x < -screenEdge)
					enemyBatch = "straightRight";			// Switch direction
			}
			break;

			/* 
			SIN-WAVE
			mod 1: Movement speed
			mod 2: Negative-wavelength (k in "sin(kx)")
			mod 3: Wave height (a in "a*sin(kx)")		//Negative height => change direction
			Default: 1, 5, 0.5
			*/
			// Sin-Wave: Left to Right - Pseudo-Descending
			case "sinRightDes": {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);								// increases x through time
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);			// y = a*sin(kx)
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];					// Update temporary position
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position

				//If out-of-bound and reaching lower-half of og position (help descending) => Reset and change direction
				if (transform.position.x > screenEdge && transform.position.y < enemyVar[4] - (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch = "sinLeftDes";
				}
			}
			break;
			// Sin-Wave: Right to Left - Pseudo-Descending
			case "sinLeftDes": {
				enemyVar[1] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];

				if (transform.position.x < -screenEdge && transform.position.y < enemyVar[4] - (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch = "sinRightDes";
				}
			}
			break;
			// Sin-Wave: Left to Right - Pseudo-Ascending
			case "sinRightAsc": {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);			// y = a*sin(kx)
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position

				//If out-of-bound and reaching upper-half of og position (help ascending) => Reset and change direction
				if (transform.position.x > screenEdge && transform.position.y < enemyVar[4] + (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch = "sinLeftAsc";
				}
			}
			break;
			// Sin-Wave: Right to Left - Pseudo-Ascending
			case "sinLeftAsc": {
				enemyVar[1] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];

				if (transform.position.x < -screenEdge && transform.position.y < enemyVar[4] + (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch = "sinRightAsc";
				}
			}
			break;
			// Sin-Wave: Downward
			case "sinDown": {
				enemyVar[2] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[1] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[2]);			// x = sin(ky)
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position
			}
			break;

			/* 
			CURVE
			Formula: y = k(x/a)^b
			mod 1: Movement speed
			mod 2: Vertical direction (k) (-1 or 1 => Down or Up)
			mod 3: Curve point (a)
			mod 4: Curve power (b) (even number)
			Default: 1, -1, 2, 2
			*/
			// Curve: Left to Right
			case "curveRight": {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[2] * Mathf.Pow(enemyVar[1]/enemyMod[3], enemyMod[4]);		//y = k(x/a)^b
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position
			}
			break;
			// Curve: Right to Left
			case "curveLeft": {
				enemyVar[1] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[2] * Mathf.Pow(enemyVar[1]/enemyMod[3], enemyMod[4]);		//y = k(x/a)^b
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position
			}
			break;

		}

		// Die/Suicide conditions
		if (userInfo.hp <= 0 || Mathf.Abs(transform.position.x) > 6 || Mathf.Abs(transform.position.y) > 7) {
			EnemyDie();
		}
	}

	public void EnemyDie () {
		enemyCount--;
		
		userInfo.hp = 0;
		Destroy(gameObject);
	}

	private IEnumerator startFirePattern (int slotID, int gap, int randomGap, int continuous, int continuousGap, int randomContinuous) {
		/* 
		gap (ms): Time gap between segments
		randomGap (ms): Random period for Time gap (Gap + (0 <->randomGap))
		continuous (ms): Numbers of Fire shot in a segment
		continuousGap (ms): Gap between Fire shots in a segment
		randomContinuous (ms): Random period for continuousGap (continuousGap + (0 <-> randomContinoous))
		*/
		yield return new WaitForSeconds ( 1f*(gap + rnd.Next(0, randomGap) )/1000f );			//TEMPORARY

		WeaponDB weaponDB = GameObject.Find("GameMaster").GetComponent<WeaponDB>();
		while (userInfo.hp > 0) {
			for (int i=0; i < continuous; i++) {
				weaponDB.useCard (ref userInfo, userInfo.currentCard, gameObject);
				yield return new WaitForSeconds ( 1f*(continuousGap + rnd.Next(0, randomContinuous) ) / 1000f );
			}
			yield return new WaitForSeconds ( 1f*(gap + rnd.Next(0, randomGap) )/1000f );
		}
	}

	private int boundaryCheck (float x, float y) {
		if (x < -3.85f)
			return 0;
		if (x > 3.85f)
			return 0;
		if (y < -3.8f)
			return 0;
		if (y > 5f)
			return 0;
		return 1;
	}
}
