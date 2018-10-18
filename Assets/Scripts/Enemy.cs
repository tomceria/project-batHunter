using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Game.info userInfo;				//Using info struct from Game.cs

	public float screenEdge = 4.5f;
	public int enemyID;
	public int enemyBatch;
	private System.Random rnd = new System.Random();
	public float[] enemyMod = new float[10];		//Value determined by EnemySpawner to change batch's behaviour
	public float[] enemyVar = new float[10];		//Value changing throughout the process for batch's functions

	// Use this for initialization
	void Start () {

		// Enemy properties
		switch (enemyID) {
			case 0: {				// Test Enemy
				userInfo.inventory = new WeaponDB.weapon[20];				//Initiating Enemy's card inventory
				WeaponDB.getCard(101, ref userInfo, 1, 1);
				userInfo.type = 2;
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, -0.2f, 0);
				StartCoroutine (startFirePattern (1, 1000, 7000, 1, 0, 0));
				break;
			}
			case 1: {				// Test Enemy 2
				userInfo.inventory = new WeaponDB.weapon[20];
				WeaponDB.getCard(102, ref userInfo, 1, 8);
				userInfo.type = 2;
				userInfo.hp = 5f;
				userInfo.barrelPos = new Vector3 (0, 0, 0);
				gameObject.GetComponent<SpriteRenderer>().color = Color.red;
				StartCoroutine (startFirePattern (1, 1000, 10000, 3, 100, 0));
				break;
			}
		}
		gameObject.tag = "Enemy";
		userInfo.currentCard = 1;				//TEMP
		userInfo.heatMax = 100;
		userInfo.heat = userInfo.heatMax;
		//

		// Enemy movement variable (Startup)
		switch (enemyBatch) {
			// Zig-Zag Approaching
			case 2: {
				enemyVar[1] = transform.position.x;
				enemyVar[2] = transform.position.y;
				enemyVar[3] = 1000;					// Increasing time, instantly move after spawning
				enemyVar[5] = 0;					//5; 6: Random value, enemy move toward near player
				enemyVar[6] = 0;
				break;
			}
			// Sin-Wave & Curve:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 31: {		//Apply for y=sin(x), y=(x/a)^b
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

		//Enemy movement
		switch (enemyBatch) {

			/*
			AUTO PATHING
			mod 1: Movement speed
			mod 2: Refresh rate (second)
			mod 3: Player approaching radius
			mod 4: Player approaching radius' decrement
			*/
			// Straight-up Approaching Player
			case 1: {
				Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
				transform.position = Vector2.MoveTowards (gameObject.transform.position, playerComponent.transform.position, enemyMod[1] * Time.deltaTime);
				
			}
			break;
			// Approaching Zig-Zag Style (Random near player)
			// (The goal is to make enemy move towards the outline of player's radius, radius reduce every step)
			case 2: {
				Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
				enemyVar[3] += 1f * Time.deltaTime;				// Increase by time, reset when reaching refresh rate time
				Debug.Log (enemyVar[3]);
				if (enemyVar[3] >= enemyMod[2]) {
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

			/*
			STRAIGHT LINE
			mod 1: Vertical movement speed			//Vertical movement speed = 0 => straight horizontal movement
			mod 2: Descending speed					//Descending speed = 0 => straight vertical movement
			Default: 1, 0.1
			*/
			// Straight Line: Left to Right
			case 11: {						
				transform.position = new Vector2 (transform.position.x + (enemyMod[1] * Time.deltaTime), transform.position.y - (enemyMod[2] * Time.deltaTime));
				if (transform.position.x > screenEdge)
					enemyBatch++;			// Switch direction
			}
			break;
			// Straight Line: Right to Left
			case 12: {
				transform.position = new Vector2 (transform.position.x - (enemyMod[1] * Time.deltaTime), transform.position.y - (enemyMod[2] * Time.deltaTime));
				if (transform.position.x < -screenEdge)
					enemyBatch--;			// Switch direction
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
			case 21: {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);			// y = a*sin(kx)
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position

				//If out-of-bound and reaching lower-half of og position (help descending) => Reset and change direction
				if (transform.position.x > screenEdge && transform.position.y < enemyVar[4] - (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch++;
				}
			}
			break;
			// Sin-Wave: Right to Left - Pseudo-Descending
			case 22: {
				enemyVar[1] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];

				if (transform.position.x < -screenEdge && transform.position.y < enemyVar[4] - (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch--;
				}
			}
			break;
			// Sin-Wave: Left to Right - Pseudo-Ascending
			case 23: {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);			// y = a*sin(kx)
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position

				//If out-of-bound and reaching upper-half of og position (help ascending) => Reset and change direction
				if (transform.position.x > screenEdge && transform.position.y < enemyVar[4] + (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch++;
				}
			}
			break;
			// Sin-Wave: Right to Left - Pseudo-Ascending
			case 24: {
				enemyVar[1] -= (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[3] * Mathf.Sin(enemyMod[2] * enemyVar[1]);
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];

				if (transform.position.x < -screenEdge && transform.position.y < enemyVar[4] + (Mathf.Abs(enemyMod[3])/2)) {
					enemyVar[1] = 0;	enemyVar[2] = 0;
					enemyVar[3] = transform.position.x;	enemyVar[4] = transform.position.y;
					enemyBatch--;
				}
			}
			break;
			// Sin-Wave: Downward
			case 25: {
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
			mod 4: Curve power (b) (odd number)
			Default: 1, -1, 2, 2
			*/
			// Curve: Left to Right
			case 31: {
				enemyVar[1] += (enemyMod[1] * Time.deltaTime);
				enemyVar[2] = enemyMod[2] * Mathf.Pow(enemyVar[1]/enemyMod[3], enemyMod[4]);		//y = k(x/a)^b
				enemyVar[3] += enemyVar[1];		enemyVar[4] += enemyVar[2];
				transform.position = new Vector2 (enemyVar[3], enemyVar[4]);
				enemyVar[3] -= enemyVar[1];		enemyVar[4] -= enemyVar[2];				//Reset to spawn position
			}
			break;
			// Curve: Right to Left
			case 32: {
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
}
