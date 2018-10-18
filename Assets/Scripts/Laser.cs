using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	public GameObject laserStart;
	public GameObject laserMid;
	public GameObject laserEnd;

	private GameObject start;
	private GameObject middle;
	private GameObject end;

	public Color laserA;

	public int laserWidth;			// Receive variable from WeaponDB
	public float damage;            // Receive variable from WeaponDB
	public float userType;          // Receive variable from WeaponDB

	// Use this for initialization
	void Start () {
		//transform.position = new Vector2 (Player.playerPositionX, Player.playerPositionY);

		// SPAWN LASER BUTT AND LASER BODY
		if (start == null) { //only spawn once
			start = Instantiate (laserStart, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
			//TEMP
			start.transform.localScale = new Vector3 (start.transform.localScale.x * laserWidth, start.transform.localScale.y, start.transform.localScale.z);
		}
		if (middle == null) {
			middle = Instantiate (laserMid, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
			//TEMP
			middle.transform.localScale = new Vector3 (middle.transform.localScale.x * laserWidth, middle.transform.localScale.y, middle.transform.localScale.z);
		}


		// SET LASER SIZE
		float LaserSize;					// Actual laser size (sprite size)
		float MaxLaserSize;					// Default laser size if no target hit
		float raycastSize;					// Raycast size
		float highestRaycastSize;
		MaxLaserSize = 20f;
		raycastSize = MaxLaserSize;
		LaserSize = MaxLaserSize;
		highestRaycastSize = 0f;
		Enemy[] dObj = new Enemy[20];			// GameObject array containing different hit objects
		int dObjSize = 0;

		//RAYCAST
		Vector2 laserDirection = this.transform.up;


		// CREATE ARRAY OF RAYCAST, CONTAINING OBJECTS HIT BY RAYCAST
		RaycastHit2D[] hit = new RaycastHit2D[100];
		for (int i = -5*laserWidth; i <= 5*laserWidth; i++) {
			int id = i - (-5*laserWidth);
			Vector2 beamStartPos = new Vector2 (transform.position.x + 0.02f*i, transform.position.y);

			hit[id] = Physics2D.Raycast (beamStartPos, laserDirection, MaxLaserSize, LayerMask.GetMask("Users"));		//8: layer Users (only Player and Enemy)
			//TODO: apply on Player
			if (hit[id].collider != null && hit[id].collider.tag == "Enemy") {			// Check if raycast actually hit any obj (collider)
				raycastSize = Vector2.Distance (this.transform.position, hit[id].point);		// Set raycastSize based on distance from shooter to raycast hit surface
				// Picking actual LaserSize
				if (raycastSize > highestRaycastSize) {
					highestRaycastSize = raycastSize;
				}
				if (dObjSize == 0) {					// Add new object into the array immediately if array is empty
					dObj[dObjSize++] = hit[id].collider.GetComponent<Enemy>();
				}
				else {
					for (int j=0; j < dObjSize; j++) {
						//Debug.Log (hit[id].collider.transform.position);
						if (hit[id].collider.GetComponent<Enemy>() == dObj[j]) {			//Check if hit[id] GameObject is different from dObj[j] GameObject
							break;
						}
						if (j == dObjSize-1 &&  hit[id].collider.GetComponent<Enemy>() != dObj[j]) {			//Last in array, if still different, add obj into the array	
							dObj[dObjSize++] = hit[id].collider.GetComponent<Enemy>();
							break;					// as dObjSize++, avoid continuing
						}
					}
				}

				
			}
			Debug.DrawRay (beamStartPos, new Vector2 (transform.position.x, raycastSize), Color.black, 2f);		//debug
		}

		// DEAL DAMAGE
		dealDmg (ref dObj, dObjSize);
		// SET LASER SIZE BASED ON HIGHEST RAYCAST SIZE
		if (dObjSize > 0) {
			LaserSize = highestRaycastSize;
		}

		//LASER IMPACT EFFECT
		if (end == null) {
			end = Instantiate (laserEnd, new Vector2 (transform.position.x, transform.position.y + LaserSize), Quaternion.identity) as GameObject;
			//TEMP
			end.transform.localScale = new Vector3 (end.transform.localScale.x * laserWidth, end.transform.localScale.y, end.transform.localScale.z);
		}

		//DETERMINE LASER SIZE BASED ON RAYCAST
		middle.transform.localScale = new Vector3 (middle.transform.localScale.x, LaserSize, middle.transform.localScale.z);

	}
	
	// Update is called once per frame
	void Update () {
		

		//FADEOUT
		start.GetComponent<Renderer> ().material.color = laserA;
		middle.GetComponent<Renderer> ().material.color = laserA;
		end.GetComponent<Renderer> ().material.color = laserA;
		laserA.a -= 0.05f;
		if (laserA.a <= 0) {
			Destroy (start); Destroy (middle); Destroy (end);
			Destroy (gameObject);
		}


	}

	void dealDmg (ref Enemy[] dObj, int dObjSize) {
		for (int i=0; i<dObjSize; i++) {
            Enemy enemyComponent = dObj[i].GetComponent<Enemy>();
            enemyComponent.userInfo.hp -= damage;
            Debug.Log("POW!LASER " + enemyComponent.userInfo.hp);
            //Destroy(gameObject);
		}
		
	}
}
