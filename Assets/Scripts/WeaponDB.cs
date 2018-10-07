using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDB : MonoBehaviour {

	public GameObject projectile;
	public GameObject laser;

	public struct weapon {
		public int id;
		public int type;
		public int level;
		public float power;
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

	public static void getCard (int id, ref Game.info userInfo, int slotID, int cardLevel) {		//Temporary. TODO: Check for empty slot to add card in
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
			PROJECTILE: Card Projectiles number / size
			DELAYMAX: Card delay, decreases every frame, unusuable until reaches 0. Reset when use card
			HEATDES: Heat cost
			HEATACCELBASE: Heat acceleration, increases by multiplicative increment
		*/
		int baseType=0, baseProjectileFROM=0, baseProjectileTO=0;
		float basePowerFROM=0, basePowerTO=0, baseDelayMaxFROM=0, baseDelayMaxTO=0, baseHeatDesFROM=0, baseHeatDesTO=0, baseHeatAccelBaseFROM=0, baseHeatAccelBaseTO=0;
		switch (id) {
			case 1:				// Stick-projectile shoot in cone shape
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 20;					baseDelayMaxTO = 10;
				baseHeatDesFROM = 7;					baseHeatDesTO = 5;
				baseHeatAccelBaseFROM = 0.1f;			baseHeatAccelBaseTO = 0.08f;
				break;
			case 2:				// Laser beam
				baseType = 0;
				basePowerFROM = 8;						basePowerTO = 20;
				baseProjectileFROM = 1;					baseProjectileTO = 3;
				baseDelayMaxFROM = 30;					baseDelayMaxTO = 20;
				baseHeatDesFROM = 20;					baseHeatDesTO = 16;
				baseHeatAccelBaseFROM = 0.3f;			baseHeatAccelBaseTO = 0.28f;
				break;
			case 3:				// Ball-projectile shoot in 180/x direction (surrounding)
				baseType = 0;
				basePowerFROM = 3;						basePowerTO = 10;
				baseProjectileFROM = 1;					baseProjectileTO = 5;
				baseDelayMaxFROM = 15;					baseDelayMaxTO = 13;
				baseHeatDesFROM = 10;					baseHeatDesTO = 6;
				baseHeatAccelBaseFROM = 0.25f;			baseHeatAccelBaseTO = 0.1f;
				break;
		}
		userInfo.inventory[slotID].type = baseType;
		userInfo.inventory[slotID].projectile = ((baseProjectileTO-baseProjectileFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+baseProjectileFROM;
		userInfo.inventory[slotID].power = ((basePowerTO-basePowerFROM)*(userInfo.inventory[slotID].level-1)/(10-1))+basePowerFROM;
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

		// Card behaviour (TODO: Card properties (sprite, etc.) separately)
		switch (userInfo.inventory[slotID].id) {
			case 1: {				// Stick-projectile shoot in cone shape
				int bulletAngel = 5;
				for (var i = 0; i <= (userInfo.inventory[slotID].projectile-1)*2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z - (userInfo.inventory[slotID].projectile - 1)*bulletAngel + bulletAngel*i + facedown*180 + unconcentrate*rnd.Next(-5, 5));
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.userType = userInfo.type;				// Transfer user type into Projectile.cs
				}
				break;
			}
			case 2: {				// Laser beam
				GameObject Laser = Instantiate(laser, new Vector2(user.transform.position.x, user.transform.position.y + 0.8f), Quaternion.identity) as GameObject;
				Laser.transform.rotation = Quaternion.Euler(0, 0, 0 + facedown*180);
				//TODO: Laser size based on w.projectile
				break;
			}
			case 3: {				// Ball-projectile shoot in 180/x direction (surrounding)
				int bulletAngel = 180/(userInfo.inventory[slotID].projectile);
				for (var i = 0; i < userInfo.inventory[slotID].projectile * 2; i++) {
					GameObject Bullet = Instantiate(projectile, user.transform.position + userInfo.barrelPos + zDepth, Quaternion.identity) as GameObject;
					Bullet.transform.rotation = Quaternion.Euler(0, 0, user.transform.rotation.z + bulletAngel*i);
					Projectile bulletComponent = Bullet.GetComponent<Projectile>();			// Get Projectile.cs component
					bulletComponent.damage = userInfo.inventory[slotID].power;				// Transfer damage/power data into Projectile.cs
					bulletComponent.userType = userInfo.type;				// Transfer user type into Projectile.cs
				}
				break;
			}
		}
	}
}
