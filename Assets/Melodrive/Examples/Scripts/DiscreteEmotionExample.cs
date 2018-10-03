using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscreteEmotionExample : MonoBehaviour {

    public void OnEmotionChange()
    {
        MelodrivePlugin md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();
        Dropdown emotionChange = GameObject.Find("EmotionSelect").GetComponent<Dropdown>();
        string emotion = emotionChange.options[emotionChange.value].text.ToLower();
        md.SetEmotion(emotion);
    }
}
