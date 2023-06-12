using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Settings Menu from main menu
 */
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;

    // Start is called before the first frame update
    void Awake()
    {
        GameSettings.Instance.OnSettingsChanged += UpdateSettingsDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateSettingsDisplay()
    {
        masterVolume.value = GameSettings.Instance.GetMasterVolume();
        bgmVolume.value = GameSettings.Instance.GetBGMVolume();
        sfxVolume.value = GameSettings.Instance.GetSFXVolume();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
