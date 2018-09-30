using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float maxSpeed = 600f;
    bool facingRight = true;
    Rigidbody2D Rigidbody;
    Animator anim;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
     float move = Input.GetAxisRaw("Horizontal"); //1 or -1?
     Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y); //velocity either 10 or -10?
        //set the velocity of player's rigidbody using vector(x,y)

        anim.SetFloat("Speed", Mathf.Abs(move)); //check if speed is not 0 (get 
        //absolute value and set it equal to speed float). It's either 1 or not

        if (move > 0 && !facingRight)
        { Flip(); }
        else if (move < 0 && facingRight)
        { Flip(); }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //go from 1 to -1 to 1 again
        transform.localScale = theScale;
    }
}
