using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float maxSpeed = 50f;
    bool facingRight = true;
    Rigidbody2D Rigidbody;
    Animator anim;
    public float jumpForce = 3000f;
    bool grounded = false;
    public Transform groundCheck; 
    float groundRadius = 0.2f;
    public LayerMask whatIsGround; 
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public AudioClip jumpLand;
    public AudioSource soundSource;
    public AudioClip jump;
    public AudioSource jumpSource;



    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundSource.clip = jumpLand;
        jumpSource.clip = jump;
    }


    void FixedUpdate()
    {
        ////JUMP SETUP//// 
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        ///HORIZONTAL MOVEMENT////
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));

        if (anim.GetFloat("Speed") >= 1.0)
        {
            maxSpeed += 1; //increase maxSpeed by 1

            if (maxSpeed >= 95)
            {
                maxSpeed = 95;
            }

            if (maxSpeed > 90)
            {
                anim.speed = 2.0f;
            }
        }
        else if (anim.GetFloat("Speed") <= 0)
        {
            maxSpeed = 50;
            anim.speed = 1.0f;
        }

        Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);

        ////////// FLIPPING LEFT OR RIGHT /////
        if (move > 0 && !facingRight)
        { Flip(); }
        else if (move < 0 && facingRight)
        { Flip(); }
        //////////
    }

    private void Update()
    {
        ////JUMPING////

        //Landing sound
        if (Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround) && !grounded)
        {
            soundSource.Play();
        }

        //Pressing up
        if (grounded == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpSource.Play();
            isJumping = true;
            jumpTimeCounter = jumpTime; //reset to initial jump time value
            Rigidbody.velocity = Vector2.up * jumpForce;
        }
        if (Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                Rigidbody.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime; //decrease time counter each second
            }
            else
            {
                isJumping = false;
                
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
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
