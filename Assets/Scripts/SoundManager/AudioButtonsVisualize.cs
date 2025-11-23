using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MusicButton : MonoBehaviour
{
    // Sprites for muted and unmuted states
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private bool isMusicButton;

    // Reference to this GameObject's Image component
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
        // Make sure Sprites exist
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