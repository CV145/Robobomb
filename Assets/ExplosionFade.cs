using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFade : MonoBehaviour {

    SpriteRenderer renderer;
    Color color;

    // Use this for initialization
    void Start () {
        Camera.main.GetComponent<CameraControl>().Shake(1f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?

        renderer = GetComponent<SpriteRenderer>();
        color = renderer.material.color;
    }
	
	// Update is called once per frame
	void Update () {
    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) + 
            new Vector2(.01f, .01f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        color.a -= 0.01f;
        renderer.material.color = color; // fully opaque
    }
}
