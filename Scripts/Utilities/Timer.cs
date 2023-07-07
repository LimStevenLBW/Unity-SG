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
    public int fightTimer;

    private int clock;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Director.Instance.OnCombatEnded += EndTimer;
    }

    public void StartTimer()
    {
        clock = fightTimer;
        clockText.SetText(clock.ToString());
        clockText.color = Color.white;

        StartCoroutine(Tick());
        
    }

    public void EndTimer()
    {
        StopAllCoroutines();
        clockText.SetText("");
    }

    private IEnumerator Tick()
    {
        float volumeScale = 1f;
        clockText.color = Color.white;
        while (clock > 0)
        {
            yield return new WaitForSeconds(1);
            OnSecondPassed?.Invoke();
            Director.Instance.AddToTimer(1);
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
        }

        //Out of time

        yield return new WaitForSeconds(0.5f);
        clockText.SetText("");
        Director.Instance.SetPhase("ENDCOMBAT");
    }
}
