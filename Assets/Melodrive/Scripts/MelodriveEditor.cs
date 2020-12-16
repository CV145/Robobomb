using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MelodriveEditor : MonoBehaviour
{
    public string style = "house";
    public string emotion = "tender";

    private GameObject listener = null;
    private GameObject target = null;
    private float speed = 1.0f;
    private MelodrivePlugin md;
    private bool init = false;
    private static bool updateMusicalSeeds = false;
    private static bool updateEnsembles = false;
    private static bool updateScale = false;
    private Vector3 targetScale;
    private Vector3 targetScale2;
    private string lastSavePath;
    private bool autoPlay = false;

    // Use this for initialization
    void Start()
    {
        lastSavePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Melodrive");
        if (!Directory.Exists(lastSavePath))
            Directory.CreateDirectory(lastSavePath);

        // Setup Melodrive....
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();
        md.Beat += new MelodrivePlugin.BeatHandler(Beat);
        md.NewMusicalSeed += new MelodrivePlugin.NewMusicalSeedHandler(NewMusicalSeed);
        md.CueChange += new MelodrivePlugin.CueChangeHandler(CueChange);
        md.ProjectLoad += new MelodrivePlugin.ProjectLoadHandler(ProjectLoad);


        // init style dropdown
        Dropdown styleChange = GameObject.Find("StyleSelect").GetComponent<Dropdown>();
        styleChange.ClearOptions();
        string[] styles = md.GetStyles();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        int selectedValue = -1;
        for (int i = 0; i < styles.Length; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = styles[i];
            options.Add(item);
            if (style == styles[i])
                selectedValue = i;
        }
        styleChange.AddOptions(options);

        if (selectedValue > -1)
            styleChange.value = selectedValue;

        md.Init(style, emotion);
        md.PreloadStyles();

        Vector3 initPos = new Vector3();
        GameObject[] points = { GameObject.Find("Happy"), GameObject.Find("Tender"), GameObject.Find("Sad"), GameObject.Find("Angry") };
        for (int i = 0; i < points.Length; i++)
        {
            GameObject p = points[i];
            Vector3 pos = p.transform.position;
            if (p.name.ToLower() == emotion)
            {
                target = p;
                initPos = pos;
            }
            md.AddEmotionalPoint(pos.x, pos.z, p.name.ToLower());
        }

        listener = GameObject.Find("Listener");
        listener.transform.position = initPos;
        targetScale = listener.transform.localScale;
        targetScale2 = new Vector3(1, 1, 1);
        md.SetListenerPosition(listener.transform.position.x, listener.transform.position.z);
        init = true;

        //updateMusicalSeeds = true;
        //updateEnsembles = true;

        OnUIAutoPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject oldTarget = target;
                // the object identified by hit.transform was clicked
                target = hit.transform.gameObject;
                if (oldTarget && oldTarget != target)
                    oldTarget.transform.localScale = targetScale2;
            }
        }

        if (target)
        {
            float step = speed * Time.deltaTime;
            listener.transform.position = Vector3.MoveTowards(listener.transform.position, target.transform.position, step);
        }

        if (updateScale)
        {
            listener.transform.localScale = new Vector3(1, 1, 1);
            if (target)
                target.transform.localScale = new Vector3(2, 2, 2);
            updateScale = false;
        }
        else
        {
            listener.transform.localScale = Vector3.Lerp(listener.transform.localScale, targetScale, 2 * Time.deltaTime);
            if (target)
                target.transform.localScale = Vector3.Lerp(target.transform.localScale, targetScale2, 2 * Time.deltaTime);
        }


        // update listener position
        if (md)
            md.SetListenerPosition(listener.transform.position.x, listener.transform.position.z);

        if (updateMusicalSeeds)
        {
            UpdateMusicalSeedList();
            updateMusicalSeeds = false;
        }

        if (updateEnsembles)
        {
            UpdateEnsembleList();
            updateEnsembles = false;
        }
    }

    public void OnUIProjectMenu()
    {
        Dropdown menu = GameObject.Find("ProjectMenu").GetComponent<Dropdown>();
        int selectedItem = menu.value;

        if (md)
        {
            switch (selectedItem)
            {
                case 0:
                    break;
                case 1:
                    OpenProject();
                    break;
                case 2:
                    SaveProject();
                    break;
            }
        }

        menu.value = 0;
    }

    public void OnUIMusicalSeedMenu()
    {
        Dropdown menu = GameObject.Find("MusicalSeedMenu").GetComponent<Dropdown>();
        int selectedItem = menu.value;

        if (md)
        {
            switch (selectedItem)
            {
                case 0:
                    break;
                case 1:
                    NewMusicalSeed();
                    break;
                case 2:
                    LoadMusicalSeed();
                    break;
                case 3:
                    SaveMusicalSeed();
                    break;
            }
        }

        menu.value = 0;
    }

    public void UpdateMusicalSeedList()
    {
        Dropdown musicalSeedChange = GameObject.Find("MusicalSeedSelect").GetComponent<Dropdown>();
        musicalSeedChange.ClearOptions();
        string[] seeds = md.GetMusicalSeeds();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < seeds.Length; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = seeds[i];
            options.Add(item);
        }
        musicalSeedChange.AddOptions(options);
    }

    public void UpdateEnsembleList()
    {
        Dropdown ensembleChange = GameObject.Find("EnsembleSelect").GetComponent<Dropdown>();
        ensembleChange.ClearOptions();
        string[] ensembles = md.GetEnsembles();
        string currentEnsemble = md.GetCurrentEnsemble();
        int value = 0;
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < ensembles.Length; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = ensembles[i];
            options.Add(item);
            if (ensembles[i] == currentEnsemble)
                value = i;
        }
        ensembleChange.AddOptions(options);
        ensembleChange.value = value;

        Toggle chiptuneToggle = GameObject.Find("ChiptuneMode").GetComponent<Toggle>();
        chiptuneToggle.isOn = md.GetChiptuneMode();
    }

    public void OnUIStyleChange()
    {
        if (!init)
            return;

        Dropdown styleChange = GameObject.Find("StyleSelect").GetComponent<Dropdown>();
        if (md)
        {
            string[] styles = md.GetStyles();
            Debug.Log("Style changed: " + styles[styleChange.value]);
            md.SetStyle(styles[styleChange.value]);
        }
    }

    public void OnUIMusicalSeedChange()
    {
        if (!init)
            return;

        Dropdown musicalSeedChange = GameObject.Find("MusicalSeedSelect").GetComponent<Dropdown>();
        if (md)
        {
            string[] seeds = md.GetMusicalSeeds();
            Debug.Log("Musical Seed changed: " + seeds[musicalSeedChange.value]);
            md.SetMusicalSeed(seeds[musicalSeedChange.value]);
        }
    }

    public void OnUIEnsembleChange()
    {
        Dropdown ensembleChange = GameObject.Find("EnsembleSelect").GetComponent<Dropdown>();
        if (md)
        {
            string[] ensembles = md.GetEnsembles();
            Debug.Log("Ensemble changed: " + ensembles[ensembleChange.value]);
            md.SetEnsemble(ensembles[ensembleChange.value]);
        }
    }

    public void OnUIChiptuneModeToggle()
    {
        Toggle chiptuneToggle = GameObject.Find("ChiptuneMode").GetComponent<Toggle>();
        if (md)
            md.SetChiptuneMode(chiptuneToggle.isOn);
    }

    public void OnUIPlay()
    {
        if (md)
            md.Play();
    }

    public void OnUIAutoPlay()
    {
        if (md)
        {
            GameObject autoPlayToggleGO = GameObject.Find("AutoPlayToggle");
            Toggle autoPlayToggle = autoPlayToggleGO.GetComponent<Toggle>();
            autoPlay = autoPlayToggle.isOn;
            if (autoPlay)
                md.Play();
        }
    }

    public void OnUIStop()
    {
        if (md)
            md.Stop();
    }

    public void OnUITempoChange()
    {
        if (md)
        {
            Slider tempoSlider = GameObject.Find("TempoSlider").GetComponent<Slider>();
            md.SetTempoScale(tempoSlider.value);
        }
    }

    public void OnUIVolumeChange()
    {
        if (md)
        {
            Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
            md.SetMasterGain(volumeSlider.value);
        }
    }

    private void NewMusicalSeed()
    {
        Debug.Log("New Musical Seed clicked!");
        md.CreateMusicalSeed();
    }

    private void LoadMusicalSeed()
    {
#if UNITY_EDITOR
        if (md)
        {
            //Get a filename
            string filename = EditorUtility.OpenFilePanel("Choose a Musical Seed to load",
                                                         lastSavePath, "mdseed");

            if (filename != "" && File.Exists(filename))
            {
                md.LoadMusicalSeed(filename);
                lastSavePath = Path.GetDirectoryName(filename);
            }
        }
#endif
    }

    private void SaveMusicalSeed()
    {
#if UNITY_EDITOR
        if (md)
        {
            Dropdown seedChange = GameObject.Find("MusicalSeedSelect").GetComponent<Dropdown>();
            string selectedSeed = seedChange.options[seedChange.value].text;

            //Get a filename
            string filename = EditorUtility.SaveFilePanel("Choose a location",
                                                         lastSavePath, selectedSeed, "mdseed");

            if (filename != "")
            {
                md.SaveMusicalSeed(selectedSeed, filename);
                lastSavePath = Path.GetDirectoryName(filename);
            }
        }
#endif
    }

    private void OpenProject()
    {
#if UNITY_EDITOR
        if (md)
        {
            //Get a filename
            string filename = EditorUtility.OpenFilePanel("Choose file", lastSavePath, "mdproj");
            if (filename != "" && File.Exists(filename))
            {
                md.LoadProject(filename);
                lastSavePath = Path.GetDirectoryName(filename);
            }
            if (autoPlay)
                md.Play();
        }
#endif
    }

    private void SaveProject()
    {
#if UNITY_EDITOR
        //Get a filename
        string filename = EditorUtility.SaveFilePanel("Choose location", lastSavePath, Application.productName, "mdproj");
        if (filename != "")
        {
            md.SaveProject(filename);
            lastSavePath = Path.GetDirectoryName(filename);
        }
#endif
    }

    private void Beat(float beat, float bar)
    {
        updateScale = true;
    }

    private void NewMusicalSeed(string name)
    {
        Debug.Log("Melodrive: NewMusicalSeed " + name);
        updateMusicalSeeds = true;
    }

    private void ProjectLoad()
    {
        Debug.Log("Melodrive: ProjectLoad");
        updateMusicalSeeds = true;
        updateEnsembles = true;
    }

    private void CueChange(string cue, string seedName, string style)
    {
        Dropdown styleChange = GameObject.Find("StyleSelect").GetComponent<Dropdown>();
        string[] styles = md.GetStyles();
        for (int i = 0; i < styles.Length; i++)
        {
            if (styles[i] == style)
            {
                styleChange.value = i;
                break;
            }
        }

        Dropdown seedChange = GameObject.Find("MusicalSeedSelect").GetComponent<Dropdown>();
        string[] seeds = md.GetMusicalSeeds();
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i] == seedName)
            {
                seedChange.value = i;
                break;
            }
        }

        updateEnsembles = true;
    }
}