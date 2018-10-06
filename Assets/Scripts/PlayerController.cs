using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 50f;
    bool facingRight = true;
    Rigidbody2D Rigidbody;
    Animator anim;
    public float jumpForce = 3000f;
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.6f;
    public LayerMask whatIsGround;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public AudioClip jumpLand;
    public AudioSource soundSource;
    public AudioClip jump;
    public AudioSource jumpSource;

    bool upperHit = false;
    bool lowerHit = false;
    public LayerMask walls;
    Vector2 direction;
    public Transform lowerCheck;
    public Transform upperCheck;
    bool hanging = false;
    Vector2 groundBoxSize;
    Vector2 upperBoxSize;
    Vector2 lowerBoxSize;
    RigidbodyConstraints2D originalConstraints;


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundSource.clip = jumpLand;
        jumpSource.clip = jump;
        originalConstraints = Rigidbody.constraints;
    }


    void FixedUpdate()
    {
        ////JUMP SETUP//// 
        groundBoxSize.x = 1f;
        groundBoxSize.y = 0.5f;
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        grounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 90f, whatIsGround);

        ///HORIZONTAL MOVEMENT////
        ///

        float move = Input.GetAxis("Horizontal");

        if (!hanging)
        {
            
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
        }
        ///////////////////////

        /////LEDGE CONTROLS/////
        if (hanging)
        {
            if (facingRight)
            {
                if (move > 0)
                {
                    //climb up
                    //public Vector2 climbPos;

                   // GetComponent<Transform>().position = new Vector2(climPos.x + 3,
                   // climbPos.y + 3);
                }
                if (move < 0)
                {
                    anim.SetBool("Climb", false);
                    
                    Rigidbody.gravityScale = 50;
                }
            }

            else if (!facingRight)
            {
                if (move < 0)
                {
                    //climb up
                }
                if (move > 0)
                {
                    anim.SetBool("Climb", false);
                    Rigidbody.gravityScale = 50;
                }
            }
        }


        ////////// FLIPPING LEFT OR RIGHT /////
        if (move > 0 && !facingRight)
        { Flip(); }
        else if (move < 0 && facingRight)
        { Flip(); }
        //////////
    }

    private void Update()
    {
        ///LEDGE HANGING///
        LedgeGrab();
        //////
        ///

        ///GRAVITY FAILSAFE//
        ///
        if (anim.GetFloat("vSpeed") > 0 && anim.GetBool("Climb") == true)
        {
            Rigidbody.gravityScale = 50f;
        }
        ////

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
        if (maxSpeed >= 90)
        {
            //play skid animation
        }
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //go from 1 to -1 to 1 again
        transform.localScale = theScale;

        if (facingRight)
        {
            direction.x = 1;
            direction.y = 0;
        }
        else if (!facingRight)
        {
            direction.x = -1;
            direction.y = 0;
        }
    }

    void LedgeGrab()
    {
        upperBoxSize.x = 10;
        upperBoxSize.y = 20;

        lowerBoxSize.x = 10;
        lowerBoxSize.y = 20;

        //lowerCheck.GetComponent<BoxCollider2D>().transform.position;

        upperHit = Physics2D.OverlapBox(upperCheck.position, upperBoxSize, 90f, walls);
        lowerHit = Physics2D.OverlapBox(lowerCheck.position, lowerBoxSize, 90f, walls);


        if (grounded == false && anim.GetFloat("vSpeed") <= 0)
        {
            if (lowerHit && !upperHit)
            {
                hanging = true;
                Rigidbody.gravityScale = 0.0f;
                Rigidbody.velocity = new Vector2(0, 0);
                anim.SetBool("Climb", hanging);

                if (facingRight)
                {
                    Rigidbody.MovePosition(new Vector2(lowerCheck.position.x + 3, lowerCheck.position.y + 5));
                }
                if (!facingRight)
                {
                    Rigidbody.MovePosition(new Vector2(lowerCheck.position.x - 3, lowerCheck.position.y + 5));
                }
            }
        }
        else
        {
            hanging = false;
        }

        
        Debug.Log(hanging);
    }

}
