using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    bool gameStart = false;
    public GameObject playButton;
    public GameObject hud;

    public bool GameStart
    {
        get
        {
            return gameStart;
        }
        set
        {
            gameStart = value;
        }
    }


    public void Begin()
    {
        GameStart = true;
        hud.SetActive(true);
        playButton.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        hud.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
