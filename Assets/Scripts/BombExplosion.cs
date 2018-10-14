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

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, this.explosionDuration);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        ExplosionTriggerCheck();
    }

    private void ExplosionTriggerCheck()
    {
        //new Vector2(BombScript.getExplosionPosition().x, BombScript.getExplosionPosition().y)
        if (Physics2D.OverlapCircle(gameObject.transform.position, 
            2f, ExplosionLayers)) //100 radius as extreme...
        {
            Debug.Log(true);
            Explode();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
    }

}
