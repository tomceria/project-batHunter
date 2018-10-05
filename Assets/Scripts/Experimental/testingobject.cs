using UnityEngine;
using System.Collections;

public class testingobject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 50f * Time.deltaTime);


    }
}
