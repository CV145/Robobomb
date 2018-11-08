using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public CanvasGroup myCG;
    private bool flash = false;

    // Use this for initialization
    void Start () {
        BeginPosition = GameObject.Find("Start");
	}
	
	// Update is called once per frame
	void Update () {
        if (flash)
        {
            myCG.alpha = myCG.alpha - Time.deltaTime;
            if (myCG.alpha <= 0)
            {
                myCG.alpha = 0;
                flash = false;
            }
        }
    }
    

    public void MineHit()
    {
        flash = true;
        myCG.alpha = 1;
        GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, -10);
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 5 ;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
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
            MineHit();
            StartCoroutine(LoadYourAsyncScene());
            //Destroy(this.gameObject);
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        yield return new WaitForSeconds(0.5f); //WaitForSeconds seems to be a class type


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //// Wait until the asynchronous scene fully loads
        //while (!asyncLoad.isDone)
        //{
        //    yield return null;
        //}
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
