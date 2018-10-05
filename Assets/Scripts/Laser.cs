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

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		//transform.position = new Vector2 (Player.playerPositionX, Player.playerPositionY);

		// SPAWN LASER BUTT AND LASER BODY
		if (start == null) { //only spawn once
			start = Instantiate (laserStart, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
		}
		if (middle == null) {
			middle = Instantiate (laserMid, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
		}


		// SET LASER SIZE
		float LaserSize;
		float MaxLaserSize;
		MaxLaserSize = 20f;
		LaserSize = MaxLaserSize;

		//RAYCAST
		Vector2 laserDirection = this.transform.up;
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, laserDirection, MaxLaserSize);

		//float LaserSizeWidth = start.GetComponent<Renderer>().bounds.size.y;

		//HIT ON COLLIDER
		if (hit.collider != null) {
			LaserSize = Vector2.Distance (this.transform.position, hit.point);
		}

		//LASER IMPACT EFFECT
		if (end == null) {
			end = Instantiate (laserEnd, new Vector2 (transform.position.x, transform.position.y + LaserSize), Quaternion.identity) as GameObject;
		}

		//start.transform.position = new Vector2 (transform.position.x, transform.position.y + 0.6f);
		//middle.transform.position = new Vector2 (transform.position.x, transform.position.y + 0.6f);
		//end.transform.position = new Vector2 (transform.position.x, transform.position.y + LaserSize);


		//DETERMINE LASER SIZE BASED ON RAYCAST
		middle.transform.localScale = new Vector3 (middle.transform.localScale.x, LaserSize, middle.transform.localScale.z);

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
}
