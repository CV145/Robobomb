using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float maxSpeed = 50f;
    bool facingRight = true;
    Rigidbody2D Rigidbody;
    Animator anim;
    public float jumpForce = 1500f;

    bool grounded = false;
    public Transform groundCheck; //a transform holds vectors for positions, rotations, and scales of an object
    //This is the transform of the GroundCheck game object
    float groundRadius = 0.2f;
    public LayerMask whatIsGround; //decides what "ground" is

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //if grounded is true we're on the ground, if not we're not. This is being check every frame
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", Rigidbody.velocity.y); //get y velocity and set to vertical speed


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

    private void Update()
    {
        //input more accurate in update
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow)) //check if either w or up arrow... try KeyCode.UpArrow too
        {
            anim.SetBool("Ground", false); //jumping immediately makes grounded false
            Rigidbody.AddForce(new Vector2(0, jumpForce)); 
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //go from 1 to -1 to 1 again
        transform.localScale = theScale;
    }
}
