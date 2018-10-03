using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestMelodrive
{
	private int longMaxFrames = 1000 * 3;
    private int shortMaxFrames = 100 * 5;
    private MDBehaviourTest tester;
    private MelodrivePlugin md;

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    [Timeout(100000000)]
    public IEnumerator TestMelodrivePrefab() {
        // Use the Assert class to test conditions.
        // yield to skip a frame

        // Test Melodrive Prefab can be instansiated
        GameObject mdPrefab = Object.Instantiate(Resources.Load("Melodrive")) as GameObject;
        Assert.NotNull(mdPrefab, "Unable to Instantiate Melodrive Prefab");

        mdPrefab.AddComponent<AudioListener>();
        mdPrefab.AddComponent<MDBehaviourTest>();

        //Test component creation
        md = mdPrefab.GetComponent<MelodrivePlugin>();
        Assert.NotNull(md, "Unable to find MelodrivePlugin component");

        tester = mdPrefab.GetComponent<MDBehaviourTest>();
        Assert.NotNull(tester, "Unable to find MDBehaviourTest component");

        yield return TestPreInit();
        Debug.Log("TestPreInit passed!");

        tester.MDInit();

        //Run melodrive until we get the first Cue
        tester.cueChanged = false;
        tester.frameCount = 0;
        while (tester.frameCount < shortMaxFrames && ! tester.cueChanged)
            yield return null;

        yield return TestTransport();
        Debug.Log("TestTransport passed!");

        md.Play();

        yield return TestStyles();
        Debug.Log("TestStyles passed!");

        yield return TestMusicalSeeds();
        Debug.Log("TestMusicalSeeds passed!");

        yield return TestEnsembles();
        Debug.Log("TestEnsembles passed!");
    }

    private IEnumerator TestPreInit()
    {
        yield return null;

        md.Stop();

        yield return null;

        LogAssert.Expect(LogType.Log, "Melodrive not initialised!");
    }

    private IEnumerator TestTransport()
    {
        tester.rms = 0;
        tester.peakRMS = 0;
        md.Play();
        while (tester.frameCount < shortMaxFrames)
            yield return null;
        Assert.Greater(tester.peakRMS, 0, "Play() should make some sound happen!");

        tester.rms = 0;
        tester.peakRMS = 0;
        md.Stop();
        while (tester.frameCount < longMaxFrames && tester.rms > 0.01)
            yield return null;
        Assert.Less(tester.rms, 0.01, "Stop() should stop making sound!");
    }

    private IEnumerator TestStyles()
    {
        string style = md.GetStyle();

        // Test style change
        tester.cueChanged = false;
        tester.frameCount = 0;
        md.SetStyle("ambient");
        while (tester.frameCount < longMaxFrames && !tester.cueChanged)
            yield return null;

        Assert.AreNotEqual(md.GetStyle(), style, "SetStyle should actually set the style!");
    }

    private IEnumerator TestMusicalSeeds()
    {
        //Test at least one musical seed was made
        string[] seeds = md.GetMusicalSeeds();
        Assert.Greater(seeds.Length, 0, "Melodrive should initialise at least one Musical Seed");

        //Test Musical Seed creation
        tester.newSeed = false;
        tester.frameCount = 0;
        md.CreateMusicalSeed();
        while (tester.frameCount < shortMaxFrames && !tester.newSeed)
            yield return null;

        string[] seeds2 = md.GetMusicalSeeds();
        Assert.Greater(seeds2.Length, seeds.Length, "CreateMusicalSeed didn't create a Musical Seed");

        //Test Musical Seed change
        tester.cueChanged = false;
        tester.frameCount = 0;
        md.SetMusicalSeed(seeds2[seeds2.Length - 1]);
        while (tester.frameCount < longMaxFrames && !tester.cueChanged)
            yield return null;

        string seedName = md.GetMusicalSeed();
        Assert.AreEqual(seedName, seeds2[seeds2.Length - 1]);
    }
    private IEnumerator TestEnsembles()
    {
        //Test at least two ensembles exist
        string[] ensembles = md.GetEnsembles();
        Assert.Greater(ensembles.Length, 1, "There should be at least two Ensembles");

        //Test Ensemble change
        string ensemble = md.GetCurrentEnsemble();
        string oldEnsemble = ensemble;

        int i = Random.Range(0, ensembles.Length - 1);
        while (ensembles[i] == ensemble)
            i = Random.Range(0, ensembles.Length - 1);
        ensemble = ensembles[i];

        tester.cueChanged = false;
        tester.frameCount = 0;
        md.SetEnsemble(ensemble);
        while (tester.frameCount < longMaxFrames && !tester.cueChanged)
            yield return null;

        Assert.AreEqual(md.GetCurrentEnsemble(), ensemble);
        Assert.AreNotEqual(md.GetCurrentEnsemble(), oldEnsemble);
    }

    public class MDBehaviourTest : MonoBehaviour
    {
        public string initStyle = "house";
        public string initEmotion = "neutral";

        public int frameCount = 0;
        public bool cueChanged = false;
        public bool newSeed = false;
        public float rms = 0;
        public float peakRMS = 0;

        private MelodrivePlugin md;

        void Start()
        {
            md = GameObject.Find("Melodrive(Clone)").GetComponent<MelodrivePlugin>();
            md.CueChange += new MelodrivePlugin.CueChangeHandler(OnCueChange);
            md.NewMusicalSeed += new MelodrivePlugin.NewMusicalSeedHandler(OnNewMusicalSeed);
        }

        public void MDInit()
        {
            md.Init(initStyle, initEmotion);
        }

        void Update()
        {
            frameCount++;

            float[] bothRMS = md.GetRMS();
            rms = (bothRMS[0] + bothRMS[1]) * 0.5f;
            if (rms > peakRMS)
                peakRMS = rms;
        }

        void OnCueChange(string cue, string theme, string style)
        {
            cueChanged = true;
        }

        void OnNewMusicalSeed(string seedName)
        {
            newSeed = true;
        }
    }
}
