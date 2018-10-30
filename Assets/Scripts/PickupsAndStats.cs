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
    GameObject BeginPosition;

    // Use this for initialization
    void Start () {
        BeginPosition = GameObject.Find("Start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetBombLV()
    {
        return bombLV;
    }

    public int GetFireLV()
    {
        return fireLV;
    }

    public int GetBombsOnScreen()
    {
        return bombsOnScreen;
    }

    ///RETURN STRINGS FOR HUD ///
    ///

        public string GetFireText()
    {
        return fireLV.ToString();
    }

    public string GetBombText()
    {
        return bombLV.ToString();
    }


    /// ////////////
    /// 
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 15 || collision.gameObject.layer == 13)
        {
            transform.position = BeginPosition.transform.position;
        }
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
        if (bombsOnScreen <= 0)
        {
            bombsOnScreen = 0;
        }
    }

    public void FireLVUp()
    {
        if (fireLV < 5)
        {
            fireLV++;
        }
        Debug.Log(fireLV);
    }

    public void BombLVUp()
    {
        if (bombLV < 5)
        {
            bombLV++;
        }
    }

}
