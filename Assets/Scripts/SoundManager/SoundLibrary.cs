using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sound Library Class

[System.Serializable]
public struct SoundEffect
{
    public string name;
    public AudioClip clip;
    public float volume;
}


public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

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
