using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardbotScript : MonoBehaviour {

    public LayerMask Collidables;
    public float direction = -10;
    Rigidbody2D rigidbody;
    Vector2 myVel;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        myVel = rigidbody.velocity;
        myVel.x = 10;
    }

    void Flip()
    {
        myVel.x *= -1;
    }
	
	// Update is called once per frame. Remember 60 fps
	void Update () {

        rigidbody.velocity = myVel;

        if (Physics2D.OverlapBox(new Vector2 (transform.position.x, transform.position.y - 2),
            new Vector2(12, 8), 0f, Collidables))
        {
            rigidbody.AddForce(new Vector2(-16, 0));
            myVel.x = -10;
            Debug.Log(myVel.x);
        }
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 2),
            new Vector2(-12, 8), 0f, Collidables))
        {
            rigidbody.AddForce(new Vector2(16, 0));
            myVel.x = 10;
           // StartCoroutine("BriefPause");
           // Debug.Log("Flipped");
        }

    }

    IEnumerator BriefPause()
    {
        Flip();
        yield return new WaitForSeconds(.15f);
    }
}
