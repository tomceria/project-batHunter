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

	public void playSFX (AudioSource sfxSource, int sfxID) {
		switch (sfxID) {
			case 1:
			case 101:
			case 102: {
				sfxSource.clip = ioncannon;
			}
			break;
			case 2: {
				sfxSource.clip = plasmabeam;
			}
			break;
		}
		sfxSource.PlayOneShot(sfxSource.clip);
	}
}
