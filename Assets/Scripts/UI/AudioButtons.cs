using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AudioButtons : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private Image image;
    private static Dictionary<string, bool> NameMatch = new Dictionary<string, bool>();

    void Awake()
    {
        image = GetComponent<Image>();
        if (!NameMatch.ContainsKey(gameObject.name))
        {
            NameMatch[gameObject.name] = true; // default state
        }
        UpdateSprite();
    }

    public void OnMusicPress()
    {
        bool isOn = NameMatch[gameObject.name];
        isOn = !isOn;
        NameMatch[gameObject.name] = isOn;

        if (gameObject.name == "Music" && GlobalMusicManager.Instance != null)
        {
            GlobalMusicManager.Instance.SetMusicMuted(!isOn);
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        bool isOn = NameMatch[gameObject.name];
        image.sprite = isOn ? sprites[0] : sprites[1];
    }
}