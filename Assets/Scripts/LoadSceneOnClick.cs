using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {
    //script with functions that activate and deactivate the button script and images for its planet
    public bool planetActivated;
    public GameObject PurchaseUI;
    public LoadSceneOnClick previousPlanet;

    public void Start()
    {
        PurchaseUI.SetActive(true);
    }

    public void FixedUpdate()
    {
        if (previousPlanet.planetActivated == false)
        {
            PurchaseUI.SetActive(false);
        }
        else
        {
            PurchaseUI.SetActive(true);
        }
    }

    public void LoadByIndex(int sceneIndex)
    {
        if (planetActivated == true)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void ActivatePlanet()
    {
        planetActivated = true;
        PurchaseUI.SetActive(false);
    }

}
