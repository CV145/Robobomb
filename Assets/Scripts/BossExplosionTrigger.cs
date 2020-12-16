using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosionTrigger : MonoBehaviour {

    //Script that keeps track of damage and handles collisions against explosions without patrolling
    //It also has a customizable health variable

    //Health variable
    public int health;
    //Timer used for handling opacity in-out
    float timer;
    //Temp float tracking time transparency changes
    float temp = 3f;
    //Bool that signifies when boss is hit to nullify more damage
    bool isHit;
    //Bool that's in charge of setting game object as transparent or not
    bool transparent;
    //Get the renderer
    SpriteRenderer renderer;
    //Get the animator controller
    Animator animator;

    public bool Hit
    {
        get
        {
            return isHit;
        }
    }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        transparent = false;
        animator = GetComponent<Animator>();
    }

    //Collision function that takes in explosion hits
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            //Take damage only if isHit is false
            if (isHit == false)
            {
                //Lower health here
                health--;
                isHit = true;
                timer = 3;
            }
        }
        //If a bomb hits, play the Deflect animation
        if (collision.gameObject.layer == 17 && !animator.GetBool("isDead") && !animator.GetBool("isLaughing"))
        {
            animator.SetBool("Deflect", true);
        }
    }
	// Update is called once per frame
	void Update () {

        //Check if dead
        DestroySelf();
	}

    public void DeflectFinished()
    {
        animator.SetBool("Deflect", false);
    }

    //Function that destroys seld when health reaches 0
    void DestroySelf()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            //Change color
            renderer.color = new Color(0.89f, 0.52f, 0.22f, 1f);
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
            Destroy(this.gameObject, 2f);
        }
    }
}
