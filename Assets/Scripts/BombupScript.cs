using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombupScript : MonoBehaviour {

    public LayerMask Player;
    BoxCollider2D collider;
    GameObject Robo;

	// Use this for initialization
	void Start ()
    {
        collider = GetComponent<BoxCollider2D>();
        Robo = GameObject.Find("RoboPlayer");
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (collider.IsTouchingLayers(Player))
        {
            //destroy self
            Robo.GetComponent<PickupsAndStats>().BombLVUp();
            Destroy(this.gameObject);
        }
	}
}
