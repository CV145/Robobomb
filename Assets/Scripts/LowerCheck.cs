using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerCheck : MonoBehaviour {

    BoxCollider2D collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {

    }
}
