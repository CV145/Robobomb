using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    bool gameStart = false;
    public GameObject playButton;
    public GameObject hud;
    public GameObject resultsHUD;
    public GameObject ThrowBtn, JumpBtn, RightBtn, LeftBtn;
    public Admanager instance;

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
        //instance.HideBanner();
    }

	// Use this for initialization
	void Start () {
        hud.SetActive(false);
        resultsHUD.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<PickupsAndStats>().Alive) //when Robo dies
        {
            //hud.SetActive(false);
            resultsHUD.SetActive(true);
            ThrowBtn.SetActive(false);
            JumpBtn.SetActive(false);
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
           // instance.ShowBanner();
        }
        if (GetComponent<PickupsAndStats>().Alive)
        {
            //instance.HideBanner();
        }
	}
}
