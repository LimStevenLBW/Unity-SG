using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardNum;
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioHover;
    [SerializeField] private AudioClip AudioClick;
    [SerializeField] private AudioClip AudioAppear;


    void OnEnable()
    {
        AudioPlayer.PlayOneShot(AudioAppear);
    }

    public void Init()
    {
       
        
    }

    // Start is called before the first frame update
    void OnStart()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
