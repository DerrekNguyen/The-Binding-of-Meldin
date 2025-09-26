using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Image))]
public class AudioButtons : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private Image image;
    private bool IsOn = true;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnMusicPress()
    {
        IsOn = !IsOn; // Toggle state

        if (IsOn)
        {
            image.sprite = sprites[0];
        }
        else
        {
            image.sprite = sprites[1];
        }
    }
}