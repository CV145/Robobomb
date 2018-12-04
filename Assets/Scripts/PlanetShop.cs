using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShop : MonoBehaviour {

    // public List planets = new List();
    public GameObject planet1, planet2, planet3, planet4, planet5;
    public bool planet1Purchased, planet2Purchased, planet3Purchased, planet4Purchased, planet5Purchased;
    public PickupsAndStats stats;
    public GameControl control;

    // Use this for initialization
    void Start () {
        planet1Purchased = true;
	}

    public int Count { get; private set; }

    public void BuyPlanet()
    {
        //when buy button is clicked purchase its corresponding planet
        if (planet1Purchased && !planet2Purchased) //buying planet 2
        {
            if (stats.Crystals >= 100)
            {
                planet2Purchased = true;
                stats.Crystals -= 100;
                control.SaveGame();
                
                return;
            }
        }
        if (planet2Purchased && !planet3Purchased) //buying planet 3
        {
            if (stats.Crystals >= 300)
            {
                planet3Purchased = true;
                stats.Crystals -= 300;
                control.SaveGame();
                return;
            }
        }
        if (planet3Purchased && !planet4Purchased) //buying planet 4
        {
            if (stats.Crystals >= 500)
            {
                planet4Purchased = true;
                stats.Crystals -= 500;
                control.SaveGame();
                return;
            }
        }
        if (planet4Purchased && !planet5Purchased) //buying planet 5
        {
            if (stats.Crystals >= 800)
            {
                planet5Purchased = true;
                stats.Crystals -= 800;
                control.SaveGame();
                return;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		if (planet1Purchased)
        {
            planet1.GetComponent<LoadSceneOnClick>().ActivatePlanet();
            Count = 2;
        }
        if (planet2Purchased)
        {
            planet2.GetComponent<LoadSceneOnClick>().ActivatePlanet();
            Count = 3;
        }
        if (planet3Purchased)
        {
            planet3.GetComponent<LoadSceneOnClick>().ActivatePlanet();
            Count = 4;
        }
        if (planet4Purchased)
        {
            planet4.GetComponent<LoadSceneOnClick>().ActivatePlanet();
            Count = 5;
        }
        if (planet5Purchased)
        {
            planet5.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
    }
}
