using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip BGM2;

    public int bgmSelect = 1;
    public float startingTime = 13.21f;
    // Start is called before the first frame update
    void Start()
    {
        PlayCombatBGM(bgmSelect, startingTime);
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
