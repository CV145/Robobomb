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
    public LayerMask grabbables;
    Vector2 direction;
    public Transform lowerCheck;
    public Transform upperCheck;
    bool hanging = false;
    Vector2 groundBoxSize;
    Vector2 upperBoxSize;
    Vector2 lowerBoxSize;
    float move;

    bool leftDown = false;
    bool rightDown = false;
    bool jumpDown = false;


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
        groundBoxSize.x = 1f;
        groundBoxSize.y = 0.5f;
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        grounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 90f, whatIsGround);

        ///HORIZONTAL MOVEMENT////
        ///

        move = Input.GetAxis("Horizontal");

        if (!hanging)
        {

            anim.SetFloat("Speed", Mathf.Abs(move));

            if (anim.GetFloat("Speed") >= 1.0)
            {
                maxSpeed += 1; //increase maxSpeed by 1

                if (maxSpeed >= 70)
                {
                    maxSpeed = 70;
                }

                if (maxSpeed > 65)
                {
                    anim.speed = 1.4f;
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
        ///


        /////HORIZONTAL MOBILE MOVEMENT/////

        ///LEFT///
        if (leftDown)
        {
            move = -1;
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
        }

        ///RIGHT///
        ///

        if (rightDown)
        {
            move = 1;
            if (!hanging)
            {

                anim.SetFloat("Speed", Mathf.Abs(move));

                if (anim.GetFloat("Speed") >= 1.0)
                {
                    maxSpeed += 1; //increase maxSpeed by 1

                    if (maxSpeed >= 70)
                    {
                        maxSpeed = 70;
                    }

                    if (maxSpeed > 70)
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
        }

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
        ///
        ///CLIMB FAILSAFE//
        ///
        if (anim.GetBool("Ground") == true && anim.GetBool("Climb") == true)
        {
            anim.SetBool("Climb", false);
        }

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

        ///MOBILE JUMPING UPDATE////
        ///

        if (jumpDown == true && isJumping == true)
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
        upperBoxSize.x = 5;
        upperBoxSize.y = 20;

        lowerBoxSize.x = 5;
        lowerBoxSize.y = 20;

        upperHit = Physics2D.Raycast(upperCheck.position, direction, 5f, grabbables);
        lowerHit = Physics2D.Raycast(lowerCheck.position, direction, 5f, grabbables);

        //Debug.DrawLine(upperCheck.position, new Vector2(upperCheck.position.x + 5, upperCheck.position.y));


        if (grounded == false && anim.GetFloat("vSpeed") < 100)
        {
            if (lowerHit && !upperHit)
            {
                hanging = true;
                Rigidbody.gravityScale = 0.0f;
                Rigidbody.velocity = new Vector2(0, 0);
                anim.SetBool("Climb", hanging);

                if (facingRight)
                {
                    Rigidbody.MovePosition(new Vector2(lowerCheck.position.x, lowerCheck.position.y));
                }
                if (!facingRight)
                {
                    Rigidbody.MovePosition(new Vector2(lowerCheck.position.x, lowerCheck.position.y));
                }
            }
        }
        else
        {
            hanging = false;
        }


    }


    /// MOBILE TOUCH CONTROLS ///

    public void LeftClick() //function called when on-screen button pressed
    {
        leftDown = true;
        rightDown = false;
    }

    public void LeftUp()
    {
        leftDown = false;
    }

    public void RightClick()
    {
        rightDown = true;
        leftDown = false;
    }

    public void RightUp()
    {
        rightDown = false;
    }

    public void JumpClick()
    {
        jumpDown = true;

        if (grounded)
        {
            jumpSource.Play();
            isJumping = true;
            jumpTimeCounter = jumpTime; //reset to initial jump time value
            Rigidbody.velocity = Vector2.up * jumpForce;
        }
    }

    public void JumpUp()
    {
        jumpDown = false;
        isJumping = false;
    }
}
