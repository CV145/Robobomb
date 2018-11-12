using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 50f;
    public bool facingRight = true;
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
    //bool dropDown = false;
    bool hangOver;
    RigidbodyConstraints2D originalConstraints;
    public bool stopMoving = false;
    public bool arcade;
    Vector3 startPosition;
    //bool kicking;

    public PickupsAndStats RoboStats;

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
        if (!arcade)
        {
            originalConstraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            originalConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            startPosition = new Vector2(transform.position.x, transform.position.y);
        }
    }

    //
    /// ////////////
    
    void FixedUpdate()
    {
        ////JUMP SETUP//// 
        groundBoxSize.x = 10f;
        groundBoxSize.y = 0.3f;
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        grounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);


        ///GRAB SETUP///
        ///
        lowerBoxSize.x = 5;
        lowerBoxSize.y = 20;
        lowerHit = Physics2D.Raycast(lowerCheck.position, direction, 5f, grabbables);
        //check if lower hit is true then do ledge grab function

        ///HORIZONTAL MOVEMENT////
        ///
        if (!stopMoving)
        {
            move = Input.GetAxisRaw("Horizontal");
        }

        Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);

        if (!hanging)
        {
            if (!arcade) //if arcade is false change velovity and move normally
            {
                anim.SetFloat("Speed", Mathf.Abs(move));
                Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);
            }
            //else //if arcade then slightly budge in corresponding direction
            //{
            //    //if (facingRight)
                //{
                //    //should budge through a double tap. First tap changes direction
                //    if (Input.GetKeyDown(KeyCode.RightArrow))
                //    {
                //        if (grounded && !kicking && !drop)
                //        {
                //            Rigidbody.constraints = RigidbodyConstraints2D.None; //remove constraints on X
                //            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                //            transform.position = new Vector2(transform.position.x + 4, transform.position.y);
                //            StartCoroutine("MoveForABit");
                //            Rigidbody.constraints = originalConstraints;   
                //        }
                //    }
                //}
                //else if (!facingRight)
                //{
                //    if (Input.GetKeyDown(KeyCode.LeftArrow))
                //    {
                //        if (grounded && !kicking && !drop)
                //        {
                //            Rigidbody.constraints = RigidbodyConstraints2D.None; //remove constraints on X
                //            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                //            transform.position = new Vector2(transform.position.x - 4, transform.position.y);
                //            StartCoroutine("MoveForABit");
                //            Rigidbody.constraints = originalConstraints;
                //        }
                //    }
                //}
            //}





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
                if (!arcade)
                {
                    anim.SetFloat("Speed", Mathf.Abs(move));
                    Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);
                }
            }
        }

        ///RIGHT///
        ///

        if (rightDown)
        {          
                move = 1f;
            if (!hanging)
            {
                if (!arcade)
                {
                    anim.SetFloat("Speed", Mathf.Abs(move));
                    Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);
                }
            }
        }

        /////LEDGE CONTROLS/////
        //if (hanging)
        //{
        //    if (facingRight)
        //    {
        //        if (move > 0)
        //        {
        //            if (hangOver == true) //when hanging animation finishes set bool
        //            {
        //                Vector2 climbPos = new Vector2(transform.position.x, transform.position.y); //current position
        //                GetComponent<Transform>().position = new Vector2(climbPos.x + 12, climbPos.y + 24); //change Robo position
        //                anim.SetBool("Climb", false); //ends the hanging animation
        //                 //freeze position for a bit
        //                StartCoroutine("BriefPause");
        //                Rigidbody.gravityScale = 40; //reset gravity
        //                hanging = false;
        //                hangOver = false;
        //            }
        //        }
        //        if (move < 0)
        //        {
        //            anim.SetBool("Climb", false);
        //            Rigidbody.gravityScale = 40;
        //            hanging = false;
        //            hangOver = false;
        //        }
        //    }

        //    else if (!facingRight)
        //    {
        //        if (move < 0)
        //        {
        //            if (hangOver == true) //when hanging animation finishes set bool
        //            {
        //                Vector2 climbPos = new Vector2(transform.position.x, transform.position.y); //current position
        //                GetComponent<Transform>().position = new Vector2(climbPos.x - 12, climbPos.y + 24); //change Robo position
        //                anim.SetBool("Climb", false); //ends the hanging animation
        //                //freeze position for a bit
        //                StartCoroutine("BriefPause");
        //                Rigidbody.gravityScale = 40; //reset gravity
        //                hanging = false;
        //                hangOver = false;
        //            }
        //        }
        //        if (move > 0)
        //        {
        //            anim.SetBool("Climb", false);
        //            Rigidbody.gravityScale = 40;
        //            hanging = false;
        //            hangOver = false;
        //        }
        //    }
        //}


        ////////// FLIPPING LEFT OR RIGHT /////
        if (move > 0 && !facingRight)
        { Flip(); }
        else if (move < 0 && facingRight)
        { Flip(); }
        //////////
    }

    //IEnumerator MoveForABit ()
    //{
    //    kicking = true;
    //    yield return new WaitForSeconds(.22f); //wait for .2 secs
    //    transform.position = startPosition;
    //    kicking = false;
    //    leftDown = false;
    //    rightDown = false;
    //}


    IEnumerator BriefPause()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        stopMoving = true;
        move = 0;
        yield return new WaitForSeconds(.6f);
        stopMoving = false;
        Rigidbody.constraints = originalConstraints; //unfreeze
    }

    IEnumerator BrieferPause()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        stopMoving = true;
        move = 0;
        yield return new WaitForSeconds(.15f);
        stopMoving = false;
        Rigidbody.constraints = originalConstraints; //unfreeze
    }

    public bool GetGrounded()
    {
        return grounded;
    }

    private void Update()
    {
        /////Constraints when starting game//////
        ///
        if (GetComponent<GameControl>().GameStart == false)
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else if (GetComponent<GameControl>().GameStart == true)
        {
            Rigidbody.constraints = originalConstraints;
        }

        ///LEDGE HANGING///
        ///
        if (lowerHit)
        {
            Debug.Log("lower is hit");
            LedgeGrab();
        }
        //////
        ///

        ///CHECK DIRECTION///
        ///
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

        if (RoboStats.GetBombsOnScreen() < RoboStats.GetBombLV() && !hanging)
        {
            if (Input.GetKey(KeyCode.Z) == true || Input.GetKey(KeyCode.X) == true)
            {
                anim.SetBool("Ready", true);
            }
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
            {
                anim.SetBool("Ready", false);
                anim.SetBool("Fire", true);
                throwed = true;
            }

            //////////////////
            ///
            ////BOMB DROPPING////

            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    if (!bombNotDown && !bombDown && !kicking)
            //    {
            //        if (anim.GetBool("Ready") == false)
            //        {
            //            anim.SetBool("Fire", true); //bomb will only spawn if animation works because anim event
            //            drop = true;
            //        }
            //    }
            //}
        }
        else
        {
            anim.SetBool("Ready", false);
            anim.SetBool("Fire", false);
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
    }

    void LedgeGrab()
    {
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

        //if (!facingRight)
        //{
        //    if (grounded && !kicking && !drop)
        //    {
        //        Rigidbody.constraints = RigidbodyConstraints2D.None; //remove constraints on X
        //        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //        transform.position = new Vector2(transform.position.x - 4, transform.position.y);
        //        StartCoroutine("MoveForABit");
        //        Rigidbody.constraints = originalConstraints;
        //    }
        //}
    }

    public void LeftUp()
    {
        leftDown = false;
    }

    public void RightClick()
    {
        rightDown = true;
        leftDown = false;

        //if (facingRight)
        //{
        //    if (grounded && !kicking && !drop)
        //    {
        //        Rigidbody.constraints = RigidbodyConstraints2D.None; //remove constraints on X
        //        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //        transform.position = new Vector2(transform.position.x + 4, transform.position.y);
        //        StartCoroutine("MoveForABit");
        //        Rigidbody.constraints = originalConstraints;
        //    }
        //}
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
    { //ready
       anim.SetBool("Ready", true);
    }

    public void BombClickUp()
    {//fire
        if (RoboStats.GetBombsOnScreen() < RoboStats.GetBombLV() && !hanging)
        {
            anim.SetBool("Ready", false);
            anim.SetBool("Fire", true);
            throwed = true;
            //drop = false;
        }
    }

    public void JumpUp()
    {
        jumpDown = false;
        isJumping = false;
    }

    //public void BombDropClick() //even that fires when the bomb drop UI is clicked
    //{
    //    if (RoboStats.GetBombsOnScreen() < RoboStats.GetBombLV() && !hanging) //only do this if not hanging and bomb limit not reached
    //    {
    //        if (anim.GetBool("Ready") == false && !kicking)
    //        { 
    //            //a bomb drop is instant, but you can't drop a bomb if you're preparing to throw one
    //            anim.SetBool("Fire", true); //begins the fire animation, which then leads to the FireDone()
    //            drop = true; //used to tell FireDone() that we're dropping a bomb, not throwing
    //        }
    //    }
    //}

    //This event goes off when the fire animation is over. It checks which button was pressed to spawn 
    //the corresponding bomb
    public void FireDone()
    {
        if (throwed) //do this is you're bomb throwing
        {
            //stop the fire animation and spawn a bomb depending on where you're facing
            
            anim.SetBool("Fire", false);
            if (facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 90));
            }
            if (!facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 90));
            }
            //Reset to be able to begin process again and briefly pause after the throw
            throwed = false;
            StartCoroutine("BrieferPause");
        }

        //if (drop) //do this if you're bomb dropping
        //{
        //   //stop the fire animation, then spawn a new bomb drop depending on whether facing right or not
        //   //Afterwards drop bools are set to false to begin process over again
        //        anim.SetBool("Fire", false);
        //        if (facingRight)
        //        {
        //            Instantiate(BombDrop, new Vector2(transform.position.x + 7, transform.position.y - 6), Quaternion.Euler(0, 0, 90));
        //        }
        //        if (!facingRight)
        //        {
        //            Instantiate(BombDrop, new Vector2(transform.position.x - 7, transform.position.y - 6), Quaternion.Euler(0, 0, 90));
        //        }
        //        drop = false;
        //    //Pause briefly afterwards
        //    StartCoroutine("BrieferPause");

        //}
    }

    public void hangFinished() //calls out that player is no longer hanging anymore
    {
        hangOver = true;
    }
}
