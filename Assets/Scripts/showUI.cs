using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showUI : MonoBehaviour {

    GameControl gameControl;

	// Use this for initialization
	void Start () {
        gameControl = GameObject.Find("RoboPlayer").GetComponent<GameControl>();
        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (gameControl.GameStart)
        {
            this.gameObject.SetActive(true);
        }

        if (!gameControl.GameStart)
        {
            this.gameObject.SetActive(false);
        }
	}
}
