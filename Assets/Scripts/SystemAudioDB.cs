using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAudioDB : MonoBehaviour {

	public AudioClip overheatAlert;
	public AudioClip overheatExhaust;
	public AudioClip overheatCooling;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playSFX (AudioSource sfxSource, string sfxName) {
		switch (sfxName) {
			case "overheat1": {
				sfxSource.loop = true;
				sfxSource.clip = overheatAlert;
				sfxSource.Play();
				sfxSource.PlayOneShot (overheatExhaust);
			}
			break;
			case "overheat2": {
				sfxSource.Stop();
				sfxSource.PlayOneShot (overheatCooling);
			}
			break;
		}
	}
}
