using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedChangeExample : MonoBehaviour
{
    private MelodrivePlugin md;
    private Dropdown seedSelect;

    private void Start()
    {
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();

        // Musical Seeds are created asynchronously - this means we need to listen to callbacks
        // to find out when they are ready to be queried.
        md.NewMusicalSeed += new MelodrivePlugin.NewMusicalSeedHandler(NewMusicalSeed);
        md.CueChange += new MelodrivePlugin.CueChangeHandler(CueChange);

        seedSelect = GameObject.Find("SeedSelect").GetComponent<Dropdown>();
    }

    public void OnNewSeed()
    {
        md.CreateMusicalSeed();
    }

    public void OnSeedChange()
    {
        string seed = seedSelect.options[seedSelect.value].text;
        md.SetMusicalSeed(seed);
    }

    public void UpdateSeedSelect()
    {
        seedSelect.ClearOptions();
        string[] seeds = md.GetMusicalSeeds();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < seeds.Length; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = seeds[i];
            options.Add(item);
        }
        seedSelect.AddOptions(options);
    }

    private void NewMusicalSeed(string name)
    {
        UpdateSeedSelect();
    }

    private void CueChange(string cue, string seedName, string style)
    {
        string[] seeds = md.GetMusicalSeeds();
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i] == seedName)
            {
                seedSelect.value = i;
                break;
            }
        }
    }
}
