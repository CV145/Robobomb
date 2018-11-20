using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Text : MonoBehaviour {

    //public Text textComponent;
    GameObject player;
    PickupsAndStats statsScript;

    public enum ChangingText
    {
        ThisIsFireText,
        ThisIsBombText,
        ThisIsScoreText,
        ThisIsCrystalText,
        ThisIsHighScoreText
    }
    public ChangingText thisText;

	// Use this for initialization
	void Start () {
        //textComponent = GetComponent<Text>();
        player = GameObject.Find("RoboPlayer");
        statsScript = player.GetComponent<PickupsAndStats>();
	}
	
	// Update is called once per frame
	void Update () {

        if (thisText == ChangingText.ThisIsHighScoreText)
        {
            GetComponent<UnityEngine.UI.Text>().text = statsScript.GetHighScoreText();
        }

		if (thisText == ChangingText.ThisIsBombText)
        {
            GetComponent<UnityEngine.UI.Text>().text = statsScript.GetBombText();
        }

        if (thisText == ChangingText.ThisIsFireText)
        {
            GetComponent<UnityEngine.UI.Text>().text = statsScript.GetFireText();
        }

        if (thisText == ChangingText.ThisIsScoreText)
        {
            GetComponent<UnityEngine.UI.Text>().text = statsScript.GetScoreText();
        }

        if (thisText == ChangingText.ThisIsCrystalText)
        {
            GetComponent<UnityEngine.UI.Text>().text = statsScript.GetCrystalText();
        }
    }
}
