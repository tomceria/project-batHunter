using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static double gameTime = 0;
	public int stageID = 1;
	private double gameTimeMark = 0;
	private int spawnQueue = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Updating gameTime every frame
		gameTime += Time.deltaTime;
		
		if (gameTime >= gameTimeMark) {
			spawnQueue++;								//spawn queue increment; increase this whenever a batch start
			//Make use of multiple batchMod for simultaneous spawning
			float[] batchMod = new float[10];
			float[] batchMod2 = new float[10];
			//float[] batchMod3 = new float[10];
			switch (stageID) {
				case 0: {			// SINGLE BATCH TEST
					switch (spawnQueue) {
						case 1: {
							gameTimeMark = 0;
							break;
						}
					}
					break;
				}
				case 1: {			// TEST LEVEL
					switch (spawnQueue) {
						case 1: {
							gameTimeMark = 2; 
							break;		//First spawn delay, actual spawn starts after this mark
						}		
						case 2: {
							batchMod[1]=3;	batchMod[2]=0.25f;
							spawnBatchCmd (11, 0, 5, -4.5f, 3.5f, 0.25f, 0, batchMod);
							gameTimeMark = 6;
							break;
						}
						case 3: {
							batchMod[1]=1;		batchMod[2]=5f;		batchMod[3]=1f;
							spawnBatchCmd (25, 0, 10, 1f, 5.5f, 0.5f, 0, batchMod);
							batchMod2[1]=1;		batchMod2[2]=5f;	batchMod2[3]=-1f;
							spawnBatchCmd (25, 0, 10, 1f, 5.5f, 0.5f, 0, batchMod2);
							//
							gameTimeMark = 15;
							break;
						}
                        case 4: {
							batchMod[1]=1.5f;		batchMod[2]=1;		batchMod[3]=4;		batchMod[4]=4;
							spawnBatchCmd (31, 1, 20, -4.5f, -3.5f, 0.3f, 0, batchMod);
							gameTimeMark = 100;
							break;
						}
					}
					break;
				}
			}
			
		}
	}

	void spawnBatchCmd (int batchID, int enemyID, int enemyQuantity, float posX, float posY, float spawnTimeGap, float spawnDelay, float[] mod) {
		StartCoroutine (EnemySpawner.spawnEnemyBatch (batchID, enemyID, enemyQuantity, new Vector2(posX, posY), spawnTimeGap, spawnDelay, mod));
	}
}
