using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour {

    public LayerMask Explosions;
    public GameObject Fireup;
    public GameObject Bombup;
    bool instantiated;

    public bool spawnFireup;
    public bool spawnBombup;
    public bool spawnHeart;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Physics2D.OverlapBox(transform.position, new Vector2(16, 16), 0f, Explosions))
        {
            Object.Destroy(gameObject, 1f);
            if (!instantiated)
            {
                if (spawnFireup)
                    {
                    Instantiate(Fireup, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                    instantiated = true;
                }
                if (spawnBombup)
                {
                    Instantiate(Bombup, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                    instantiated = true;
                }
                if (spawnHeart)
                {

                }
            }
        }
	}
}
