using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSource : MonoBehaviour
{
    [SerializeField] private AudioSource AudioPlayer; //Always the gameobject attached to this
    [SerializeField] private float baseVolumeLevel;
    // Start is called before the first frame update
    void Start()
    {
        GameSettings.Instance.OnSettingsChanged += UpdateSoundSettings;
    }

  
    /*
     * Adjust the sound level
     */
    void UpdateSoundSettings()
    {
        int masterVolume = GameSettings.Instance.GetMasterVolume();
        int sfxVolume = GameSettings.Instance.GetSFXVolume();

        float currentVolume = baseVolumeLevel;

        currentVolume = currentVolume * ConvertToRatio(sfxVolume);
        currentVolume = currentVolume * ConvertToRatio(masterVolume);

        AudioPlayer.volume = currentVolume;
    }

    float ConvertToRatio(int volume)
    {
        if (volume == 0) return 0;
        else if (volume == 1) return 0.33f;
        else if (volume == 2) return 0.66f;
        else if (volume == 3) return 1f;
        else if (volume == 4) return 1.33f;
        else if (volume == 5) return 1.66f;
        else if (volume == 6) return 2f;
        return 1f;
    }
}
