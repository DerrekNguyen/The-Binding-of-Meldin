using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonHelper : MonoBehaviour
{
    public void ToggleMusicMute()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.MuteMusicVolume();
    }

    public void ToggleSoundMute()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.MuteSoundVolume();
    }

    public void IncrementSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.IncreaseSoundVolume();        
    }

    public void DecrementSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.DecreaseSoundVolume();        
    }

    public void IncrementMusic()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.IncreaseMusicVolume();        
    }

    public void DecrementMusic()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.DecreaseMusicVolume();        
    }
}
