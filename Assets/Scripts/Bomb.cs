using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    Rigidbody2D Rigidbody;
    GameObject Robo;
    PlayerController playerScript;
    private Vector2 explosionPosition;

    // Use this for initialization
    void Start () {
        Rigidbody = GetComponent<Rigidbody2D>();
        Robo = GameObject.Find("RoboPlayer");
        playerScript = Robo.GetComponent<PlayerController>();

        if (playerScript.FacingRightGetter())
        {
            Rigidbody.velocity = new Vector2(70, Rigidbody.velocity.y); //100 too extreme. 50 too low?
            Rigidbody.AddForce(new Vector2(10, 35), ForceMode2D.Impulse); //maybe increase y vector
        }
        if (playerScript.FacingRightGetter() == false)
        {
            Rigidbody.velocity = new Vector2(-70, Rigidbody.velocity.y);
            Rigidbody.AddForce(new Vector2(-10, 35), ForceMode2D.Impulse);
        }
    }

    public Vector2 getExplosionPosition()
    {
        return explosionPosition;
    }
	
	// Update is called once per frame
	void Update () {
        explosionPosition = Rigidbody.transform.position;
	}
}
