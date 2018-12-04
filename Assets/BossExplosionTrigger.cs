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

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        transparent = false;
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
    }
	// Update is called once per frame
	void Update () {

        //Check if dead
        DestroySelf();

	    if (isHit)
        {
            //Begin counting down from 3 using timer. During this time make game object blink
            timer -= Time.deltaTime;

            if (timer <= temp)
            {
                //This function should be in charge of blinking
                TransparentCheck();
                //Subtract temp to repeat
                temp -= 0.15f;
            }
            //When timer reaches 0 restart and set isHit to false
            if (timer <= 0)
            {
                timer = 3;
                temp = 3;
                isHit = false;
                transparent = false;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
            }
        }
	}

    //Function that flips between transparent or not
    void TransparentCheck()
    {
        if (!transparent)
        {
            //Makes transparent
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0f);
            transparent = true;
            return;
        }
        if (transparent)
        {
            //Brings back sprite
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
            transparent = false;
            return;
        }
    }


    //Function that destroys seld when health reaches 0
    void DestroySelf()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
