/*
   __  ___    __        __    _
  /  |/  /__ / /__  ___/ /___(_)  _____
 / /|_/ / -_) / _ \/ _  / __/ / |/ / -_)
/_/  /_/\__/_/\___/\_,_/_/ /_/|___/\__/
*/

using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * This is the Melodrive controller class.
 * Other Game objects in Unity should control this class,
 * or listen to the events fired.
 */
public class MelodrivePlugin : MonoBehaviour
{
    public const string DEFAULT_STYLE = "piano";
    public const string DEFAULT_EMOTION = "neutral";

    public bool playOnStart = false;
    public string initStyle = DEFAULT_STYLE;
    public string initEmotion = DEFAULT_EMOTION;
    public bool chiptuneMode = false;

    private const string DLL_NAME = "AudioPlugin_Melodrive";
    private const string RELEASE_LITE = "lite";
    private const string RELEASE_INDIE = "indie";
    private string instrumentsPath = "";
    private Vector2 listener;
    private string[] styles = { "ambient", "house", "piano", "rock" };
    private int nextEmotionalPointID = 1;
    private string currentMusicalSeed;
    private string currentStyle;
    private string releaseMode = RELEASE_INDIE;
    private bool prevChiptuneMode = false;

    /*
	   ____              __
	  / __/  _____ ___  / /____
	 / _/| |/ / -_) _ \/ __(_-<
	/___/|___/\__/_//_/\__/___/

    */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugHandler(string message);
    public event DebugHandler DebugLog;

    public delegate void NoteOnHandler(string part, int num, int velocity);
    public event NoteOnHandler NoteOn;

    public delegate void NoteOffHandler(string part, int num);
    public event NoteOffHandler NoteOff;

    public delegate void ParamChangeHandler(string part, string param, float value);
    public event ParamChangeHandler ParamChange;

    public delegate void BarHandler(float bar);
    public event BarHandler Bar;

    public delegate void BeatHandler(float beat, float bar);
    public event BeatHandler Beat;

    public delegate void BeatTickHandler(int tick, float beat, float bar);
    public event BeatTickHandler BeatTick;

    public delegate void TempoChangeHandler(float bpm);
    public event TempoChangeHandler TempoChange;

    public delegate void NewMusicalSeedHandler(string name);
    public event NewMusicalSeedHandler NewMusicalSeed;

    public delegate void CueChangeHandler(string cue, string seedName, string style);
    public event CueChangeHandler CueChange;

    public delegate void ProjectLoadHandler();
    public event ProjectLoadHandler ProjectLoad;

    /*
       ____     _ __  _      ___          __  _
      /  _/__  (_) /_(_)__ _/ (_)__ ___ _/ /_(_)__  ___
     _/ // _ \/ / __/ / _ `/ / (_-</ _ `/ __/ / _ \/ _ \
    /___/_//_/_/\__/_/\_,_/_/_/___/\_,_/\__/_/\___/_//_/

    */



    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getReleaseMode();

    [DllImport(DLL_NAME)]
    private static extern void md_sendInfo([MarshalAs(UnmanagedType.LPStr)] string path);

    [DllImport(DLL_NAME)]
    private static extern void md_setInstrumentsPath([MarshalAs(UnmanagedType.LPStr)] string path);

    [DllImport(DLL_NAME)]
    private static extern void md_init([MarshalAs(UnmanagedType.LPStr)] string style, [MarshalAs(UnmanagedType.LPStr)] string initEmotion);

    /**
     * Sets the Melodrive AudioPlugin to load instruments from the StreamingAssets folder
     */
    public void SetInstrumentsPathToStreamingAssets()
    {
        string embededPath = System.IO.Path.Combine(Application.streamingAssetsPath, "MelodriveInstruments");
        if (Directory.Exists(embededPath))
            SetInstrumentsPath(embededPath);
    }

    /**
     * Sets the Melodrive AudioPlugin to load instruments from the specified folder
     */
    public void SetInstrumentsPath(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.Log("Melodrive: Can't find instruments directory: " + path);
            return;
        }

        instrumentsPath = path;
        md_setInstrumentsPath(path);
    }

    /**
     * Initialises Melodrive with a given style and emotion
     * @param string style - the style
     * @param string initEmotion - the initial emotion
     */
    public void Init(string style = DEFAULT_STYLE, string initEmotion = DEFAULT_EMOTION)
    {
        int idx = Array.IndexOf(styles, style);
        if (idx == -1)
        {
            Debug.LogError("Melodrive: Invalid style: \"" + style + "\".");
            return;
        }

        // Some simple Analytics
        #if (UNITY_EDITOR)
        string sessionID = EditorAnalyticsSessionInfo.id.ToString();
        string storedID = PlayerPrefs.GetString("MelodriveSessionID");

        if (!sessionID.Equals(storedID, StringComparison.CurrentCultureIgnoreCase))
            md_sendInfo(Application.productName);

        PlayerPrefs.SetString("MelodriveSessionID", sessionID);
        #endif

        md_init(style, initEmotion);

        if (chiptuneMode)
            SetChiptuneMode(chiptuneMode);

        if (playOnStart)
            Play();
    }

    private void Start()
    {
        releaseMode = Marshal.PtrToStringAnsi(md_getReleaseMode());

        messageQueue = new ArrayList();

        SetInstrumentsPathToStreamingAssets();

        IntPtr delegatePtr1, delegatePtr2;

        DebugHandler debug = new DebugHandler(DebugMethod);
        delegatePtr1 = Marshal.GetFunctionPointerForDelegate(debug);
        md_registerDebugCallback(delegatePtr1);
        DebugLog += OnDebug;

        NewMusicalSeedHandler newMusicalSeed = new NewMusicalSeedHandler(OnNewMusicalSeed);
        delegatePtr1 = Marshal.GetFunctionPointerForDelegate(newMusicalSeed);
        md_registerNewMusicalSeedCallback(delegatePtr1);

        CueChangeHandler cueChange = new CueChangeHandler(OnCueChange);
        delegatePtr1 = Marshal.GetFunctionPointerForDelegate(cueChange);
        md_registerCueChangeCallback(delegatePtr1);
        CueChange += OnCueChange1;

        if (releaseMode == RELEASE_INDIE)
        {
            NoteOnHandler noteOn = new NoteOnHandler(OnNoteOn);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(noteOn);
            NoteOffHandler noteOff = new NoteOffHandler(OnNoteOff);
            delegatePtr2 = Marshal.GetFunctionPointerForDelegate(noteOff);
            md_registerNoteCallbacks(delegatePtr1, delegatePtr2);

            ParamChangeHandler paramChange = new ParamChangeHandler(OnParamChange);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(paramChange);
            md_registerParamChangeCallback(delegatePtr1);

            BeatTickHandler beatTick = new BeatTickHandler(OnBeatTick);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(beatTick);
            md_registerBeatTickCallback(delegatePtr1);

            BeatHandler beat = new BeatHandler(OnBeat);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(beat);
            md_registerBeatCallback(delegatePtr1);

            BarHandler bar = new BarHandler(OnBar);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(bar);
            md_registerBarCallback(delegatePtr1);

            TempoChangeHandler tempoChange = new TempoChangeHandler(OnTempoChange);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(tempoChange);
            md_registerTempoChangeCallback(delegatePtr1);

            ProjectLoadHandler projectLoad = new ProjectLoadHandler(OnProjectLoad);
            delegatePtr1 = Marshal.GetFunctionPointerForDelegate(projectLoad);
            md_registerProjectLoadCallback(delegatePtr1);
        }

        if (playOnStart)
            Init(initStyle, initEmotion);
    }

    private void Update()
    {
        if (chiptuneMode != prevChiptuneMode)
        {
            SetChiptuneMode(chiptuneMode);
            prevChiptuneMode = chiptuneMode;
        }

        HandleMessages();
    }

    /*
	   ___             _         __
	  / _ \_______    (_)__ ____/ /____
	 / ___/ __/ _ \  / / -_) __/ __(_-<
	/_/  /_/  \___/_/ /\__/\__/\__/___/
	             |___/

    */

    [DllImport(DLL_NAME)]
    private static extern void md_saveProject([MarshalAs(UnmanagedType.LPStr)] string filename);

    [DllImport(DLL_NAME)]
    private static extern void md_loadProject([MarshalAs(UnmanagedType.LPStr)] string filename);

    /**
     * Saves a project to the specified path
     */
    public void SaveProject(string filename)
    {
        md_saveProject(filename);
    }

    /**
     * Loads a project from the specified path
     */
    public void LoadProject(string filename)
    {
        md_loadProject(filename);
        if (playOnStart)
            Play();
    }

    /*
       ______       __
      / __/ /___ __/ /__
     _\ \/ __/ // / / -_)
    /___/\__/\_, /_/\__/
            /___/
    */

    [DllImport(DLL_NAME)]
    private static extern void md_loadStyle([MarshalAs(UnmanagedType.LPStr)] string style);

    [DllImport(DLL_NAME)]
    private static extern void md_setStyle([MarshalAs(UnmanagedType.LPStr)] string style);

    /**
     * Returns the list of supported Styles in Melodrive
     */
    public string[] GetStyles()
    {
        return styles;
    }

    /**
     * Preloads all styles to avoid audio artifacts
     */
    public void PreloadStyles()
    {
        foreach (string style in styles)
            md_loadStyle(style);
    }

    /**
     * Sets the current style
     */
    public void SetStyle(string style)
    {
        int idx = Array.IndexOf(styles, style);
        if (idx == -1)
        {
            Debug.LogError("Melodrive: Invalid style: \"" + style + "\".");
            return;
        }

        md_setStyle(style);
    }

    /**
     * Returns the currently playing Style.
     */
    public string GetStyle()
    {
        return currentStyle;
    }

    /*
	   __  ___         _          __  ____           __
	  /  |/  /_ _____ (_)______ _/ / / __/__ ___ ___/ /
	 / /|_/ / // (_-</ / __/ _ `/ / _\ \/ -_) -_) _  /
	/_/  /_/\_,_/___/_/\__/\_,_/_/ /___/\__/\__/\_,_/

    */

    [DllImport(DLL_NAME)]
    private static extern void md_setMusicalSeed([MarshalAs(UnmanagedType.LPStr)] string seedName);

    [DllImport(DLL_NAME)]
    private static extern void md_createMusicalSeed(bool setActive);

    [DllImport(DLL_NAME)]
    private static extern void md_saveMusicalSeed([MarshalAs(UnmanagedType.LPStr)] string seedName, [MarshalAs(UnmanagedType.LPStr)] string filename);

    [DllImport(DLL_NAME)]
    private static extern void md_loadMusicalSeed([MarshalAs(UnmanagedType.LPStr)] string filename);

    [DllImport(DLL_NAME)]
    private static extern int md_getNumMusicalSeeds();

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getMusicalSeedName(int num);

    /**
     * Sets the Musical Seed in Melodrive. This will trigger a CueChange event.
     */
    public void SetMusicalSeed(string seedName)
    {
        md_setMusicalSeed(seedName);
    }

    public string GetMusicalSeed()
    {
        return currentMusicalSeed;
    }

    /**
     * Creates a new Musical Seed. This will trigger a NewMusicalSeed event when complete.
     * @param bool setActive - set to true to trigger a Musical Seed change
     */
    public void CreateMusicalSeed(bool setActive = false)
    {
        md_createMusicalSeed(setActive);
    }

    /**
     * Saves a Given Musical Seed to a file for later loading.
     */
    public void SaveMusicalSeed(string seedName, string filename)
    {
        md_saveMusicalSeed(seedName, filename);
    }

    /**
     * Loads a Musical Seed into the current project.
     */
    public void LoadMusicalSeed(string filename)
    {
        md_loadMusicalSeed(filename);
    }

    /**
     * Returns a list of the Musical Seeds.
     */
    public string[] GetMusicalSeeds()
    {
        int numSeeds = md_getNumMusicalSeeds();

        string[] musicalThemes = new string[numSeeds];
        for (int i = 0; i < numSeeds; i++)
        {
            string name = Marshal.PtrToStringAnsi(md_getMusicalSeedName(i));
            musicalThemes[i] = name;
        }

        return musicalThemes;
    }

    /*
	   ____                   __   __
	  / __/__  ___ ___ __ _  / /  / /__ ___
	 / _// _ \(_-</ -_)  ' \/ _ \/ / -_|_-<
	/___/_//_/___/\__/_/_/_/_.__/_/\__/___/

    */

    [DllImport(DLL_NAME)]
    private static extern int md_getNumEnsembles();

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getEnsembleName(int ensembleIndex);

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getCurrentEnsemble();

    [DllImport(DLL_NAME)]
    private static extern void md_setEnsemble([MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport(DLL_NAME)]
    private static extern void md_setChiptuneMode(bool value);

    [DllImport(DLL_NAME)]
    private static extern bool md_getChiptuneMode();

    /**
     * Returns a list of availiable Ensembles in a Style.
     */
    public string[] GetEnsembles()
    {
        int num = md_getNumEnsembles();
        string[] ensembles = new string[num];
        for (int i = 0; i < num; i++)
        {
            string name = Marshal.PtrToStringAnsi(md_getEnsembleName(i));
            ensembles[i] = name;
        }
        return ensembles;
    }

    /**
     * Returns the currently active Ensemble
     */
    public string GetCurrentEnsemble()
    {
        return Marshal.PtrToStringAnsi(md_getCurrentEnsemble());
    }

    /**
     * Sets the active Ensemble. This will Trigger a CueChange.
     */
    public void SetEnsemble(string value)
    {
        md_setEnsemble(value);
    }

    /**
     * Activates/deactivates Melodrive's Chiptune mode.
     * While active, all the ensembles will be Chiptune until deactivated.
     * Note Chiptune mode is deactivated if SetEnsemble is called with a non-chiptune ensemble.
     */
    public void SetChiptuneMode(bool value)
    {
        chiptuneMode = value;
        prevChiptuneMode = value;
        md_setChiptuneMode(value);
    }

    /**
     * Returns the current state of Chiptune mode.
     */
    public bool GetChiptuneMode()
    {
        return md_getChiptuneMode();
    }

    /*
	   ____           __  _               __  __   _     __
	  / __/_ _  ___  / /_(_)__  ___     _/_/ / /  (_)__ / /____ ___  ___ ____
	 / _//  ' \/ _ \/ __/ / _ \/ _ \  _/_/  / /__/ (_-</ __/ -_) _ \/ -_) __/
	/___/_/_/_/\___/\__/_/\___/_//_/ /_/   /____/_/___/\__/\__/_//_/\__/_/

    */

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getEmotionMode();

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotionMode([MarshalAs(UnmanagedType.LPStr)] string mode);

    /**
     * Returns the current emotion control "mode". Options are "positional" (default),
     * "discrete" or "direct". The mode changes automatically when you use the SetVA or
     * SetEmotion methods, but you have to chnage back to "positional" mode if you'd like
     * to use EmotionalPoints again.
     */
    public string GetEmotionMode()
    {
        return Marshal.PtrToStringAnsi(md_getEmotionMode());
    }

    /**
     * Sets the current emotion mode. Options:
     *      "positional" - will listen to the listener position and the
     *                      emotional points to determine emotion
     *      "discrete" - set when using SetEmotion. Emotion values map to
     *                      discrete points
     *      "direct" - set when using SetVA. Allows direct control over
     *                      the VA space
     */
    public void SetEmotionMode(string mode)
    {
        md_setEmotionMode(mode);
    }

    [DllImport(DLL_NAME)]
    private static extern float md_getEmotionalVelocity();

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotionalVelocity(float value);

    /**
     * Returns the emotional velocity, which is how fast emotion changes happen.
     */
    public float GetEmotionalVelocity()
    {
        return md_getEmotionalVelocity();
    }

    /**
     * Sets the emotional velocity, which is how fast emotion changes happen.
     */
    public void SetEmotionalVelocity(float value)
    {
        md_setEmotionalVelocity(value);
    }

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr md_getEmotion();

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotion([MarshalAs(UnmanagedType.LPStr)] string emotion);

    /**
     * Returns the current emotion as a string e.g. "happy", "sad"
     */
    public string GetEmotion()
    {
        return Marshal.PtrToStringAnsi(md_getEmotion());
    }

    /**
     * Sets the current emotionas a string e.g. "happy", "sad".
     * This will also set the emotion mode to "discrete".
     */
    public void SetEmotion(string emotion)
    {
        md_setEmotion(emotion);
    }

    [DllImport(DLL_NAME)]
    private static extern float md_getValence();

    [DllImport(DLL_NAME)]
    private static extern void md_setValence(float value);

    [DllImport(DLL_NAME)]
    private static extern float md_getArousal();

    [DllImport(DLL_NAME)]
    private static extern void md_setArousal(float value);

    /**
     * Returns the current Valence (x) Arousal (y) point
     */
    public Vector2 GetVA()
    {
        Vector2 va = new Vector2();
        va.x = md_getValence();
        va.y = md_getArousal();
        return va;
    }

    /**
     * Set the Valence/Arousal point. This will also set the emotion mode to "direct".
     */
    public void SetVA(Vector2 value)
    {
        md_setValence(value.x);
        md_setArousal(value.y);
    }

    [DllImport(DLL_NAME)]
    private static extern void md_addEmotionalPoint(int id, float x, float y, [MarshalAs(UnmanagedType.LPStr)] string mood);

    [DllImport(DLL_NAME)]
    private static extern void md_removeEmotionalPoint(int id);

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotionalPointPosition(int id, float x, float y);

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotionAtPoint(int id, [MarshalAs(UnmanagedType.LPStr)] string mood);

    [DllImport(DLL_NAME)]
    private static extern void md_clearEmotionalPoints();

    [DllImport(DLL_NAME)]
    private static extern void md_setListenerPosition(float x, float y);

    [DllImport(DLL_NAME)]
    private static extern float md_getEmotionalStrength();

    [DllImport(DLL_NAME)]
    private static extern void md_setEmotionalStrength(float value);

    /**
     * Add an emotional point in game-space.
     * @param float x - the x co-ord
     * @param float y - the y co-ord
     * @param string mood - the mood of the point e.g. "happy"
     * @return int - the id of the created point
     */
    public int AddEmotionalPoint(float x, float y, [MarshalAs(UnmanagedType.LPStr)] string mood)
    {
        int id = nextEmotionalPointID;
        nextEmotionalPointID++;
        md_addEmotionalPoint(id, x, y, mood);
        return id;
    }

    /**
     * Remove an emotional point from Melodrive.
     * @param int id - the id of the point to remove
     */
    public void RemoveEmotionalPoint(int id)
    {
        md_removeEmotionalPoint(id);
    }

    /**
     * Update an emotional point's position in game-space.4
     * @param int id - Melodrives ID for the point
     * @param float x - the x co-ord
     * @param float y - the y co-ord
     */
    public void SetEmotionalPointPosition(int id, float x, float y)
    {
        md_setEmotionalPointPosition(id, x, y);
    }

    /**
     * Updates an emotional points mood
     * @param int id - Melodrive's ID for the point
     * @param string mood - the new mood
     */
    public void SetEmotionAtPoint(int id, [MarshalAs(UnmanagedType.LPStr)] string emotion)
    {
        md_setEmotionAtPoint(id, emotion);
    }

    /**
    * Cleares the emotional points from the scene
    */
    public void ClearEmotionalPoints()
    {
        md_clearEmotionalPoints();
    }

    /**
     * Updates the position of the listener in world space.
     * @param float x
     * @param float y
     */
    public void SetListenerPosition(float x, float y)
    {
        if (x != listener.x || y != listener.y)
        {
            listener.Set(x, y);
            md_setListenerPosition(x, y);
        }
    }

    /**
     * Returns the emotional strength, which is how strong emotional points
     * affect the emotion.
     */
    public float GetEmotionalStrength()
    {
        return md_getEmotionalStrength();
    }

    /**
     * Sets the emotional strength, which is how strong emotional points
     * affect the emotion.
     */
    public void SetEmotionalStrength(float value)
    {
        md_setEmotionalStrength(value);
    }

    /*
     ______                                __
    /_  __/______ ____  ___ ___  ___  ____/ /_
     / / / __/ _ `/ _ \(_-</ _ \/ _ \/ __/ __/
    /_/ /_/  \_,_/_//_/___/ .__/\___/_/  \__/
                         /_/
    */

    [DllImport(DLL_NAME)]
    private static extern void md_setTempoScale(float value);

    [DllImport(DLL_NAME)]
    private static extern void md_play();

    [DllImport(DLL_NAME)]
    private static extern void md_pause();

    [DllImport(DLL_NAME)]
    private static extern void md_stop();

    [DllImport(DLL_NAME)]
    private static extern void md_advance(double deltaTime);

    [DllImport(DLL_NAME)]
    private static extern void md_release();

    [DllImport(DLL_NAME)]
    private static extern void md_setCue([MarshalAs(UnmanagedType.LPStr)] string cueName);

    [DllImport(DLL_NAME)]
    private static extern void md_setStateOptions([MarshalAs(UnmanagedType.LPStr)] string style, [MarshalAs(UnmanagedType.LPStr)] string musicalSeed, [MarshalAs(UnmanagedType.LPStr)] string ensemble);

    /**
     * Sets the tempo "scale", which is a multiplier on the base tempo that
     * Melodrive chooses.
     */
    public void SetTempoScale(float value)
    {
        md_setTempoScale(value);
    }

    /**
     * Starts playback
     */
    public void Play()
    {
        md_play();
    }

    /**
     * Pauses playback
     */
    public void Pause()
    {
        md_pause();
    }

    /**
     * Stops playback
     */
    public void Stop()
    {
        md_stop();
    }

    /**
     * Activates a Cue in Melodrive
     */
    public void SetCue(string cueName)
    {
        md_setCue(cueName);
    }

    /**
     * Sets the playback state in Melodrive
     */
    public void SetStateOptions(string style, string musicalSeed, string ensemble)
    {
        md_setStateOptions(style, musicalSeed, ensemble);
    }

    private void OnApplcationQuit()
    {
#if UNITY_EDITOR
#else
        md_release();
#endif
    }

    /*
       ___          ___
      / _ |__ _____/ (_)__
     / __ / // / _  / / _ \
    /_/ |_\_,_/\_,_/_/\___/

    */

    [DllImport(DLL_NAME)]
    private static extern void md_setMasterGain(float value);

    [DllImport(DLL_NAME)]
    private static extern void md_setLimiterEnabled(bool value);

    [DllImport(DLL_NAME)]
    private static extern float md_getRMS(int channel);

    /**
     * Sets the gain of the Master bus
     */
    public void SetMasterGain(float value)
    {
        md_setMasterGain(value);
    }

    /**
     * Turns the limiter on/off
     */
    public void SetLimiterEnabled(bool value)
    {
        md_setLimiterEnabled(value);
    }

    public float[] GetRMS()
    {
        int channels = 2;
        float[] rms = new float[channels];
        for (int i = 0; i < channels; i++)
            rms[i] = md_getRMS(i);
        return rms;
    }
    
    /*
      _____     ______            __
     / ___/__ _/ / / /  ___ _____/ /__ ___
    / /__/ _ `/ / / _ \/ _ `/ __/  '_/(_-<
    \___/\_,_/_/_/_.__/\_,_/\__/_/\_\/___/

    */

    private class MDMessage { }

    private class DebugMessage : MDMessage
    {
        public string message;

        public DebugMessage(string message)
        {
            this.message = message;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerDebugCallback(IntPtr callback);

    private class NoteOffMessage : MDMessage
    {
        public string part;
        public int num;

        public NoteOffMessage(string part, int num)
        {
            this.part = part;
            this.num = num;
        }
    }

    private class NoteOnMessage : NoteOffMessage
    {
        public int velocity;

        public NoteOnMessage(string part, int num, int vel) :
            base(part, num)
        {
            velocity = vel;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerNoteCallbacks(IntPtr noteOnCallback, IntPtr noteOffCallback);

    private class ParamChangeMessage : MDMessage
    {
        public string part;
        public string param;
        public float value;

        public ParamChangeMessage(string part, string param, float value)
        {
            this.part = part;
            this.param = param;
            this.value = value;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerParamChangeCallback(IntPtr callback);

    private class BarMessage : MDMessage
    {
        public float bar;

        public BarMessage(float bar)
        {
            this.bar = bar;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerBarCallback(IntPtr callback);

    private class BeatMessage : BarMessage
    {
        public float beat;

        public BeatMessage(float beat, float bar) : base(bar)
        {
            this.beat = beat;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerBeatCallback(IntPtr callback);

    private class BeatTickMessage : BeatMessage
    {
        public int tick;

        public BeatTickMessage(int tick, float beat, float bar) : base(beat, bar)
        {
            this.tick = tick;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerBeatTickCallback(IntPtr callback);

    private class TempoChangeMessage : MDMessage
    {
        public float bpm;

        public TempoChangeMessage(float bpm)
        {
            this.bpm = bpm;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerTempoChangeCallback(IntPtr callback);

    private class NewMusicalSeedMessage : MDMessage
    {
        public string name;

        public NewMusicalSeedMessage(string name)
        {
            this.name = name;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerNewMusicalSeedCallback(IntPtr callback);

    private class CueChangeMessage : MDMessage
    {
        public string cue;
        public string seedName;
        public string style;

        public CueChangeMessage(string cue, string seedName, string style)
        {
            this.cue = cue;
            this.seedName = seedName;
            this.style = style;
        }
    }

    [DllImport(DLL_NAME)]
    private static extern void md_registerCueChangeCallback(IntPtr callback);

    private class ProjectLoadMessage : MDMessage { }
    [DllImport(DLL_NAME)]
    private static extern void md_registerProjectLoadCallback(IntPtr callback);

    private static ArrayList messageQueue;

    private static void DebugMethod(string message)
    {
        messageQueue.Add(new DebugMessage(message));
    }

    private void OnDebug(string message)
    {
        Debug.Log(message);
    }

    private static void OnNoteOn(string part, int num, int velocity)
    {
        messageQueue.Add(new NoteOnMessage(part, num, velocity));
    }

    private static void OnNoteOff(string part, int num)
    {
        messageQueue.Add(new NoteOffMessage(part, num));
    }

    private static void OnParamChange(string part, string param, float value)
    {
        messageQueue.Add(new ParamChangeMessage(part, param, value));
    }

    private static void OnBeatTick(int ticks, float beat, float bar)
    {
        messageQueue.Add(new BeatTickMessage(ticks, beat, bar));
    }

    private static void OnBeat(float beat, float bar)
    {
        messageQueue.Add(new BeatMessage(beat, bar));
    }

    private static void OnBar(float bar)
    {
        messageQueue.Add(new BarMessage(bar));
    }

    private static void OnTempoChange(float bpm)
    {
        messageQueue.Add(new TempoChangeMessage(bpm));
    }

    private static void OnNewMusicalSeed(string seedName)
    {
        messageQueue.Add(new NewMusicalSeedMessage(seedName));
    }

    private static void OnCueChange(string cue, string seedName, string style)
    {
        messageQueue.Add(new CueChangeMessage(cue, seedName, style));
    }

    private void OnCueChange1(string cue, string seedName, string style)
    {
        currentMusicalSeed = seedName;
        currentStyle = style;
        chiptuneMode = GetChiptuneMode();
    }

    private static void OnProjectLoad()
    {
        messageQueue.Add(new ProjectLoadMessage());
    }

    private void HandleMessages()
    {
        ArrayList q = (ArrayList)messageQueue.Clone();
        messageQueue.Clear();

        foreach (MDMessage msg in q)
        {
            if (msg is DebugMessage)
            {
                if (DebugLog == null)
                    continue;

                DebugMessage msg2 = (DebugMessage)msg;
                DebugLog(msg2.message);
                continue;
            }
            if (msg is NoteOnMessage)
            {
                if (NoteOn == null)
                    continue;

                NoteOnMessage msg2 = (NoteOnMessage)msg;
                NoteOn(msg2.part, msg2.num, msg2.velocity);
                continue;
            }
            if (msg is NoteOffMessage)
            {
                if (NoteOff == null)
                    continue;

                NoteOffMessage msg2 = (NoteOffMessage)msg;
                NoteOff(msg2.part, msg2.num);
                continue;
            }
            if (msg is ParamChangeMessage)
            {
                if (ParamChange == null)
                    continue;

                ParamChangeMessage msg2 = (ParamChangeMessage)msg;
                ParamChange(msg2.part, msg2.param, msg2.value);
                continue;
            }
            if (msg is BeatTickMessage)
            {
                if (BeatTick == null)
                    continue;

                BeatTickMessage msg2 = (BeatTickMessage)msg;
                BeatTick(msg2.tick, msg2.beat, msg2.bar);
                continue;
            }
            if (msg is BeatMessage)
            {
                if (Beat == null)
                    continue;

                BeatMessage msg2 = (BeatMessage)msg;
                Beat(msg2.beat, msg2.bar);
                continue;
            }
            if (msg is BarMessage)
            {
                if (Bar == null)
                    continue;

                BarMessage msg2 = (BarMessage)msg;
                Bar(msg2.bar);
                continue;
            }
            if (msg is TempoChangeMessage)
            {
                if (TempoChange == null)
                    continue;

                TempoChangeMessage msg2 = (TempoChangeMessage)msg;
                TempoChange(msg2.bpm);
                continue;
            }
            if (msg is NewMusicalSeedMessage)
            {
                if (NewMusicalSeed == null)
                    continue;

                NewMusicalSeedMessage msg2 = (NewMusicalSeedMessage)msg;
                NewMusicalSeed(msg2.name);
                continue;
            }
            if (msg is CueChangeMessage)
            {
                if (CueChange == null)
                    continue;

                CueChangeMessage msg2 = (CueChangeMessage)msg;
                CueChange(msg2.cue, msg2.seedName, msg2.style);
                continue;
            }
            if (msg is ProjectLoadMessage)
            {
                if (ProjectLoad == null)
                    continue;

                //ProjectLoadMessage msg2 = (ProjectLoadMessage)msg;
                ProjectLoad();
                continue;
            }
        }
    }

#if UNITY_EDITOR

    [InitializeOnLoadAttribute]
    public static class PlayStateNotifier
    {

        static PlayStateNotifier()
        {
            EditorApplication.playModeStateChanged += PlayStateChanged;
            EditorApplication.pauseStateChanged += PauseStateChanged;
        }

        static void PlayStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                md_stop();
        }

        static void PauseStateChanged(PauseState state)
        {
            if (state == PauseState.Paused)
                md_pause();
            else
                md_play();
        }
    }

#endif
}