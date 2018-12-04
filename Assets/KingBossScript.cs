using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBossScript : MonoBehaviour {
    //A script for behavior unique to this boss
    

    //Timer variable
    float timer;
    //Bool that tells which direction currently facing
    public bool facingRight;
    //bool that checks if "grounded". Automatically true at start and set to false when in midair
    bool grounded = true;
    //Get Rigidbody
    Rigidbody2D rigidbody;
    //Get Robo
    GameObject Robo;
    //Original constraints with frozen x position
    RigidbodyConstraints2D originalConstraints;

	// Use this for initialization
	void Start () {
        timer = 3;
        rigidbody = GetComponent<Rigidbody2D>();
        Robo = GameObject.Find("RoboPlayer");
        //originalConstraints = rigidbody.constraints;
    }
	
	// Update is called once per frame
	void Update () {
        //Trying to keep rotation but preventing from falling over
        if (transform.eulerAngles.z > 8)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        timer -= Time.deltaTime;
        //Every 3 seconds, jump. After landing reset timer
        if (timer <= 0)
        {
            //Keep calling function every frame. Timer is reset from within Jump() when its finished
            Jump();
        }
	}


    //Called at the end of a jump
    void Flip()
    {
        //If passing Robo, Flip() directions
        Vector3 theScale = transform.localScale;
        if (transform.position.x > Robo.transform.position.x)
        {
            theScale.x = 1; //Facing left
            facingRight = false;
        }
        else if (transform.position.x < Robo.transform.position.x)
        {
            theScale.x = -1; //Facing right
            facingRight = true;
        }
        
        transform.localScale = theScale;
    }

    //Function that, depending on where boss is facing, jump and move to opposite direction
    void Jump()
    {
        if (facingRight)
        {
            //Jump up and move to X897
            //Increase x and y continouosly if on ground (-170)
            if (grounded)
            {
                transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.7f);
            }
            //Cap out y at -135
            if (transform.position.y >= -135)
            {
                grounded = false;
                transform.position = new Vector2(transform.position.x + 0.5f, -135);
                //Lock gravity
                //rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            }
            //When passing Robo move down
            if (transform.position.x >= Robo.transform.position.x + 5)
            {
                transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.35f);
                //Unlock gravity
                //rigidbody.constraints = originalConstraints;
            }
            //Stop at X897 and reset timer
            if (transform.position.x >= 897)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y);
                timer = 3;
                Flip();
                grounded = true;
            }
        }

        else if (!facingRight)
        {
            //Jump up and move to X785
            //Do the same as above but in reverse direction
            if (grounded)
            {
                transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.7f);
            }
            //Cap at Y-135
            if (transform.position.y >= -135)
            {
                grounded = false;
                transform.position = new Vector2(transform.position.x - 0.5f, -135);
                //Lock gravity
                //rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            }
            //Move down after passing Robo
            if (transform.position.x <= Robo.transform.position.x - 5)
            {
                transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.35f);
                //Unlock gravity
                //rigidbody.constraints = originalConstraints;
            }
            //Stop at X785 and reset timer
            if (transform.position.x <= 785)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y);
                timer = 3;
                Flip();
                grounded = true;
            }
        }
    }
}
