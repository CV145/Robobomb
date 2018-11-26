using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShop : MonoBehaviour {

    // public List planets = new List();
    public GameObject planet1, planet2, planet3, planet4, planet5;
    
    public bool planet1Purchased, planet2Purchased, planet3Purchased, planet4Purchased, planet5Purchased;

    // Use this for initialization
    void Start () {
        planet1Purchased = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (planet1Purchased)
        {
            planet1.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
        if (planet2Purchased)
        {
            planet2.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
        if (planet3Purchased)
        {
            planet3.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
        if (planet4Purchased)
        {
            planet4.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
        if (planet5Purchased)
        {
            planet5.GetComponent<LoadSceneOnClick>().ActivatePlanet();
        }
    }
}
