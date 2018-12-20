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
		selfbar.fillAmount = (player.userInfo.heat / player.userInfo.heatMax);
		if (player.userInfo.weakShot == 0)
			selfbar.color = Color.green;
		else
			selfbar.color = Color.red;
	}
}
