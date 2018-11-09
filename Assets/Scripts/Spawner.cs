using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public enum ObjectsToSpawn
    {
        //Enemies
        BunO,
        Minibot,
        Balguard
    }
    public ObjectsToSpawn SpawnThis;
    public float startTime;
    public float endTime; 
    float periodBetweenSpawn;
    float timer;
    ///
    public GameObject BunO;
    public GameObject Minibot;
    public GameObject Balguard;

    // Use this for initialization
    void Start () {
        periodBetweenSpawn = Random.Range(startTime, endTime);
        timer = periodBetweenSpawn; //set timer to 5 for example
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime; //
        if (timer <= Time.deltaTime - periodBetweenSpawn)
        {
            switch(SpawnThis)
            {
                case ObjectsToSpawn.BunO:
                 Instantiate(BunO, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                    break;
                case ObjectsToSpawn.Minibot:
                    Instantiate(Minibot, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                    break;
                case ObjectsToSpawn.Balguard:
                    Instantiate(Balguard, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                    break;
            }
            periodBetweenSpawn = Random.Range(startTime, endTime);
            timer = periodBetweenSpawn; //then reset timer to start over again
            Debug.Log(timer);
        }

	}
}
