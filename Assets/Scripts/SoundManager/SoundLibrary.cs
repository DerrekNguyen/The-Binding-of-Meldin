using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sound Library

// Sound effect struct
[System.Serializable]
public struct SoundEffect
{
    public string name;
    public AudioClip clip;
    public float volume;
}


public class SoundLibrary : MonoBehaviour
{
    // Array of sound effects
    public SoundEffect[] soundEffects;

    // Look up audio clip from name
    public AudioClip GetClipFromName(string name)
    {
        foreach( var SoundEffect in soundEffects) {
            if(name == SoundEffect.name)
            {
                return SoundEffect.clip;
            }
        }
        return null;
    }

    // Look up audio volume from name
    public float GetVolumeFromName(string name)
    {
        foreach( var SoundEffect in soundEffects) {
            if(name == SoundEffect.name)
            {
                return SoundEffect.volume;
            }
        }
        return 0;
    }
}
