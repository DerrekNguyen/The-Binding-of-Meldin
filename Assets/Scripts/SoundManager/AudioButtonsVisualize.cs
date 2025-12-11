using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles audio button visuals

[RequireComponent(typeof(Image))]
public class MusicButton : MonoBehaviour
{
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private bool isMusicButton;

    private Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogWarning("No Image component found on this GameObject.");
        }
    }

    void Start()
    {
        if (mutedSprite == null || unmutedSprite == null)
        {
            Debug.LogWarning("Muted or Unmuted Sprite is not assigned in the inspector.");
        }
        SetImage();
    }

    void Update()
    {
        SetImage();
    }

    void SetImage()
    {
        if (buttonImage == null) return;

        if (isMusicButton)
        {
            buttonImage.sprite = SoundManager.isMusicMuted ? mutedSprite : unmutedSprite;
        }
        else
        {
            buttonImage.sprite = SoundManager.isSoundMuted ? mutedSprite : unmutedSprite;
        }
    }
}