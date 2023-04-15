using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip BGM;

    // Start is called before the first frame update
    void Start()
    {
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        AudioPlayer.clip = BGM;
        AudioPlayer.Play();
    }
}
