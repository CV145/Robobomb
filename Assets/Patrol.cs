using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    public float speed;
    public bool movingRight = true;
    public Transform collisionDetection;
    public LayerMask walls;
    bool isAlive = true;
    Vector2 direction;
    GameObject player;
    PickupsAndStats stats;
    //the transform of the game object checking for collisions

        public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
        }
    }


    private void Start()
    {
        player = GameObject.Find("RoboPlayer");
        stats = player.GetComponent<PickupsAndStats>();
        speed = speed + (3 * stats.GetFireLV()) + (3 * stats.GetBombLV());
    }


    void Update()
    {
        ///CHECK DIRECTION///
        ///
        if (movingRight)
        {
            direction.x = 1;
            direction.y = 0;
        }
        else if (!movingRight)
        {
            direction.x = -1;
            direction.y = 0;
        }


        if (isAlive)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            //move to the right by your speed each delta time second?
        }
        RaycastHit2D checker = Physics2D.Raycast(collisionDetection.position, direction, 2f, walls);

        if (checker.collider)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //go from 1 to -1 to 1 again
        transform.localScale = theScale;
        speed *= -1;
    }

}
