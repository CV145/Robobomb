using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {
    //script with functions that activate and deactivate the button script and images for its planet
    bool planetActivated;

    public void Start()
    {
        planetActivated = false;
    }

    public void LoadByIndex(int sceneIndex)
    {
        if (planetActivated == true)
        {
            SceneManager.LoadScene(sceneIndex);
            //load scene inputted into function
            //This script is dragged into the On Click
            //inside the Button Script
        }
    }

    public void ActivatePlanet()
    {
        planetActivated = true;
    }

}
