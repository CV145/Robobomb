using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropExplosion : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float explosionDuration = 1f;
    public LayerMask ExplosionLayers;
    public LayerMask Player; //layer mask for the player
    public BombDrop BombDropScript;
    bool kicked = false;
    GameObject Robo;
    PlayerController playerScript;
    Rigidbody2D Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Robo = GameObject.Find("RoboPlayer");
        playerScript = Robo.GetComponent<PlayerController>();

       /* if (playerScript.FacingRightGetter())
        {
            Rigidbody.AddForce(new Vector2(3, Rigidbody.transform.position.y), ForceMode2D.Force); //maybe increase y vector
        }
        if (playerScript.FacingRightGetter() == false)
        {
            Rigidbody.AddForce(new Vector2(-3, Rigidbody.transform.position.y), ForceMode2D.Force);
        } */
    }

    public void Explode()
    {
        Camera.main.GetComponent<CameraControl>().Shake(0.5f, 10, 50);
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, this.explosionDuration);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (kicked == false)
        {
            KickCheck();
        }
        ExplosionTriggerCheck(); 
    }

    private void KickCheck()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, 2f, Player))
        {
            kicked = true;
            Debug.Log("true");
            if (playerScript.FacingRightGetter())
            {
                Rigidbody.velocity = new Vector2(100, Rigidbody.velocity.y); 
            }
            if (playerScript.FacingRightGetter() == false)
            {
                Debug.Log("true");
                Rigidbody.velocity = new Vector2(-100, Rigidbody.velocity.y);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
    }


    private void ExplosionTriggerCheck() 
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position,
            2f, ExplosionLayers)) 
        {
            Explode();
        }
    }
}
