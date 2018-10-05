using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModuleHUD : MonoBehaviour {

    private static Image selfbar;

    // Use this for initialization
    void Start () {
        selfbar = gameObject.GetComponent<Image>();
        
	}
	
	// Update is called once per frame
	void Update () {
		Player player = GameObject.Find("Player").GetComponent<Player>();				//Get Player.userInfo
		selfbar.fillAmount = 1 - (player.userInfo.heat / player.userInfo.heatMax);
	
	}
}
