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
    public LayerMask DefaultExplosions; //explode when not kicked when hitting an enemy or explosion
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
        Robo.GetComponent<PickupsAndStats>().AnotherBombOnScreen();
    }

    public void Explode()
    {
        Robo.GetComponent<PickupsAndStats>().ABombLessOnScreen();
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, this.explosionDuration);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        DefaultExplosionTriggerCheck();
        if (kicked == false)
        {
            KickCheck();
        }
        if (kicked == true)
        {
            ExplosionTriggerCheck();
        }
    }

    private void KickCheck()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, 2f, Player))
        {
            kicked = true;
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

    private void DefaultExplosionTriggerCheck()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, 2f, DefaultExplosions))
        {
            Explode();
        }
    }

    private void ExplosionTriggerCheck() 
    {
        if (Physics2D.OverlapBox(gameObject.transform.position, new Vector2 (5f, 0.1f),
            0f, ExplosionLayers)) 
        {
            Explode();
        }
    }
}
