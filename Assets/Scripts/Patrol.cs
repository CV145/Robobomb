using System.Data.SqlTypes;
using System.Net.Sockets;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    public float speed;
    public bool movingRight;
    public Transform collisionDetection;
    public LayerMask walls;
    bool isAlive = true;
    public bool isItem;
    Vector2 direction;
    GameObject player;
    PickupsAndStats stats;

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
        speed += 10;
        

        if (stats.Kills < 20)
        {
            speed += 1.9f * stats.Kills;
        }
        else if (stats.Kills >= 20 && stats.Kills < 50)
        {
            speed += 1.9f * 20;
        }
        else if (stats.Kills >= 50 && stats.Kills < 100)
        {
            speed += 1.9f * 25;
        }
        else
        {
            speed += 1.9f * 30;
        }
        
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


        if (isAlive && player.GetComponent<GameControl>().GameStart)
        {
                if (!isItem)
                {
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                transform.Translate(Vector2.right * speed * Time.deltaTime);
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
