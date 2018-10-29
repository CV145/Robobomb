using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {

    public GameObject Switch;
    //choose which switch connects with this cell in inspector
    Animator anim;
    BoxCollider2D collider;
    Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Switch.GetComponent<SwitchTrigger>().GetHit())
        {
            anim.SetBool("Opened", true);
            Destroy(collider);
            Destroy(rigidbody);
        }
	}
}
