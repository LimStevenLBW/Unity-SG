using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Action OnSecondPassed;
    public TextMeshProUGUI clockText;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip tickClip;

    private int clock;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void StartTimer()
    {
        clock = 99;
        clockText.SetText(clock.ToString());

        StartCoroutine(Tick());
        
    }

    private IEnumerator Tick()
    {
        float volumeScale = 1f;
        clockText.color = Color.white;
        while (clock > 0)
        {
            OnSecondPassed?.Invoke();
            clock--;
            clockText.SetText(clock.ToString());
            if(clock <= 11)
            {
                volumeScale -= clock * .05f;
                audioSource.PlayOneShot(tickClip, volumeScale);
                volumeScale = 1f;
                if (clock < 11)
                {
                    clockText.color = Color.red;
                    animator.SetTrigger("TimerBouncing");
                }
            }
           

            yield return new WaitForSeconds(1);
        }
        
    }
}
