using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalEmotionExample : MonoBehaviour
{
    private GameObject listener = null;
    private GameObject target = null;
    private float speed = 1.0f;
    private MelodrivePlugin md;
    private GameObject[] points;
    private Dictionary<string, int> emotionIDs;
    
    void Start ()
    {
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();

        // First, set up the emotional points in Melodrive
        emotionIDs = new Dictionary<string, int>();
        points = new GameObject[]{ GameObject.Find("Happy"), GameObject.Find("Tender"), GameObject.Find("Sad"), GameObject.Find("Angry") };
        foreach (GameObject p in points)
        {
            Vector3 pos = p.transform.position;
            // Emotional points are 2-dimensional, so here we are using x and z.
            int emotionID = md.AddEmotionalPoint(pos.x, pos.z, p.name.ToLower());
            // Melodrive keeps its own emotion point IDs, so we need to link it to the GameObject
            emotionIDs[p.name] = emotionID;
        }

        // Setting the initial listener position
        listener = GameObject.Find("Listener");
        md.SetListenerPosition(listener.transform.position.x, listener.transform.position.z);

        // Choose a new random target
        target = points[(int)(Random.value * points.Length)];
    }
	
	void Update () {
        // Rotate around the parent...
        GameObject.Find("EmotionalPoints").transform.Rotate(new Vector3(0, 0.1f, 0));
        // and update each emotion point position in Melodrive
        foreach (GameObject p in points)
        {
            Vector3 pos = p.transform.position;
            // Use the map we made earlier
            int emotionID = emotionIDs[p.name];
            // Again, only using x and z dimentions
            md.SetEmotionalPointPosition(emotionID, pos.x, pos.z);
        }

        if (listener.transform.position == target.transform.position)
        {
            // Choose a new target if we got there
            target = points[(int)(Random.value * points.Length)];
        }
        else
        {
            // Or move towards our target
            float step = speed * Time.deltaTime;
            listener.transform.position = Vector3.MoveTowards(listener.transform.position, target.transform.position, step);
        }

        // Update the listener position
        md.SetListenerPosition(listener.transform.position.x, listener.transform.position.z);
    }
}
