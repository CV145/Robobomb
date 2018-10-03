using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteReactor : MonoBehaviour
{
    public string emotion;
    public string part;
    public int[] notes = { 42, 44, 46, 49 };
    public float thrust = 4.0f;

    private MelodrivePlugin md;
    private GameObject kicker;

    // Use this for initialization
    void Start ()
    {
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();
        md.NoteOn += new MelodrivePlugin.NoteOnHandler(NoteOn);

        kicker = GameObject.Find("Kicker");
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void DoImpulse(float vel)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        Vector3 direction = kicker.transform.position -transform.position;
        direction.Normalize();
        body.AddForce(direction * thrust, ForceMode.Impulse);
        body.AddForce(new Vector3(0, 1) * thrust * vel, ForceMode.Impulse);
    }

    private void NoteOn(string part, int num, int vel)
    {
        if (transform.position.y > 0.5)
            return;

        if (this.part == part || this.part+"chip" == part)
        {
            if (Array.IndexOf(notes, num) > -1)
            {
                string emotion = md.GetEmotion();
                if (this.emotion == emotion)
                    DoImpulse(((float)vel) / 127);
            }
        }
    }
}
