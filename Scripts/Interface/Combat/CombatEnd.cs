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
    public TransitionBlack transition;
    //private bool isInputAllowed = false;
    //private bool restartOnInput = false;
   // private bool nextStageOnInput = false;


    public void SetDamageDone(float damageDone) {
        this.damageDone = damageDone;
    }
    public void SetTimeElapsed(float timeElapsed) {
        this.timeElapsed = timeElapsed;
        timeTotal.SetText(timeElapsed + " Seconds Elapsed");
    }
    public void DisplayGameOver() {
        gameObject.SetActive(true);
        gameover.gameObject.SetActive(true);
        StartCoroutine(Restart());
    }
    public void DisplayVictory()
    {
        gameObject.SetActive(true);
        victory.gameObject.SetActive(true);
        StartCoroutine(NextStage());
    }

    IEnumerator Restart()
    {
        //Delay before next stage starts
        yield return new WaitForSeconds(4.0f);
        transition.Enter();

        yield return new WaitForSeconds(2f);

        Director.Instance.RestartGame();
    }

    IEnumerator NextStage()
    {
        //Delay before input is allowed
        yield return new WaitForSeconds(1.0f);

        //Delay before next stage starts
        yield return new WaitForSeconds(1.0f);
        transition.Enter();
        Director.Instance.ClearStage();
        yield return new WaitForSeconds(2f);

        //Close the combat screen
        victory.gameObject.SetActive(false);
        timeElapsed = 0;
        timeTotal.SetText("");

        //TEMP///////
        Director.Instance.tempCurrentStageID++;

        if (Director.Instance.tempCurrentStageID > 3)
        {
            Director.Instance.EndGame();

        }
        else
        {
            Director.Instance.ChangeWeather(Director.Instance.tempCurrentStageID);
        }
        //TEMP////////

        transition.Exit();

        yield return new WaitForSeconds(0.6f);

        transition.gameObject.SetActive(false);
        //Start the next stage
        Director.Instance.NextStage();
        gameObject.SetActive(false);

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
