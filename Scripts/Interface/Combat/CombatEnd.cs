using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatEnd : MonoBehaviour
{
    private float damageDone;
    private float timeElapsed;
    public TextMeshProUGUI victory;
    public TextMeshProUGUI gameover;
    public TextMeshProUGUI damageTotal;
    public TextMeshProUGUI timeTotal;
    private bool isInputAllowed = false;
    private bool restartOnInput = false;
    private bool nextStageOnInput = false;


    public void SetDamageDone(float damageDone) {
        this.damageDone = damageDone;
    }
    public void SetTimeElapsed(float timeElapsed) {
        this.timeElapsed = timeElapsed;
        timeTotal.SetText(timeElapsed + " Seconds");
    }
    public void DisplayGameOver() {
        isInputAllowed = false;
        restartOnInput = true;
        gameObject.SetActive(true);
        gameover.gameObject.SetActive(true);
        StartCoroutine(Restart());
    }
    public void DisplayVictory()
    {
        isInputAllowed = false;
        nextStageOnInput = true;
        gameObject.SetActive(true);
        victory.gameObject.SetActive(true);
        StartCoroutine(NextStage());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1.5f);
        isInputAllowed = true;

        yield return new WaitForSeconds(5.0f);
        Director.Instance.RestartGame();
    }

    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(1.5f);
        isInputAllowed = true;


        yield return new WaitForSeconds(5.0f);
        Director.Instance.NextStage();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        gameover.gameObject.SetActive(false);
        victory.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
