using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDB : MonoBehaviour {

	private GameObject projectile;
	public GameObject laser;
	public GameObject longBullet;
	public GameObject smallBall;

	public struct weapon {
		public string id;
		public int type;
		public int level;
		public float power;
		public float speed;
		public int projectile;
		public float delay;
		public float delayMax;
		public float heatDes;
		public float heatAccel;
		public float heatAccelBase;
	}
	//public WeaponDB.weapon thegod;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void getCard (string id, ref Game.info userInfo, int slotID, int cardLevel) {		//Temporary. TODO: Check for empty slot to add card in
		userInfo.inventory[slotID].id = id;										//Assign "int id" to item in slot
		userInfo.inventory[slotID].level += cardLevel;							//Assign number of card / increment level of card

		/* 
			VALUE LERP FORMULA:
			Assume value varies from 1 -> 10 (x; with x1=1, x2=10)
			y1: Starting value
			y2: Ending value
			
			FORMULA: y = ((y2 - y1)*(x-1)/(10-1))+y1
		*/

		/* 
			Weapon/Card variables:
			TYPE: Card type: 0: Weapon Module; 1: Spell Card; 2: Consumables; 3: Special
			POWER: Card Damage / Buff power / Debuff power
			SPEED: Projectile travel speed / Spell Channel time / etc
			PROJECTILE: Card Projectiles number / size
			DELAYMAX: Card delay, decreases every frame, unusuable until reaches 0. Reset when use card
			HEATDES: Heat cost
			HEATACCELBASE: Heat acceleration, increases by multiplicative increment
		*/
		int baseType=0, baseProjectileFROM=0, baseProjectileTO=0;
		float basePowerFROM=0, basePowerTO=0, baseSpeedFROM=0, baseSpeedTO=0, baseDelayMaxFROM=0, baseDelayMaxTO=0, baseHeatDesFROM=0, baseHeatDesTO=0, baseHeatAccelBaseFROM=0, baseHeatAccelBaseTO=0;
		switch (id) {
			case "cone-stick":				// HOLY
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseSpeedFROM = 50;						baseSpeedTO = 50;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 20;					baseDelayMaxTO = 10;
				baseHeatDesFROM = 7;					baseHeatDesTO = 5;
				baseHeatAccelBaseFROM = 0.1f;			baseHeatAccelBaseTO = 0.08f;
				break;
			case "laser-once":				// Laser beam
				baseType = 0;
				basePowerFROM = 8;						basePowerTO = 20;
				baseSpeedFROM = 0;						baseSpeedTO = 0;
				baseProjectileFROM = 1;					baseProjectileTO = 3;
				baseDelayMaxFROM = 30;					baseDelayMaxTO = 20;
				baseHeatDesFROM = 14;					baseHeatDesTO = 8;
				baseHeatAccelBaseFROM = 0.05f;			baseHeatAccelBaseTO = 0.01f;
				break;
			
			//Enemy weapons
			case "enemy-cone-stick":				// Stick-projectile shoot in cone shape (TEMPORARY)
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseSpeedFROM = 20;						baseSpeedTO = 20;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 20;					baseDelayMaxTO = 10;
				baseHeatDesFROM = 7;					baseHeatDesTO = 5;
				baseHeatAccelBaseFROM = 0.1f;			baseHeatAccelBaseTO = 0.08f;
				break;
			case "enemy-surround-ball":				// Ball-projectile shoot in 180/x direction (surrounding)
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseSpeedFROM = 10;						baseSpeedTO = 10;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 15;					baseDelayMaxTO = 13;
				baseHeatDesFROM = 10;					baseHeatDesTO = 6;
				baseHeatAccelBaseFROM = 0.25f;			baseHeatAccelBaseTO = 0.1f;
				break;
			case "enemy-cone-ball-targetPlayer":				// Ball-projectile shoot in cone shape - TARGET PLAYER (PLACEHOLDER)
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseSpeedFROM = 5;						baseSpeedTO = 5;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 20;					baseDelayMaxTO = 10;
				baseHeatDesFROM = 7;					baseHeatDesTO = 5;
				baseHeatAccelBaseFROM = 0.1f;			baseHeatAccelBaseTO = 0.08f;
				break;
			case "enemy-burst-ball-targetPlayer":				// Ball-projectile shoot in burst - TARGET PLAYER (PLACEHOLDER)
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseSpeedFROM = 5;						baseSpeedTO = 5;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 20;					baseDelayMaxTO = 10;
				baseHeatDesFROM = 7;					baseHeatDesTO = 5;
				baseHeatAccelBaseFROM = 0.1f;			baseHeatAccelBaseTO = 0.08f;
				break;
		}
		userInfo.inventory[slotID].type = baseType;
		userInfo.inventory[slotID].projectile = ((baseProjectileTO-baseProjectileFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseProjectileFROM;
		userInfo.inventory[slotID].power = ((basePowerTO-basePowerFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+basePowerFROM;
		userInfo.inventory[slotID].speed = ((baseSpeedTO-baseSpeedFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseSpeedFROM;
		userInfo.inventory[slotID].delayMax = ((baseDelayMaxTO-baseDelayMaxFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseDelayMaxFROM;
		userInfo.inventory[slotID].delay = userInfo.inventory[slotID].delayMax;			// Non-static
		userInfo.inventory[slotID].heatDes = ((baseHeatDesTO-baseHeatDesFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseHeatDesFROM;
		userInfo.inventory[slotID].heatAccelBase = ((baseHeatAccelBaseTO-baseHeatAccelBaseFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseHeatAccelBaseFROM;
		userInfo.inventory[slotID].heatAccel = 1;										// Non-static
		
	}

	public void useCard (ref Game.info userInfo, int slotID, GameObject user) {
		userInfo.heat -= userInfo.inventory[slotID].heatDes * userInfo.inventory[slotID].heatAccel;			//Decrease Heat capacity when shot
		userInfo.inventory[slotID].heatAccel += userInfo.inventory[slotID].heatAccelBase;					//Increase Heat Accel when shot
		userInfo.inventory[slotID].delay = userInfo.inventory[slotID].delayMax;								//Reset delay to Max when shot

		AudioSource audiosource = user.GetComponent<AudioSource>();											//Get AudioSource component from user
		UserAudioDB useraudiodb = GameObject.Find("GameMaster").GetComponent<UserAudioDB>();				//non-static void => get component required
		useraudiodb.playSFX(audiosource, userInfo.inventory[slotID].id);									//Play sfx with determined Card ID

		int facedown = (userInfo.type == 2)?1:0;			// Face downward if user is an enemy
		int unconcentrate = 0;								// Unconcentrating: Shots won't be accurate when stamina deplete
		System.Random rnd = new System.Random();			// Randomizing angel for Unconcentrating Shots
		Vector3 zDepth = new Vector3 (0, 0, -1);			// In 3D space, projectile stay on top of player and enemies in order to reflect light

		if (userInfo.heat < userInfo.inventory[slotID].heatDes + 1) {
			unconcentrate = 1;
		}

		//PREFAB SELECT
		switch (userInfo.inventory[slotID].id) {
			case "cone-stick":
			case "enemy-cone-stick": {
				projectile = longBullet;
			}
			break;
			case "enemy-surround-ball":
			case "enemy-cone-ball-targetPlayer":
			case "enemy-burst-ball-targetPlayer": {
				projectile = smallBall;
			}
			break;
			case "laser-once": {
				projectile = laser;				//TEMPORARY
			}
			break;
		}
		// CARD BEHAVIOUR (TODO: Card properties (sprite, etc.) separately)
		switch (userInfo.inventory[slotID].id) {
			case "cone-stick":
			case "enemy-cone-stick": {				// Stick-projectile shoot in cone shape
				int bulletAngel = 5;
				for (var i = 0; i <= (userInfo.inventory[slotID].projectile-1)*2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z - (userInfo.inventory[slotID].projectile - 1)*bulletAngel + bulletAngel*i + facedown*180 + unconcentrate*rnd.Next(-5, 5));
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					//VARIABLE TRANSFER
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.travelSpeed = userInfo.inventory[slotID].speed;			// Transfer speed into Projectile.cs
					bulletComponent.userType = userInfo.type;								// Transfer user type into Projectile.cs
				}
				break;
			}
			case "laser-once": {				// Laser beam
				GameObject Beam = Instantiate(projectile, new Vector2(user.transform.position.x, user.transform.position.y + userInfo.barrelPos.y), Quaternion.identity) as GameObject;
				Beam.transform.rotation = Quaternion.Euler(0, 0, 0 + facedown*180);
				//TODO: Laser size based on w.projectile
				Laser laserComponent = Beam.GetComponent<Laser>();
				laserComponent.laserWidth = userInfo.inventory[slotID].projectile;
				laserComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
				laserComponent.userType = userInfo.type;								// Transfer user type into Projectile.cs
				break;
			}
			case "enemy-surround-ball": {				// Ball-projectile shoot in 180/x direction (surrounding)
				int bulletAngel = 180/(userInfo.inventory[slotID].projectile);
				for (var i = 0; i < userInfo.inventory[slotID].projectile * 2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z + bulletAngel*i);
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.travelSpeed = userInfo.inventory[slotID].speed;			// Transfer speed into Projectile.cs
					bulletComponent.userType = userInfo.type;								// Transfer user type into Projectile.cs
				}
				break;
			}

			case "enemy-cone-ball-targetPlayer": {				// Ball-projectile shoot in cone shape - TARGET PLAYER (PLACEHOLDER)
				int bulletAngel = 5;
				for (var i = 0; i <= (userInfo.inventory[slotID].projectile-1)*2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
					float targetPlayerAngle = Vector2.SignedAngle(transform.up, (playerComponent.transform.position - Bullet.transform.position).normalized);
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z + targetPlayerAngle - (userInfo.inventory[slotID].projectile - 1)*bulletAngel + bulletAngel*i);
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					//VARIABLE TRANSFER
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.travelSpeed = userInfo.inventory[slotID].speed;			// Transfer speed into Projectile.cs
					bulletComponent.userType = userInfo.type;								// Transfer user type into Projectile.cs
				}
				break;
			}

			case "enemy-burst-ball-targetPlayer": {				// Ball-projectile shoot in burst - TARGET PLAYER (PLACEHOLDER)
				int bulletAngel = 5;
				for (var i = 0; i <= (userInfo.inventory[slotID].projectile-1)*2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Player playerComponent = GameObject.Find("Player").GetComponent<Player>();
					float targetPlayerAngle = Vector2.SignedAngle(transform.up, (playerComponent.transform.position - Bullet.transform.position).normalized);
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z + targetPlayerAngle - (userInfo.inventory[slotID].projectile - 1)*bulletAngel + bulletAngel*i + rnd.Next(-5, 5));
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					//VARIABLE TRANSFER
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.travelSpeed = userInfo.inventory[slotID].speed;			// Transfer speed into Projectile.cs
					bulletComponent.userType = userInfo.type;								// Transfer user type into Projectile.cs
				}
				break;
			}
		}
	}

}
