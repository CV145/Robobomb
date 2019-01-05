using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour {
    //This script spawns random clouds


    //Game object for cloud
    public GameObject cloud;
    //Timer for randomness
    float timer;
    //Period between spawns is a range between two numbers that a spawn will randomly occur in
    float periodBetweenSpawn;

	// Use this for initialization
	void Start () {
        //Choose a random number
        periodBetweenSpawn = Random.Range(0, 10);
        timer = periodBetweenSpawn; //set timer to random number and begin counting down
    }
	
	// Update is called once per frame
	void Update () {
        //Every frame reduce timer by delta time
        timer -= Time.deltaTime;

        //When timer reaches 0
        if (timer <= 0)
        {
            //Instantiate cloud randomly 50 pixels above or below spawner position
            Instantiate(cloud, new Vector2(transform.position.x, Random.Range(transform.position.y - 50, transform.position.y + 50)
                ), Quaternion.Euler(0, 0, 0));
            //Reset to start over again
            periodBetweenSpawn = Random.Range(0, 100);
            timer = periodBetweenSpawn; //then reset timer to start over again 
        }
    }
}
