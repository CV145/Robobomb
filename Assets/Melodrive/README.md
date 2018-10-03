       __  ___    __        __    _
      /  |/  /__ / /__  ___/ /___(_)  _____
     / /|_/ / -_) / _ \/ _  / __/ / |/ / -_)
    /_/  /_/\__/_/\___/\_,_/_/ /_/|___/\__/


# Melodrive Unity SDK v0.3.5

**Please note that this is beta software and so there may be bugs and crashes!**

## Overview

Melodrive is an AI adaptive music generation system for interactive media.
This package contains the Melodrive Audio Plugin for Unity Editor.

## Quick Start

1) Add the Melodrive Prefab (in `Melodrive/Resources`) to your scene.
2) (optional) Enter style and emotion parameters
3) Click "Play On Start" checkbox on the prefab.
4) Enter play mode!

## Package Contents

The included assets are structured as follows:
- Melodrive
    - Branding - This folder contains immages for the Melodrive splash screen
    - Examples - This folder contains a number of different examples, each showing one Melodrive feature
    - Materials - This folder contains the materials used in the Melodrive scenes
    - Mixers - This is where the Melodrive Mixer is stored
    - Plugins - This is where the native plugins are stored for the various platforms
    - Resources - This contains the Melodrive Prefab, which needs to be added to every scene using Melodrive
    - Scenes - The Melodrive Editor scene is stored here
    - Scripts - This folder contains various Scripts Melodrive uses to run
    - Tests - This folder contains Melodrive's unit tests
- StreamingAssets
    - Melodrive - Projects and Musical Seeds are saved here by default
    - MelodriveInstruments - This is where all the instruments are stored

## Examples

This package includes a set of example scenes to show you how Melodrive works.
It's a good idea to start by looking in here.

## Video Tutorials

There is a series of video tutorials on our [YouTube channel](https://www.youtube.com/watch?v=fQeoPf9ivL4&list=PLyOZFq1fEfUFfFWn0fSsp5Ev73ahUGJqC).

## API Documentation

The C++ / C# documentation is included in the release download.
    
## Features

### Style
Melodrive generates music in certain `Styles`, these Styles dictate the overall musical feel.
The Styles available in this release are:
- `ambient` - a slow and meditative style
- `house` - an electronic dance music style
- `piano` - a simple solo piano style
- `rock` - a classic rock band style

Use `SetStyle` to change the active style.

### Emotion
Within a Style, the music will adapt to the given emotion.
You can control the emotion in 3 different ways:

1) `discrete` - use the `SetEmotion` method on the Melodrive Prefab to set emotion to one of five states:
    - "happy"
    - "angry"
    - "sad"
    - "tender"
    - "neutral"
2) `positional` - use `AddEmotionalPoint` and the related methods to set up an Emotional space (simplified to 2D space), then use `SetListenerPosition` to update the current emotional state.
3) `direct` - use the `SetVA` method to control the emotion with a [Valence-Arousal](https://www.researchgate.net/profile/Lung-Hao_Lee/publication/304124018/figure/fig1/AS:374864755085312@1466386130906/Two-dimensional-valence-arousal-space.png) point.

N.B. If you have used `discrete` or `direct` mode emotion controls, and wish to switch to `positional` mode, you must re-activate it with `SetEmotionMode`. 

### Musical Seed

Musical Seeds are the core compositional elements of the music.
This is the core musical material (melody/chords) that Melodrive adapts infinitely.
They can be shared across Styles, which ensures that the overall musical content is be the same, but certain style-specific aspects will be different e.g. what instruments are being used.
We recommend having a play with the same Musical Seed across different styles to get an idea of how this works in practice.

Use `SetMusicalSeed` to change the active Musical Seed.

### Ensembles

An Ensemble is a collection of musical parts, similar to a virtual band.
For example, a simple rock band might contain bass, drums and guitar, but some rock bands also include a piano, or a synthesiser.
Melodrive allows you to select from a list of Ensembles per Style.

Use `SetEnsemble` to change the active Ensemble.

### Chiptune Mode

Melodrive includes a Chiptune mode for that classic 8bit sound.
Use `SetChiptuneMode` to enable it!

### Melodrive Editor

Melodrive Editor is a Unity scene with a simple GUI that allows you to play around with various Melodrive features.
You can change Style, Musical Seed, Ensemble and Emotion here, as well as create, save and load Musical Seeds, and save your changes to a Project file for loading into your experience.

### Events

As well as outputting music, Melodrive fires events for various things that happen in the system.
There are a few events you can find in the `MelodrivePlugin` script, but here we'll highlight a few of them.
All events are C# delegates.

- `NoteOn` - Fired whenever there's a note onset in the music.
The parameters you'll get are:
    - `part` - e.g. `lead`, `pad`, `drums`
    - `num` - the MIDI note number e.g. 60
    - `velocity` - the MIDI velocity e.g. 127
- `NoteOff` - Fired whenever a note ends in the music.
 The parameters you'll get are:
    - `part` - e.g. `lead`, `pad`, `drums`
    - `num` - the MIDI note number e.g. 60
- `ParamChange` - Fired whenever a music parameter changes in the music.
The parameters you'll get are:
    - `part` - e.g. `lead`, `pad`, `drums`
    - `param` - the param name that's changing e.g. `cutoff`, `delayTime`
    - `value` - the new value of the param
- `Beat` - Fired whenever there's a `beat` event. e.g. if the tempo is 120 beats per minute, one beat event will be fired every 0.5 seconds.
You can use this to sync visuals to the tempo.  
The parameters you'll get are:
    - `beat` - the beat number
    - `bar` - the bar number

### Misc controls

- `Play`, `Pause` and `Stop` - call these to start/pause/stop playback
- `SetTempoScale` - you can speed up or slow down the tempo with this simple tempo scale control.
E.g. If you want the music to be twice as fast, pass in a value of 2.
- `SetMasterGain` - sets the overall gained output of the Music. Useful for when you want to duck the audio to hear other sources such as speech or effects.
- `SetLimiterEnabled` - by default, Melodrive "limits" the audio sent to Unity, so that there's no distortion. You can enable/disable this feature here.

## License

Your use of this software is governed by the [Melodrive SDK Developer and Software License Agreement](http://melodrive.com/SDKDeveloperAgreement.html).
