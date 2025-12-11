using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles background music and dynamic creation of audio sources

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundLibrary soundLibrary;
    [SerializeField] private SoundLibrary musicLibrary;

    [SerializeField] static public float globalMusicVolume = 1f;
    [SerializeField] static public float lastKnownGlobalMusicVolume;
    [SerializeField] static public float globalSoundVolume = 1f;

    static public bool isMusicMuted;
    static public bool isSoundMuted;
    
    [SerializeField] public AudioSource backgroundMusic;
    [SerializeField] private float volumeRampDuration = 1f;

    private string currentSceneName = "";
    private Coroutine volumeRampCoroutine;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        lastKnownGlobalMusicVolume = globalMusicVolume;
    }

    // Update
    void Update()
    {
        string gottenSceneName = SceneManager.GetActiveScene().name;

        if(lastKnownGlobalMusicVolume != globalMusicVolume)
        {
            ChangeBackgroundMusic(currentSceneName);
        }

        if (currentSceneName != gottenSceneName)
        {
            currentSceneName = gottenSceneName;    
            ChangeBackgroundMusic(currentSceneName);
        }

        lastKnownGlobalMusicVolume = globalMusicVolume;
    }
    
    public void ChangeBackgroundMusic(string sceneName)
    {
        if (musicLibrary == null || backgroundMusic == null)
        {
            return;
        }
        
        AudioClip newMusicClip = musicLibrary.GetClipFromName(sceneName);
        
        if (newMusicClip == null)
        {
            return;
        }
        
        if (backgroundMusic.clip == newMusicClip)
        {
            float tv = musicLibrary.GetVolumeFromName(sceneName) * globalMusicVolume;
            backgroundMusic.volume = tv;
            return;
        }
        
        if (volumeRampCoroutine != null)
        {
            StopCoroutine(volumeRampCoroutine);
        }
        
        backgroundMusic.clip = newMusicClip;
        backgroundMusic.loop = true;
        backgroundMusic.volume = 0f;
        backgroundMusic.Play();
        
        float targetVolume = musicLibrary.GetVolumeFromName(sceneName) * globalMusicVolume;
        backgroundMusic.volume = targetVolume;
    }

    public void PlaySound2D(string soundName)
    {
        if(isSoundMuted)
        {
            return;
        }

        if (soundLibrary == null)
        {
            Debug.LogWarning("Sound library is not assigned!");
            return;
        }

        AudioClip clip = soundLibrary.GetClipFromName(soundName);

        if (clip == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found in sound library!");
            return;
        }
        
        GameObject tempAudioObject = new GameObject($"TempAudio_{soundName}");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
        
        tempAudioSource.clip = clip;
        tempAudioSource.playOnAwake = false;
        tempAudioSource.spatialBlend = 0f;
        tempAudioSource.volume = soundLibrary.GetVolumeFromName(soundName) * globalSoundVolume;
        
        tempAudioSource.Play();
        
        StartCoroutine(DestroyAfterClipFinishes(tempAudioObject, clip.length));
    }
    
    private IEnumerator DestroyAfterClipFinishes(GameObject audioObject, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        if (audioObject != null)
        {
            Destroy(audioObject);
        }
    }

    public void DecreaseMusicVolume()
    {
        if(globalMusicVolume > 0.0f)
            globalMusicVolume -= 0.1f;
        if(globalMusicVolume < 0.0f)
            globalMusicVolume = 0.0f;
    }

    public void IncreaseMusicVolume()
    {
        if(globalMusicVolume < 1.0f)
            globalMusicVolume += 0.1f;
        if(globalMusicVolume > 1.0f)
            globalMusicVolume = 1.0f;
    }

    public void DecreaseSoundVolume()
    {
        if(globalSoundVolume > 0.0f)
            globalSoundVolume -= 0.1f;
        if(globalSoundVolume < 0.0f)
            globalSoundVolume = 0.0f;
    }

    public void IncreaseSoundVolume()
    {
        if(globalSoundVolume < 1.0f)
            globalSoundVolume += 0.1f;
        if(globalSoundVolume > 1.0f)
            globalSoundVolume = 1.0f;
    }

    public void MuteMusicVolume()
    {
        if(!isMusicMuted)
        {
            isMusicMuted = true;
            backgroundMusic.mute = true;          
        }
        else
        {
            isMusicMuted = false; 
            backgroundMusic.mute = false;           
        }
    }  

    public void MuteSoundVolume()
    {
        if(!isSoundMuted)
        {
            isSoundMuted = true;           
        }
        else
        {
            isSoundMuted = false;
        }
    }
}
