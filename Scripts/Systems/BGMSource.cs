using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSource : MonoBehaviour
{
    [SerializeField] private AudioSource AudioPlayer; //Always the gameobject attached to this
    [SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip BGM2;
    [SerializeField] private float baseVolumeLevel;

    public int bgmSelect = 1;
    public float startingTime = 13.21f;

    void Start()
    {
        GameSettings.Instance.OnSettingsChanged += UpdateSoundSettings;
        PlayCombatBGM(bgmSelect, startingTime);
    }


    /*
     * Adjust the sound level
     */
    void UpdateSoundSettings()
    {
        int masterVolume = GameSettings.Instance.GetMasterVolume();
        int bgmVolume = GameSettings.Instance.GetBGMVolume();

        float currentVolume = baseVolumeLevel;

        currentVolume = currentVolume * ConvertToRatio(bgmVolume);
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


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCombatBGM(int songNum, float startTime)
    {
        if(songNum == 1) AudioPlayer.clip = BGM;
        else if(songNum == 2) AudioPlayer.clip = BGM2;
        AudioPlayer.time = startTime;
        AudioPlayer.Play();
    }
}
