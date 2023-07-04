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

    public CardDropScreen cardDropScreen;

    public GuildRoster guildRoster;
    public GuildRoster guildRosterBench;
    private bool isInputAllowed = false;

    public NextStageButton nextStageButton;

    //We dont use the same playerDeck as combat and director
    private DeckDataStore playerDeck;

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

        StartCoroutine(CloseVictoryScreen());
    }

    IEnumerator Restart()
    {
        //Delay before next stage starts
        yield return new WaitForSeconds(4.0f);
        transition.Enter();

        yield return new WaitForSeconds(2f);

        Director.Instance.RestartGame();
    }

    IEnumerator CloseVictoryScreen()
    {
        //Delay before closing the screen
        yield return new WaitForSeconds(2.5f);

        //Close the victory screen
        victory.gameObject.SetActive(false);
        timeElapsed = 0;
        timeTotal.SetText("");

        //TEMPORARY WAY OF CHECKING STAGES///////
       
        if (Director.Instance.tempCurrentStageID > 4) //End Game
        {
            Director.Instance.EndGame();
        }
        else
        {
            DisplayDrops();
            Director.Instance.tempCurrentStageID++;
        }
    }

    public void DisplayDrops()
    {
        StartCoroutine(DisplayDropScreen());
    }

    IEnumerator DisplayDropScreen()
    {
        Director.Instance.ClearUnitWindow();
        cardDropScreen.gameObject.SetActive(true);
        cardDropScreen.GenerateRandomDrops();

        yield return new WaitForSeconds(0.5f);

        //Input enabled
        isInputAllowed = true;
    }

    /*
     * 
     */
    void ShowRosterEdit()
    {
        Deck playerDeckBase = GamePersistentData.Instance.GetArcadeDeck();
        //We create a fresh copy
        playerDeck = new DeckDataStore(playerDeckBase);

        //Add drops to the playerdeck
        List<Unit> drops = cardDropScreen.GetDrops();
        playerDeck.AddDrops(drops);

        //Display Guild Rosters
        guildRoster.Show();
        guildRosterBench.Show();
        guildRoster.Init(playerDeck.unitList);
        //guildRosterBench.Init(playerDeck.benchedUnitList);
    }

    public void NextStage()
    {
        if (guildRoster.GetCount() == 25) StartCoroutine(NextStageTransition());
        else
        {
            guildRoster.DisplayCountError();
            nextStageButton.Show();
        }
    }

    IEnumerator NextStageTransition()
    {
        nextStageButton.Hide();
        Director.Instance.ChangeSong();
        transition.Enter();
        yield return new WaitForSeconds(2f);

        //SAVE GUILD ROSTER CHANGES
        GamePersistentData.Instance.SetArcadeDeck(playerDeck);

        Director.Instance.ClearStage();

        guildRoster.Hide();
        guildRosterBench.Hide();
        Director.Instance.ChangeWeather();

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
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && isInputAllowed)
        {
            cardDropScreen.gameObject.SetActive(false);
            ShowRosterEdit();
            nextStageButton.Show();
            isInputAllowed = false;
        }
        
    }
}
