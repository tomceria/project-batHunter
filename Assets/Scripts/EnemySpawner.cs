using System;			//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject EnemyObj;

	// Use this for initialization
	void Start () {
		/*
		float[] batchMod = new float[10];
		batchMod[1]=1;
		batchMod[2]=5f;
		batchMod[3]=1f;
		StartCoroutine (spawnEnemyBatch (25, 0, 10, new Vector2(-3f, 5.5f), 0.5f, batchMod));
		*/
	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void spawnEnemy (int enemyID, Vector2 ogPos, int batchID, float[] mod) {
		EnemySpawner enemyspawner = GameObject.Find("GameMaster").GetComponent<EnemySpawner>();
		GameObject enemy = Instantiate (enemyspawner.EnemyObj, ogPos, Quaternion.identity);
		Enemy EnemyComponent = enemy.GetComponent<Enemy>();
		EnemyComponent.enemyBatch = batchID;			// Send "batchID" over to "enemy" object => Move patterns
		EnemyComponent.enemyMod = mod;					// Send "mod" (float array) over to "enemy" object => Enemy Batch initial modifications
	}

	public static IEnumerator spawnEnemyBatch (int batchID, int enemyID, int enemyQuantity, Vector2 ogPos, float spawnTimeGap, float spawnDelay, float[] mod) {
		//TODO: Spawn portal, closes when loop is finished
		yield return new WaitForSeconds (spawnDelay);
		//switch (batchID) {
			//case 1: {
				for (int i=0; i<enemyQuantity; i++) {
					spawnEnemy (enemyID, ogPos, batchID, mod);
					yield return new WaitForSeconds (spawnTimeGap);
				}
			//}
			//break;
		//}
	}


}