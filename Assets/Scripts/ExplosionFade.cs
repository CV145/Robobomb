using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFade : MonoBehaviour {

    SpriteRenderer renderer;
    Color color;
    GameObject Robo;

    // Use this for initialization
    void Start () {

        Robo = GameObject.Find("RoboPlayer");

        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 1)
        {
            Camera.main.GetComponent<CameraControl>().Shake(1f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 2)
        {
            Camera.main.GetComponent<CameraControl>().Shake(2f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 3)
        {
            Camera.main.GetComponent<CameraControl>().Shake(3f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 4)
        {
            Camera.main.GetComponent<CameraControl>().Shake(4f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 5)
        {
            Camera.main.GetComponent<CameraControl>().Shake(5f, 10, 50); // 1f, 2f, 3f, 4f... 6f, 10 shakes death bomb?
        }

        renderer = GetComponent<SpriteRenderer>();
        color = renderer.material.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 1)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.01f, .01f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 2)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.02f, .02f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 3)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.03f, .03f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 4)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.04f, .04f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        }
        if (Robo.GetComponent<PickupsAndStats>().GetFireLV() == 5)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.05f, .05f); //.01, .02, .03, .04 <- possible fire level increases? ... death bomb .08f??
        }

        color.a -= 0.01f;
        renderer.material.color = color; // fully opaque
    }
}
