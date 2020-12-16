using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour {
    Rigidbody2D Rigidbody;
    private Vector2 explosionPosition;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public Vector2 getExplosionPosition()
    {
        return explosionPosition;
    }

    // Update is called once per frame
    void Update()
    {
        explosionPosition = Rigidbody.transform.position;
    }
}
