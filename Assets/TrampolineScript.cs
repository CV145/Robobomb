using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{

    GameObject Player;
    public LayerMask jumpLayer;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("RoboPlayer");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(28, 8), 0f, jumpLayer))
            {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * 250;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Physics2D.OverlapBox(transform.position, new Vector2(28, 8), 0f, jumpLayer)
    //        && Player.GetComponent<PlayerController>().GetGrounded())
    //    {
    //        Player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 200;
    //    }
    //}
}
