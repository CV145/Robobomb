using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PickupsAndStats : MonoBehaviour {

    public LayerMask Bombups;
    public LayerMask Fireups;

    public int fireLV = 1;
    public int bombLV = 1;
    public int Crystals = 0;
    public int killCount;
    private int highScore = 0;
    bool isAlive = true;
    int bombsOnScreen = 0;
    public PlayerController player; //setup in inspector
    public GameControl control;
    GameObject BeginPosition;
    //
    public AudioClip score;
    public AudioSource scoreSource;
    public AudioClip hit;
    public AudioSource hitSource;
    public AudioClip powerup;
    public AudioSource powerupSource;
    public AudioSource crystalSource;
    public AudioClip crystalGet;

    //
    public CanvasGroup myCG;
    private bool flash = false;

    //Properties
    public int CrystalsProperty
    {
        get
        {
            return Crystals;
        }
        set
        {
            Crystals = value;
        }
    }

    public int HighScoreProperty
    {
        get
        {
            return highScore;
        }
        set
        {
            highScore = value;
        }
    }

    public int ScoreProperty
    {
        get
        {
            return killCount;
        }
        set
        {
            killCount = value;
        }
    }

    // Use this for initialization
    void Start() {
        BeginPosition = GameObject.Find("Start");
        scoreSource.clip = score;
        hitSource.clip = hit;
        powerupSource.clip = powerup;
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
        hitSource.Play();
        GetComponent<Animator>().SetBool("Dead", true);
        myCG.alpha = 1;
        GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, -10);
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 5 ;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().mass = 0;
        GameObject.Destroy(GetComponent<PlayerController>());

        //check if there's a high score
        HighScoreCheck();

        //save the game here, storing crystals and high score
        control.SaveGame();
        //get the current high score and current crystals and save them
    }

    private void HighScoreCheck()
    {
        //function called after game ends or when game is saving to check for high scores

        if (ScoreProperty > HighScoreProperty)
        {
            HighScoreProperty = ScoreProperty;
        }
    }

    public int Kills
    {
        get
        {
            return killCount;
        }
        set
        {
            killCount = value;
            scoreSource.Play();
        }
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

    public string GetScoreText()
    {
        return Kills.ToString();
    }

    public string GetHighScoreText()
    {
        return highScore.ToString();
    }

    public string GetCrystalText()
    {
        return Crystals.ToString();
    }

    //property for isAlive bool
    public bool Alive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
        }
    }


    /// ////////////
    /// 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            if (isAlive)
            {
                MineHit();
                isAlive = false;
            }
        }
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(0.5f); //WaitForSeconds seems to be a class type
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            powerupSource.Play();
        }
    }

    public void CrystalIncrease()
    {
        Crystals++;
        crystalSource.Play();

    }

    public void BombLVUp()
    {
        if (bombLV < 5)
        {
            bombLV++;
            powerupSource.Play();
        }
    }

}
