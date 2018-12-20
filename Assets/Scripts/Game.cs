using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public struct info {			// Used by Player / Ally / Enemy
		public int type;						//0: player; 1: ally; 2: enemy
		public float hp;							//User's Health
		public float heat;						//Mana system
		public float heatMax;					//Max mana int
		public WeaponDB.weapon[] inventory;		//User's inventory
		public int currentCard;					//ID of current card in inventory
		public int overheatState;
		public Vector3 barrelPos;				//Weapon's barrel position
		public int weakShot;
	}

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
