using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    public LayerMask whatIsGround;
    Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 4), 0.4f, whatIsGround))
            {
            rigidbody.velocity = Vector2.up * 25;
        }
        }
    }
