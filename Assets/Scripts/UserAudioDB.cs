using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAudioDB : MonoBehaviour {

	public AudioClip ioncannon;
	public AudioClip plasmabeam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playSFX (AudioSource sfxSource, string sfxID) {
		switch (sfxID) {
			case "cone-stick":
			case "enemy-cone-stick":
			case "enemy-surround-ball":
			case "enemy-cone-ball-targetPlayer":
			case "enemy-burst-ball-targetPlayer": {
				sfxSource.clip = ioncannon;
			}
			break;
			case "laser-once": {
				sfxSource.clip = plasmabeam;
			}
			break;
		}
		sfxSource.PlayOneShot(sfxSource.clip);
	}
}
