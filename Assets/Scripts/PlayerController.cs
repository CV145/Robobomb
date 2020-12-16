using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*Player controller script is in charge of controlling Robo. Controls can be used on both PC and mobile.
     * Robo can jump, throw bombs, and change directions. 
     * This script also handles animations.
         */


    public float maxSpeed = 50f;
    public bool facingRight = true;
    Rigidbody2D Rigidbody;
    Animator anim;
    public float jumpForce = 3000f;
    public bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.6f;
    public LayerMask whatIsGround;
    [SerializeField]
    private float jumpTimeCounter;
    public float jumpTime;
    [SerializeField]
    private bool isJumping;
    //Sounds
    public AudioClip jumpLand;
    public AudioSource soundSource;
    public AudioClip jump;
    public AudioSource jumpSource;
    public AudioClip bombThrow;
    public AudioSource throwSource;
    //
 
    Vector2 direction;
    Vector2 groundBoxSize;
    Vector2 lowerBoxSize;
    float move;

    bool leftDown = false;
    bool rightDown = false;
    public bool jumpDown = false;
    public GameObject Bomb;
    bool throwed = false;
    RigidbodyConstraints2D originalConstraints;
    public bool stopMoving = false;
    public bool arcade;
    Vector3 startPosition;

    public PickupsAndStats RoboStats;


    public bool FacingRightGetter()
    {
        return facingRight;
    }


    void Start()
    {
        //When the game begins, set the animator controller, rigidbody, and sounds
        Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundSource.clip = jumpLand;
        jumpSource.clip = jump;
        throwSource.clip = bombThrow;
        originalConstraints = RigidbodyConstraints2D.FreezeRotation;
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

        ///HORIZONTAL MOVEMENT////
        ///
        if (!stopMoving)
        {
            move = Input.GetAxisRaw("Horizontal");
        }

        Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);

        ///////////////////////
        ///
        

        /////HORIZONTAL MOBILE MOVEMENT/////

        ///LEFT///
        if (leftDown)
        {
            move = -1;
                    Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);            
        }

        ///RIGHT///
        ///
        if (rightDown)
        {          
                move = 1f;
                    Rigidbody.velocity = new Vector2(move * maxSpeed, Rigidbody.velocity.y);
        }

        ////////// FLIPPING LEFT OR RIGHT /////
        if (move > 0 && !facingRight)
        { Flip(); }
        else if (move < 0 && facingRight)
        { Flip(); }
        //////////
        ///

        //Constantly check if there's movement to animate
        anim.SetFloat("Speed", Mathf.Abs(move));
    }


    //Pause when Robo throws a bomb
    IEnumerator BrieferPause()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        stopMoving = true;
        float temp = move;
        move = 0;
        yield return new WaitForSeconds(.15f);
        stopMoving = false;
        move = temp;
        Rigidbody.constraints = originalConstraints; //unfreeze
    }

    //Public function that can tell another script if Robo is grounded or not
    public bool GetGrounded()
    {
        return grounded;
    }

IEnumerator TouchRight()
    {
        rightDown = true;
        yield return new WaitForSeconds(.15f);
        rightDown = false;
        movedRight = true;
    }

bool movedRight = false;
    private void Update()
    {
        /////Constraints when starting game//////
        ///
        if (GetComponent<GameControl>().GameStart == false)
        {
            //Freeze Robo in midair until PLAY is pressed
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else if (GetComponent<GameControl>().GameStart == true)
        {
            //Set constraints to original ones in Start() 
            Rigidbody.constraints = originalConstraints;
            if (!movedRight)
            StartCoroutine("TouchRight");
        }

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


        ////BOMB THROWING///
        ///

        if (RoboStats.GetBombsOnScreen() < RoboStats.GetBombLV())
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
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //go from 1 to -1 to 1 again
        transform.localScale = theScale;
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
    { //ready
       anim.SetBool("Ready", true);
    }

    public void BombClickUp()
    {//fire
        if (RoboStats.GetBombsOnScreen() < RoboStats.GetBombLV())
        {
            anim.SetBool("Ready", false);
            anim.SetBool("Fire", true);
            throwed = true;
        }
    }

    public void JumpUp()
    {
        jumpDown = false;
        isJumping = false;
    }

    //This event goes off when the fire animation is over. It checks which button was pressed to spawn 
    //the corresponding bomb
    public void FireDone()
    {
        if (throwed) //do this is you're bomb throwing
        {
            //stop the fire animation and spawn a bomb depending on where you're facing
            throwSource.Play();
            anim.SetBool("Fire", false);
            if (facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 0));
            }
            if (!facingRight)
            {
                Instantiate(Bomb, new Vector2(transform.position.x, transform.position.y - 3), Quaternion.Euler(0, 0, 0));
            }
            //Reset to be able to begin process again and briefly pause after the throw
            throwed = false;
            StartCoroutine("BrieferPause");
        }
    }
}
