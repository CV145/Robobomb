﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public enum ObjectsToSpawn
    {
        //Enemies
        BunO,
        Minibot,
        Balguard,
        UFO,
        KingBoss,
        //Items
        Bombup,
        Fireup,
        Crystal
    }
    public ObjectsToSpawn SpawnThis;
    public float startTime;
    public float endTime;
    public bool endless;
    public int spawnThisManyTimes;
    float periodBetweenSpawn;
    float timer;
    ///
    public GameObject BunO;
    public GameObject Minibot;
    public GameObject Balguard;
    public GameObject UFO;
    ///
    public GameObject KingBoss;
    ///
    public GameObject Fireup;
    public GameObject Bombup;
    public GameObject Crystal;
    PickupsAndStats stats;
    GameControl control;


    int spawnedCount = 0;
    ///
    public int triggerGoal;//the # of enemies required to kill to activate spawns

    // Use this for initialization
    void Start () {
        periodBetweenSpawn = Random.Range(startTime, endTime);
        timer = periodBetweenSpawn; //set timer to 5 for example
        stats = GameObject.Find("RoboPlayer").GetComponent<PickupsAndStats>();
        control = GameObject.Find("RoboPlayer").GetComponent<GameControl>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (control.GameStart == true && stats.Alive) //if game is started, and player is alive, begin spawning
        {
            if (stats.Kills >= triggerGoal)
            {
                timer -= Time.deltaTime; //
                if (timer <= Time.deltaTime - periodBetweenSpawn)
                {
                    switch (SpawnThis)
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
                        case ObjectsToSpawn.UFO:
                            Instantiate(UFO, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                            break;
                        ///
                        case ObjectsToSpawn.KingBoss:
                            Instantiate(KingBoss, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                            break;
                        ///
                        case ObjectsToSpawn.Fireup:
                            Instantiate(Fireup, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                            break;
                        case ObjectsToSpawn.Bombup:
                            Instantiate(Bombup, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                            break;
                        case ObjectsToSpawn.Crystal:
                            Instantiate(Crystal, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
                            break;
                    }
                    spawnedCount++;
                    periodBetweenSpawn = Random.Range(startTime, endTime);
                    timer = periodBetweenSpawn; //then reset timer to start over again 
                }
                if (!endless && spawnedCount >= spawnThisManyTimes)
                {
                    Destroy(gameObject);
                }
            }
        }
	}
}
