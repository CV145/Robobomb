using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleChangeExample : MonoBehaviour {

    MelodrivePlugin md;

    private void Start()
    {
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();
        md.Init("ambient", "happy");

        // When changing styles, we reccomend they are preloaded first to avoid any audio jitters
        md.PreloadStyles();
        md.Play();
    }

    public void OnStyleChange()
    {
        Dropdown styleChange = GameObject.Find("StyleSelect").GetComponent<Dropdown>();
        string style = styleChange.options[styleChange.value].text.ToLower();
        md.SetStyle(style);
    }
}
