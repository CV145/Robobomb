using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour {
    //This script makes the UFO fall down and rotate when hit. Upon hitting the floor, "explode"

    //Get the animator and begin the script when "isHit" is true
     public Animator anim;
    public Patrol patrol;
    //Explosion game object
    public GameObject explosion;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //If isHit is true, turn on gravity and unlock rotation. When hitting the floor, destroy self and spawn explosion
		if (anim.GetBool("isHit"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 30;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            Destroy(GetComponent<Patrol>());
        }
	}

    //If grounded or colliding with an enemy - explode
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Against a wall
        if (collision.gameObject.layer == 10)
        {
            Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
            Destroy(this.gameObject);
        }
        //Against an enemy
        if (collision.gameObject.layer == 13)
        {
            Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
            Destroy(this.gameObject);
        }
    }
}
