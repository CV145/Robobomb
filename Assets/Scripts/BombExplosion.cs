using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float explosionDuration = 1f;
    public LayerMask ExplosionLayers;
    public Bomb BombScript;
    GameObject RoboPlayer;
    GameObject explosion;

    public void Start()
    {
        RoboPlayer = GameObject.Find("RoboPlayer");
        RoboPlayer.GetComponent<PickupsAndStats>().AnotherBombOnScreen(); //increase bomb on screen by 1
    }

    public void Explode()
    {
        
        explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, this.explosionDuration);
        RoboPlayer.GetComponent<PickupsAndStats>().ABombLessOnScreen(); //lower bomb on screen by 1
        Destroy(this.gameObject);
    }

    private void Update()
    {
        ExplosionTriggerCheck();
    }

    private void ExplosionTriggerCheck()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, 
            2f, ExplosionLayers)) //100 radius as extreme...
        {
            Explode();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
    }

}
