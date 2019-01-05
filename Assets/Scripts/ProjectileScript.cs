using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    //This script handles any and all projectiles. 
    //It has variables that keep track of the direction the projectile moves and its speed
    //It's also in charge of moving the transform of the projectile each frame


    //Game object for Robo
    GameObject robo;
    //Float that determines the projectile's speed
    public float speed;
    //Timer that destroys this game object when reaching 0
     float timer;

    private void Start()
    {
        robo = GameObject.Find("RoboPlayer");
        //Change projectile direction depending on owner position
        SetDirection();
        timer = 10f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If colliding against player, wall, or explosion - destroy self
        if (collision.gameObject.layer == 9 ||
            collision.gameObject.layer == 10 ||
            collision.gameObject.layer == 15)
        {
            Destroy(this.gameObject);
        }
    }

    // Public function that sets direction on whether right or left of Robo
    public void SetDirection()
    {
        if (transform.position.x > robo.transform.position.x)
        {
            speed *= -1;
        }
        else if (transform.position.x < robo.transform.position.x)
        {
            speed *= 1;
        }
    }

    // Update is called once per frame
    void Update () {
        //Move the projectile
        transform.position = new Vector2(transform.position.x + speed, transform.position.y);
        //Countdown timer and destroy game object when at 0
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
