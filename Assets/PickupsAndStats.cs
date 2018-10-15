using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsAndStats : MonoBehaviour {

    public LayerMask Bombups;
    public LayerMask Fireups;
    public LayerMask Health;

    public int fireLV = 1;
    public int bombLV = 1;
    public int currentHealth = 4;
    public int maxHealth = 4;

    int bombsOnScreen = 0;
    public PlayerController player; //setup in inspector

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetBombLV()
    {
        return bombLV;
    }

    public int GetBombsOnScreen()
    {
        return bombsOnScreen;
    }

    public void AnotherBombOnScreen()
    {
        //called at a bomb script's Start()
        bombsOnScreen++;
    }

    public void ABombLessOnScreen()
    {
        //called when a bomb Explode() happens
        bombsOnScreen--;
    }

    public void FireLVUp()
    {
        if (fireLV < 5)
        {
            fireLV++;
        }
    }

    public void BombLVUp()
    {
        if (bombLV < 5)
        {
            bombLV++;
        }
    }

}
