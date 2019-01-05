using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosions : MonoBehaviour {
    //This is a script on enemy explosions. Mostly the same as ExplosionFade, but size is customizable here
    //instead of powerup levels. Depending on the game object the explosion can be either friendly or not, 
    //but the idea behind this script is on explosions caused by enemies instead of player

    //Choose strength of flame through enum
    public enum FireStrength
    {
        LV1,
        LV2,
        LV3,
        LV4,
        LV5,
        //LV6?
    }
    public FireStrength FS;

    //For fading out
    SpriteRenderer renderer;
    Color color;

    //For the explosion sound
    public AudioClip explosion;
    public AudioSource explosionSource; 

    // Use this for initialization
    void Start () {
        explosionSource.clip = explosion;
        explosionSource.Play();

        //Camera shakes
        switch (FS)
        {
            case (FireStrength.LV1):
                Camera.main.GetComponent<CameraControl>().Shake(1f, 10, 50);
                break;
            case (FireStrength.LV2):
                Camera.main.GetComponent<CameraControl>().Shake(1.4f, 10, 50);
                break;
            case (FireStrength.LV3):
                Camera.main.GetComponent<CameraControl>().Shake(1.8f, 10, 50);
                break;
            case (FireStrength.LV4):
                Camera.main.GetComponent<CameraControl>().Shake(2.2f, 10, 50);
                break;
            case (FireStrength.LV5):
                Camera.main.GetComponent<CameraControl>().Shake(2.6f, 10, 50);
                break;
        }

        renderer = GetComponent<SpriteRenderer>();
        color = renderer.material.color;

        //Destroy self after 0.4 secs
        Destroy(this.gameObject, 0.65f);
    }
	
	// Update is called once per frame
	void Update () {

        //Explosion scale
        switch (FS)
        {
            case (FireStrength.LV1):
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
           new Vector2(.01f, .01f);
                break;
            case (FireStrength.LV2):
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.014f, .014f);
                break;
            case (FireStrength.LV3):
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.018f, .018f);
                break;
            case (FireStrength.LV4):
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.022f, .022f);
                break;
            case (FireStrength.LV5):
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y) +
            new Vector2(.026f, .026f);
                break;
        }

        color.a -= 0.01f;
        renderer.material.color = color; // fully opaque
    }
}
