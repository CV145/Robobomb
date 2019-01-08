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
        speed = speed + (2.5f * stats.GetFireLV()) + (2.5f * stats.GetBombLV());
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
