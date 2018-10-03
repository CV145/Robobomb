using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProjectExample : MonoBehaviour
{
    private MelodrivePlugin md;
    private string lastSavePath;

    // Use this for initialization
    void Start ()
    {
        lastSavePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Melodrive");

        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();
        md.ProjectLoad += new MelodrivePlugin.ProjectLoadHandler(ProjectLoad);
    }
    
    public void LoadMusicalSeed()
    {
#if UNITY_EDITOR
        //Get a filename
        string filename = EditorUtility.OpenFilePanel("Choose a Musical Seed to load",
                                                        lastSavePath, "mdseed");

        if (filename != "" && File.Exists(filename))
        {
            md.LoadMusicalSeed(filename);
            lastSavePath = Path.GetDirectoryName(filename);
        }
#endif
    }

    public void SaveMusicalSeed()
    {
#if UNITY_EDITOR
        Dropdown seedChange = GameObject.Find("SeedSelect").GetComponent<Dropdown>();
        string selectedSeed = seedChange.options[seedChange.value].text;

        //Get a filename
        string filename = EditorUtility.SaveFilePanel("Choose a location",
                                                        lastSavePath, selectedSeed, "mdseed");

        if (filename != "")
        {
            md.SaveMusicalSeed(selectedSeed, filename);
            lastSavePath = Path.GetDirectoryName(filename);
        }
#endif
    }

    public void LoadProject()
    {
#if UNITY_EDITOR
        //Get a filename
        string filename = EditorUtility.OpenFilePanel("Choose file", lastSavePath, "mdproj");
        if (filename != "" && File.Exists(filename))
        {
            md.LoadProject(filename);
            lastSavePath = Path.GetDirectoryName(filename);
        }
#endif
    }

    public void SaveProject()
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

    private void ProjectLoad()
    {
        GetComponent<SeedChangeExample>().UpdateSeedSelect();
    }
}
