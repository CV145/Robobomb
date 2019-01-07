using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    public LayerMask whatIsGround;
    Rigidbody2D rigidbody;
    Transform player;
    bool facingRight;
    public Patrol patrol;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        facingRight = false;
    }

    private void Awake()
    {
        player = GameObject.Find("RoboPlayer").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 4), 0.4f, whatIsGround))
        {
            rigidbody.velocity = Vector2.up * 25;
        }
    }


}
