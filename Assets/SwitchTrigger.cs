using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    public Animator anim;
    private bool hit;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool GetHit()
    {
        return hit;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.layer == 15)
        {
            anim.SetBool("Hit", true);
            hit = true;
        }
    }
}