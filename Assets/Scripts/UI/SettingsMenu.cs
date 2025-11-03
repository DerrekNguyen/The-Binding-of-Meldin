using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI generalVolumeText;

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        musicVolumeText.text = Settings.Instance.MusicVolume.ToString("0.0");
        generalVolumeText.text = Settings.Instance.GeneralVolume.ToString("0.0");
    }

    public void DecreaseMusicVolume()
    {
        Settings.Instance.MusicVolume = Mathf.Max(0.0f, Settings.Instance.MusicVolume - 0.1f);
    }

    public void IncreaseMusicVolume()
    {
        Settings.Instance.MusicVolume = Mathf.Min(1.0f, Settings.Instance.MusicVolume + 0.1f);
    }
    
    public void DecreasePlayerVolume()
    {
        Settings.Instance.GeneralVolume = Mathf.Max(0.0f, Settings.Instance.GeneralVolume - 0.1f);
    }

    public void IncreasePlayerVolume()
    {
        Settings.Instance.GeneralVolume = Mathf.Min(1.0f, Settings.Instance.GeneralVolume + 0.1f);
    }
}
