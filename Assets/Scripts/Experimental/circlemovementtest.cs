using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlemovementtest : MonoBehaviour {
	Vector2 rigidOrigin;

	public GameObject rigidTest;
	static int circleTrigger;

	private float CircleX;
	private float CircleY;
	public float PathX;
	public float PathY;
	private float interPola = 0f;
	private float minPola;
	private float maxPola;
	private float tempPola;
	private int positive;
	// Use this for initialization
	void Start () {
		rigidOrigin = transform.position;
		circleTrigger = 0;
		minPola = -Mathf.Sqrt (2F);
		maxPola = Mathf.Sqrt (2F);
		positive = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//CircleX = transform.position.x;
		//CircleY = transform.position.y;
		//PathX = CircleX - Mathf.Sqrt (Mathf.Pow (CircleX, 2F) + Mathf.Pow (CircleY, 2F) + 2);
		//PathY = CircleY - Mathf.Sqrt (Mathf.Abs(2 * CircleX * PathX + Mathf.Pow (CircleY, 2F) - Mathf.Pow (PathX, 2F) + 2));


		if (circleTrigger == 0) {
			gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (2f, 0);
		
		}
		else if (circleTrigger == 1) {
			gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			transform.position = new Vector2 (PathX, PathY);

			interPola += 2f;
			//PathX += 0.05f;
			PathX = Mathf.Lerp (minPola, maxPola, Time.deltaTime * interPola);
			PathY = positive * Mathf.Sqrt (2 - Mathf.Pow (PathX, 2F));



			if (interPola > 50) {
				tempPola = maxPola;
				maxPola = minPola;
				minPola = tempPola;
				switch (positive) {
				case -1:
					positive = 1;
					break;
				case 1:
					positive = -1;
					break;
				}
				interPola = 0;
			}
			Debug.Log (interPola);
		}

		float daDistance = Vector2.Distance (rigidOrigin, transform.position);
		if (daDistance >= 2) {
			circleTrigger = 1;
		} 



	}
}
