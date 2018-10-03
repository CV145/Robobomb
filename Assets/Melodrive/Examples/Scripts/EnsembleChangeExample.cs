using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnsembleChangeExample : MonoBehaviour
{
    private MelodrivePlugin md;
    private Dropdown ensembleSelect;

    // Use this for initialization
    void Start ()
    {
        md = GameObject.Find("Melodrive").GetComponent<MelodrivePlugin>();

        // Ensembles are dependent on Style, so we need to listen for when that changes
        md.CueChange += new MelodrivePlugin.CueChangeHandler(CueChange);

        ensembleSelect = GameObject.Find("EnsembleSelect").GetComponent<Dropdown>();
    }

    public void OnEnsembleChange()
    {
        md.SetEnsemble(ensembleSelect.options[ensembleSelect.value].text);
    }

    public void UpdateEnsembleList()
    {
        string[] ensembles = md.GetEnsembles();
        string currentEnsemble = md.GetCurrentEnsemble();

        ensembleSelect.ClearOptions();
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
        ensembleSelect.AddOptions(options);
        ensembleSelect.value = value;
    }

    private void CueChange(string cue, string seedName, string style)
    {
        UpdateEnsembleList();
    }
}
