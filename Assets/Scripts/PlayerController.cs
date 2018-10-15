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

 
    bool lowerHit = false;
    public LayerMask grabbables;
    Vector2 direction;
    public Transform lowerCheck;
    bool hanging = false;
    Vector2 groundBoxSize;
    Vector2 lowerBoxSize;
    float move;

    bool leftDown = false;
    bool rightDown = false;
    bool jumpDown = false;
    public GameObject Bomb;
    bool bombDown;
    bool bombNotDown;
    public GameObject BombDrop;
    bool drop = false;
    bool throwed = false;
    bool dropDown = false;
    bool hangOver;
    RigidbodyConstraints2D originalConstraints;
    public bool stopMoving = false;

    public bool FacingRightGetter()
    {
        return facingRight;
    }


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundSource.clip = jumpLand;
        jumpSource.clip = jump;
        originalConstraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void FixedUpdate()
    {
        ////JUMP SETUP//// 
        groundBoxSize.x = 10f;
        groundBoxSize.y = 0.5f;
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        grounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);

        ///HORIZONTAL MOVEMENT////
        ///

        if (!stopMoving)
        {
            move = Input.GetAxisRaw("Horizontal");
        }

  if (!hanging)
        {
            anim.SetFloat("Speed", Mathf.Abs(move));
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
                Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);
            }
        }

        ///RIGHT///
        ///

        if (rightDown)
        {          
                move = 1f;
                if (!hanging)
                {

                    anim.SetFloat("Speed", Mathf.Abs(move));
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
                    if (hangOver == true) //when hanging animation finishes set bool
                    {
                        Vector2 climbPos = new Vector2(transform.position.x, transform.position.y); //current position
                        GetComponent<Transform>().position = new Vector2(climbPos.x + 12, climbPos.y + 24); //change Robo position
                        anim.SetBool("Climb", false); //ends the hanging animation
                         //freeze position for a bit
                        StartCoroutine("BriefPause");
                        Rigidbody.gravityScale = 50; //reset gravity
                        hanging = false;
                        hangOver = false;
                    }
                }
                if (move < 0)
                {
                    anim.SetBool("Climb", false);
                    Rigidbody.gravityScale = 50;
                    hanging = false;
                    hangOver = false;
                }
            }

            else if (!facingRight)
            {
                if (move < 0)
                {
                    if (hangOver == true) //when hanging animation finishes set bool
                    {
                        Vector2 climbPos = new Vector2(transform.position.x, transform.position.y); //current position
                        GetComponent<Transform>().position = new Vector2(climbPos.x - 12, climbPos.y + 24); //change Robo position
                        anim.SetBool("Climb", false); //ends the hanging animation
                        //freeze position for a bit
                        StartCoroutine("BriefPause");
                        Rigidbody.gravityScale = 50; //reset gravity
                        hanging = false;
                        hangOver = false;
                    }
                }
                if (move > 0)
                {
                    anim.SetBool("Climb", false);
                    Rigidbody.gravityScale = 50;
                    hanging = false;
                    hangOver = false;
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

    IEnumerator BriefPause()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        stopMoving = true;
        move = 0;
        yield return new WaitForSeconds(.6f);
        stopMoving = false;
        Rigidbody.constraints = originalConstraints; //unfreeze
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


        ////BOMB THROWING///
        ///
        if (Input.GetKey(KeyCode.Z) == true || Input.GetKey(KeyCode.X) == true || bombDown == true)
        {
            anim.SetBool("Ready", true);
        }
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) || bombNotDown == true) 
        {
            anim.SetBool("Ready", false);
            anim.SetBool("Fire", true);
            throwed = true;
        }

        //////////////////
        ///
        ////BOMB DROPPING////

        if (Input.GetKeyDown(KeyCode.DownArrow) || dropDown == true)
        {
            anim.SetBool("Fire", true); //bomb will only spawn if animation works because anim event
            drop = true;
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
        lowerBoxSize.x = 5;
        lowerBoxSize.y = 20;
        lowerHit = Physics2D.Raycast(lowerCheck.position, direction, 5f, grabbables);

        


        if (grounded == false && anim.GetFloat("vSpeed") < 100)
        {
            if (lowerHit)
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

    public void BombClick()
    {
        bombDown = true;
        bombNotDown = false;
        Debug.Log("bombDown: " + bombDown);
        Debug.Log("bombNotDown: " + bombNotDown);
    }

    public void BombClickUp()
    {
        bombDown = false;
        bombNotDown = true;
    }

    public void JumpUp()
    {
        jumpDown = false;
        isJumping = false;
    }

    public void BombDropClick()
    {
        dropDown = true;
    }

    //This event goes off when the fire animation is over
    public void FireDone()
    {
        if (throwed)
        {
            anim.SetBool("Fire", false);
            if (facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 90));
            }
            if (!facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 90));
            }
            bombDown = false;
            bombNotDown = false;
            throwed = false;
        }

        if (drop)
        {
            anim.SetBool("Fire", false);
            if (facingRight)
            {
                Instantiate(BombDrop, new Vector2(transform.position.x + 7, transform.position.y - 6), Quaternion.Euler(0, 0, 90));
            }
            if (!facingRight)
            {
                Instantiate(BombDrop, new Vector2(transform.position.x - 7, transform.position.y - 6), Quaternion.Euler(0, 0, 90));
            }
            drop = false;
            dropDown = false;
          //  dropNotDown = false;
        }
    }

    public void hangFinished()
    {
        hangOver = true;
    }
}
