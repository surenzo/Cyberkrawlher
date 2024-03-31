# EzSoundManager

## Introduction

EzSoundManager is a comprehensive audio management system for Unity, designed to facilitate easy and efficient sound and
music control within games and interactive applications. Created by KZ, this asset simplifies the implementation of
common audio functionalities like playing music, sound effects, managing audio pools, and audio transitions.

## Installation

To install and use EzSoundManager in your Unity project, follow these steps:

1. Extract the EzSoundManager archive into the `Assets/Scripts` folder of your Unity project.

3. To create an audio database, right-click on the `Resources` folder and choose "Create" > "Audio/Audio
   Database." Name the database "AudioDatabase."

4. Populate the audio database with audio clips and categories as needed. You can organize audio clips into categories
   and subcategories within the AudioDatabase asset.

## Usage

### Playing Background Music

To play background music, use the following code snippet:

```csharp
SoundManager.Instance.PlayMusicByCategory("BackgroundTrack", "Music", true);
```

### Playing a Sound Effect

To play a sound effect, use the following code snippet:

```csharp
SoundManager.Instance.PlaySoundEffect("Explosion", "SFX", Vector3.zero, true);
```

## Features

- **Singleton Pattern:** Centralized management of audio playback.
- **Audio Pooling:** Efficient handling of audio sources.
- **3D Sound Support:** Play sounds with spatial awareness.
- **Music and Sound Effects:** Separate handling for music tracks and sound effects.
- **Dynamic Volume Control:** Adjust volumes for different audio groups.
- **Fade and Crossfade:** Smooth transitions for audio tracks.
- **Random Pitch & Sound Selection:** Variation in audio playback.

## Configuration

You can configure EzSoundManager by adjusting the following parameters in the Inspector:

- Audio Mixer settings
- Pool Size for audio sources
- Volume levels for different audio categories

## Contributors

- KZ (Creator)

---

## EzSoundManager AudioPlayer

The `AudioPlayer` class is a crucial part of the EzSoundManager asset, responsible for managing audio sources and
playing sound effects in your game. Here's a complete guide on how to use the `AudioPlayer` class effectively:

### Adding the AudioPlayer

1. Attach the `AudioPlayer` component to a GameObject in your scene. You can do this by dragging and dropping
   the `AudioPlayer` script onto a GameObject in the Unity editor.

### Configuring the Audio Pool

2. In the Inspector, you can configure the audio pool size by setting the value of `poolSize`. This determines how many
   audio sources will be pre-created for playing sound effects. By default, the pool contains 3 audio sources.

### Playing a Sound Effect

3. To play a sound effect, use the `PlaySound` method of the `AudioPlayer` class. Here's an example:

```csharp
string clipName = "Explosion"; // Name of the sound effect
string category = "SFX"; // Category of the sound effect
bool is3D = true; // Is the sound effect 3D or not

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.PlaySound(clipName, category, is3D);
```

- `clipName`: The name of the sound effect you want to play.
- `category`: The category to which the sound effect belongs.
- `volume`: The volume of the sound effect (ranging from 0 to 1).
- `is3D`: Determines whether the sound effect should be spatialized in 3D space.

### Playing a Sound Effect with Fade

4. The `AudioPlayer` also allows you to play a sound effect with a fade using the `PlaySoundWithFade` method. Here's an
   example:

```csharp
string clipName = "Explosion"; // Name of the sound effect
string category = "SFX"; // Category of the sound effect
float fadeDuration = 2.0f; // Duration of the fade in seconds
float targetVolume = 1.0f; // Target volume of the sound effect
bool is3D = true; // Is the sound effect 3D or not

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.PlaySoundWithFade(clipName, category, fadeDuration, targetVolume, is3D);
```

- `clipName`: The name of the sound effect you want to play.
- `category`: The category to which the sound effect belongs.
- `fadeDuration`: The duration of the fade in seconds.
- `targetVolume`: The target volume of the sound effect (ranging from 0 to 1).
- `is3D`: Determines whether the sound effect should be spatialized in 3D space.

### Modifying Pitch

5. You can also modify the pitch (tonal height) of a sound effect using the `SetPitch` method. Here's an example:

```csharp
string clipName = "Explosion"; // Name of the sound effect
float newPitch = 1.2f; // New pitch of the sound effect

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.SetPitch(clipName, newPitch);
```

- `clipName`: The name of the sound effect you want to change the pitch of.
- `newPitch`: The new pitch of the sound effect (1.0f corresponds to the normal pitch, values higher increase it, and
  values lower decrease it).

### Adding and Removing Echo Effect

6. The `AudioPlayer` allows you to add and remove echo effects to a sound effect. Here's how to do it:

#### Adding an Echo Effect

```csharp
string clipName = "Explosion"; // Name of the sound effect
float delay = 0.2f; // Delay of the echo in seconds
float decayRatio = 0.5f; // Decay ratio of the echo

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.AddEchoEffect(clipName, delay, decayRatio);
```

- `clipName`: The name of the sound effect to which you want to add an echo.
- `delay`: The delay of the echo in seconds.
- `decayRatio`: The decay ratio of the echo (ranging from 0 to 1).

#### Removing an Echo Effect

```csharp
string clipName = "Explosion"; // Name of the sound effect

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.RemoveEchoEffect(clipName);
```

- `clipName`: The name of the sound effect from which you want to remove the echo.

## Conclusion

The `AudioPlayer` in the EzSoundManager asset is a powerful tool for managing sound effects in your Unity game. It
allows you to play sound effects, modify them in real-time, and add echo effects to create an immersive sound experience
for your players. Use it wisely to enhance the ambiance and immersion of your game.