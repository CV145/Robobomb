﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBossScript : MonoBehaviour {
    //A script for behavior unique to this boss
    

    //Timer variable
    float timer;
    //Bool that tells which direction currently facing
    public bool facingRight;
    //bool that checks if "grounded". Automatically true at start and set to false when in midair
    bool grounded = false;
    //bool that makes game object fall down when spawning
    public bool intro = true;
    //Get Robo
    GameObject Robo;
    //Projectile
    public GameObject Projectile;

	// Use this for initialization
	void Start () {
        timer = 3;
        Robo = GameObject.Find("RoboPlayer");
    }
	
	// Update is called once per frame
	void Update () {
        if (Robo.GetComponent<PickupsAndStats>().Alive)
        {
            
            if (!intro)
            {
                timer -= Time.deltaTime;
                //Every 3 seconds, jump. After landing reset timer
                if (timer <= 0)
                {
                    //Keep calling function every frame. Timer is reset from within Jump() when its finished
                    JumpNShoot();
                }
            }
            //When starting, begin moving down until reaching -170, then set grounded to true 
            if (intro)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
                if (transform.position.y <= -170)
                {
                    intro = false;
                    grounded = true;
                }
            }
        }
        else if (!Robo.GetComponent<PickupsAndStats>().Alive)
        {
            //When Robo is dead, stop in place and laugh at player
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
    void JumpNShoot()
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
            }
            //When passing Robo move down
            if (transform.position.x >= Robo.transform.position.x + 5)
            {
                transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.35f);
            }
            //Stop at X897 and reset timer. Also spawn a projectile
            if (transform.position.x >= 897)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y);
                timer = 3;
                Flip();
                grounded = true;
                //Spawn projectile
                Instantiate(Projectile, new Vector2(transform.position.x, -175), Quaternion.Euler(0, 0, 0));
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
            }
            //Move down after passing Robo
            if (transform.position.x <= Robo.transform.position.x - 5)
            {
                transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.35f);
            }
            //Stop at X785 and reset timer
            if (transform.position.x <= 785)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y);
                timer = 3;
                Flip();
                grounded = true;
                //Spawn projectile
                Instantiate(Projectile, new Vector2(transform.position.x, -175), Quaternion.Euler(0, 0, 0));
            }
        }
    }
}