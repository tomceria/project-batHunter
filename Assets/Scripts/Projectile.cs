using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Rigidbody2D projectileRigid;
    public Camera theCamera;
    private float accelerate;
    public float damage;            // Receive variable from WeaponDB
    public float userType;          // Receive variable from WeaponDB
    //private static int ProjectileweaponLevel;


    // Use this for initialization
    void Start () {
        accelerate = 10f;
        //WEAPON INITIAL PROPERTIES
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 50f * accelerate);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        
        //accelerate += 0.25f;
    }
    
    void Update () {
        if (Mathf.Abs(transform.position.x) > 6f || Mathf.Abs(transform.position.y) > 6f)       // Destroy when out of view (TEMPORARY, TODO: varies depend on camera position)
            Destroy(gameObject);

    }

    void OnTriggerEnter2D (Collider2D hitUser) {            // Start when projectile hit a user
        if (userType == 0 && hitUser.gameObject.tag == "Enemy") {           //Player shoots enemy
            Enemy enemyComponent = hitUser.GetComponent<Enemy>();
            enemyComponent.userInfo.hp -= damage;
            Debug.Log("POW! " + enemyComponent.userInfo.hp);
            Destroy(gameObject);
        }
        else if (userType == 2 && hitUser.gameObject.tag == "Player") {     //Enemy shoots player
            Player playerComponent = hitUser.GetComponent<Player>();
            playerComponent.userInfo.hp -= damage;
            Debug.Log("OUCH! " + playerComponent.userInfo.hp);
            Destroy(gameObject);
        }
    }

}
