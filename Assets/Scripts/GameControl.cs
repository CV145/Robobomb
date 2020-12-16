using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameControl : MonoBehaviour {

    bool gameStart = false;
    public GameObject MainMenu;
    public GameObject hud;
    public GameObject resultsHUD;
    public GameObject ThrowBtn, JumpBtn, RightBtn, LeftBtn;
    public PickupsAndStats stats;
    public Admanager instance;
    public AudioClip BGmusic;
    public AudioClip BGloop;
    public AudioSource BGloopsource;
    public AudioSource BGmusicSource;
    bool secondLoopStarted = false;

    public bool GameStart
    {
        get
        {
            return gameStart;
        }
        set
        {
            gameStart = value;
        }
    }


    public void Begin()
    {
        GameStart = true;
       hud.SetActive(true);
        MainMenu.SetActive(false);
        ThrowBtn.SetActive(true);
        JumpBtn.SetActive(true);
        LeftBtn.SetActive(true);
        RightBtn.SetActive(true);
        //BGmusicSource.Play();
        //instance.HideBanner();
    }

	// Use this for initialization
	void Start () {
        BGmusicSource.clip = BGmusic; //sets up the music
        BGloopsource.clip = BGloop; //BGloop is what loops after the intro is done
        hud.SetActive(true);
        ThrowBtn.SetActive(false);
        JumpBtn.SetActive(false);
        LeftBtn.SetActive(false);
        RightBtn.SetActive(false);
        resultsHUD.SetActive(false);
        //load the game stats when starting scene over
        LoadGame();
    }
	
	// Update is called once per frame
	void Update () {

        if (gameStart && !secondLoopStarted)
        {
            //When the game has started, if the intro is done playing loop the seocnd part forever
            if (!BGmusicSource.isPlaying)
            {
                BGloopsource.Play();
                secondLoopStarted = true;
            }
        }

		if (!GetComponent<PickupsAndStats>().Alive) //when Robo dies
        {
            //hud.SetActive(false);
            resultsHUD.SetActive(true);
            ThrowBtn.SetActive(false);
            JumpBtn.SetActive(false);
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
            // instance.ShowBanner();

            //Fade the music out
            StartCoroutine(FadeOut(BGmusicSource, 3.5f));
            StartCoroutine(FadeOut(BGloopsource, 3.5f));
        }
        if (GetComponent<PickupsAndStats>().Alive)
        {
            //instance.HideBanner();
        }
	}

    //Enum for fading out music
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        float adjustedVolume = startVolume;

        while (audioSource.volume > 0)
        {
            adjustedVolume -= startVolume * Time.deltaTime / FadeTime;
            audioSource.volume = adjustedVolume;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }


    //Saving and loading functions

    private Save CreateSaveGameObject()
    {
        Save save = new Save(); //create Save instance
        save.highScore = stats.HighScoreProperty;
        save.crystals = stats.CrystalsProperty;

        return save; //return an instance of save with above variables stored
    }

    public void SaveGame()
    {
        //this function is used to save the game
        //function should be called whenever player loses
        Save save = CreateSaveGameObject(); //get the save returned from function

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath +
        "/gamesave.save");
        bf.Serialize(saveFile, save);
        saveFile.Close();
        //creates a file that stores the contents of the save class from the 
        //function. These contents are serialized, or stored as binary
        Debug.Log("game saved");
    }

    public void LoadGame()
    {
        //function used to load the game
        //should be called whenever player starts the game (only called once)

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //do the following if there is a save file stored
            BinaryFormatter bf = new BinaryFormatter();
            FileStream saveFile = File.Open(Application.persistentDataPath +
            "/gamesave.save", FileMode.Open);
            //^ opens stored save file
            Save save = (Save)bf.Deserialize(saveFile);
            //take the contents out of the save file and "deserialize" it, change from
            //binary to actual code, and put it into an instance of a save class
            saveFile.Close();

            //now put the contents of what was saved into the actual game
            stats.CrystalsProperty = save.crystals;
            stats.HighScoreProperty = save.highScore;
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }


}
