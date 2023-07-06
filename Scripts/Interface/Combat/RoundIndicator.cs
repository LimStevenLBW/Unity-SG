using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundIndicator : MonoBehaviour
{
    private int round = 1;

    public TextMeshProUGUI roundText;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Init()
    {
        gameObject.SetActive(true);
        round = 1;
    }

    public void NextRound()
    {
        round++;
        roundText.SetText("Round " + round);
    }

    public int GetRound()
    {
        return round;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        round = 1;
    }
}
