using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private bool isMusicText;

    // Reference to this GameObject's Text component
    private TMP_Text volumeText;

    void Awake()
    {
        volumeText = GetComponent<TMP_Text>();
        if (volumeText == null)
        {
            Debug.LogWarning("No TMP_Text component found on this GameObject.");
        }
    }

    void Start()
    {
        SetText();
    }

    void Update()
    {
        SetText();
    }

    void SetText()
    {
        if (volumeText == null) return;

        if (isMusicText)
        {
            volumeText.text = SoundManager.globalMusicVolume.ToString("0.0");
        }
        else
        {
            volumeText.text = SoundManager.globalSoundVolume.ToString("0.0");
        }
    }
}
