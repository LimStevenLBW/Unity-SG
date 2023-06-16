using Assets.Scripts.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    enum Phase
    {
        INTRO,
        CARDSELECT,
        DEPLOYMENT,
        ENEMYCARDSELECT,
        ENEMYDEPLOYMENT,
        REPOSITIONING,
        COMBAT,
        ENDCOMBAT,
        CONCLUSION
    }
    //Temporary until stages are reworked
    public int tempNumberofStages = 3;
    public int tempCurrentStageID = 0;

    private int playerHealth;
    private int cpuHealth;
    public int defaultPlayerHealth;
    public int defaultCpuHealth;
    public HeartBar playerHearts;
    public HeartBar cpuHearts;

    private int playerSelectable = 3;
    private int cpuSelectable = 3;
    public int defaultPlayerSelectable;
    public int defaultCpuSelectable;

    //for counting selected cards
    private int selectedCardsCount = 0;

    public Action<int> OnCardDeselected;
    public Action OnCombatStarted;
    public Action OnCombatEnded;

    private Phase phase = Phase.INTRO;
    private bool gameStarted = false;

    private Deck playerDeckBase;
    private Deck enemyDeckBase;

    private DeckDataStore playerDeck;
    private DeckDataStore enemyDeck;

    public TextMeshProUGUI playPromptText;
    public RouteMap route;
    public int stageNum;
    public StageIntro stageIntro;
    public CombatEnd combatEndScreen;
    private float damageDone;
    private float timeElapsed;

    public ManagerCombatUI combatManager;
    public UnitManager unitManager;
    public PlayerHandPanel playerHand;
    public PlayerHandPanel enemyHand;
    public CenterPrompt centerPrompt;
    public GameObject sortiePrompt;
    public GameObject repositioningPrompt;

    public RoundIndicator roundIndicator;

    public CameraControl playerCamera;
    public StartDeploymentButton startDeploymentButton;
    public StartCombatButton startCombatButton;

    //Weather
    public GameObject rainDropSystem;
    public GameObject rainy;
    public GameObject sunset;
    public GameObject night;

    public Timer timer;
    [SerializeField] private TraitBuffsList playerTraitBuffs;
    [SerializeField] private TraitBuffsList enemyTraitBuffs;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioPlayStart;
    [SerializeField] private AudioClip AudioSortie;

    [SerializeField] private bool playerTookDamage;
    [SerializeField] private bool cpuTookDamage;

    public static Director Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /*
     * Acquire preset enemy deck and store it
     */
    public void GetEnemyDeck(Deck deck)
    {
        enemyDeckBase = deck;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && !gameStarted)
        {
            StartGame();  
        }
    }

    void StartGame()
    {
        tempCurrentStageID = 0;
        tempCurrentStageID++;

        //Setup player hearts
        playerTraitBuffs.team = 1;
        enemyTraitBuffs.team = -1;

        gameStarted = true;

        AudioPlayer.PlayOneShot(AudioPlayStart);
        route.Initialize(this);

        //Turn off the prompt text
        playPromptText.gameObject.SetActive(false);

        //Game Start
        route.DisplayRoute();
        route.AdvanceRoute();
        

        InitStageData();
    }

    void ResetHealth()
    {
        playerHearts.SetHearts(defaultPlayerHealth);
        playerHearts.HealMax();
        cpuHearts.SetHearts(defaultCpuHealth);
        cpuHearts.HealMax();
    }

    void InitStageData()
    {
        playerDeckBase = GamePersistentData.Instance.GetPlayerDeck();
        //Deck is available now to use by monobehaviours
        playerDeck = new DeckDataStore(playerDeckBase); //from Guild roster
        playerDeck.Shuffle();
        playerDeck.UpdateTroopCount();

        enemyDeck = new DeckDataStore(enemyDeckBase); //Obtained from stage
        enemyDeck.Shuffle();
        enemyDeck.UpdateTroopCount();

        stageIntro.Init(playerDeck, enemyDeck);

        //Set selectable counts
        playerSelectable = defaultPlayerSelectable;
        cpuSelectable = defaultCpuSelectable;

        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);

        playerHand.Init(playerDeck);
        enemyHand.Init(enemyDeck);

        //Health set
        playerHealth = defaultPlayerHealth;
        cpuHealth = defaultCpuHealth;

        SetPhase("INTRO");
    }

    public string GetPhase()
    {
        if (phase == Phase.INTRO) return "INTRO";
        if (phase == Phase.CARDSELECT) return "CARDSELECT";
        if (phase == Phase.DEPLOYMENT) return "DEPLOYMENT";
        if (phase == Phase.ENEMYCARDSELECT) return "ENEMYCARDSELECT";
        if (phase == Phase.ENEMYDEPLOYMENT) return "ENEMYDEPLOYMENT";
        if (phase == Phase.REPOSITIONING) return "REPOSITIONING";
        if (phase == Phase.COMBAT) return "COMBAT";
        if (phase == Phase.ENDCOMBAT) return "END";
        if (phase == Phase.CONCLUSION) return "CONCLUSION";
        return "Unknown State";
    }

    public void SetPhase(string phase)
    {
        if (phase == "INTRO")
        {
            this.phase = Phase.INTRO;
            StartCoroutine(DisplayIntroduction());
        }
        if (phase == "CARDSELECT")
        {
            this.phase = Phase.CARDSELECT;

            playerTookDamage = false;
            cpuTookDamage = false;

            roundIndicator.Init();
            centerPrompt.DisplayPrompt(playerSelectable);
            playerHand.gameObject.SetActive(true);
            playerHand.FillHand();
            playerCamera.UnFocus();
            unitManager.ResetUnitPositions();
        }
        if (phase == "DEPLOYMENT")
        {
            this.phase = Phase.DEPLOYMENT;
            centerPrompt.ResetText();
            playerCamera.UnFocus();

            selectedCardsCount = 0; //reset order

            //Start Unit Deployment
            unitManager.ClearActiveTraitBuffs(1); //Clear allied buffs
            unitManager.DeployQueuedUnits(playerHand.PlayCards(), true);
            playerHand.RearrangeCards();
            playerHand.gameObject.SetActive(false);
        }
        if(phase == "ENEMYCARDSELECT")
        {
            this.phase = Phase.ENEMYCARDSELECT;
            unitManager.GetActiveTraitBuffs(playerTraitBuffs); // Get buffs to allies after deployment
            unitManager.ApplyActiveTraitBuffs(1); //Apply allied buffs

            enemyHand.gameObject.SetActive(true);
            enemyHand.FillHand();
            enemyHand.CPUSelectCards();
            playerCamera.UnFocus();
        }
        if (phase == "ENEMYDEPLOYMENT")
        {
            this.phase = Phase.ENEMYDEPLOYMENT;
            playerCamera.UnFocus();
            selectedCardsCount = 0;
            unitManager.ClearActiveTraitBuffs(-1); //Clear enemy buffs
            unitManager.DeployQueuedUnits(enemyHand.PlayCards(), false);
            enemyHand.RearrangeCards();
            enemyHand.gameObject.SetActive(false);
        }

        if (phase == "REPOSITIONING")
        {
            this.phase = Phase.REPOSITIONING;
            unitManager.GetActiveTraitBuffs(enemyTraitBuffs); //Apply buffs to enemies after deployment
            unitManager.ApplyActiveTraitBuffs(-1); //Apply enemy buffs
            unitManager.RepositionEnemyUnits();

            repositioningPrompt.gameObject.SetActive(true);

            combatManager.ClearSelectionKeepWindow();
            playerCamera.UnFocus();
        }
        if (phase == "COMBAT")
        {
            repositioningPrompt.gameObject.SetActive(false);
            this.phase = Phase.COMBAT;
            StartCoroutine(BeginCombat());
        }
        if (phase == "ENDCOMBAT")
        {
            this.phase = Phase.ENDCOMBAT;
            unitManager.ApplyActiveTraitBuffsOnCombatEnd();


            AudioPlayer.PlayOneShot(AudioSortie);
            RecalculateControllerTraits();
            OnCombatEnded?.Invoke();

            StartCoroutine(EndCombat());
        }
        if (phase == "CONCLUSION")
        {
            this.phase = Phase.CONCLUSION;
            AudioPlayer.PlayOneShot(AudioSortie);
            RecalculateControllerTraits();
            OnCombatEnded?.Invoke();

            playerCamera.UnFocus();
        }
    }
    IEnumerator DisplayIntroduction()
    {
        yield return new WaitForSeconds(2.5f);

        route.HideRoute();
        stageIntro.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        combatManager.DisplayHeader();
        ResetHealth();

        stageIntro.gameObject.SetActive(false);

        //Player Card Selection phase
        SetPhase("CARDSELECT");

    }

    IEnumerator BeginCombat()
    {
        sortiePrompt.SetActive(true);
        AudioPlayer.PlayOneShot(AudioSortie);
        yield return new WaitForSeconds(1f);

        sortiePrompt.SetActive(false);
       
        timer.StartTimer();
        OnCombatStarted?.Invoke();
    }
    IEnumerator EndCombat()
    {
        yield return new WaitForSeconds(1.5f);
        unitManager.RefreshStamina();
        UpdatePlayerSelectable();

        if(playerHealth == 0 && cpuHealth == 0)
        {
            //win on draw game currently
            SetPhase("CONCLUSION");
            Victory();
        }
        else if (playerHealth <= 0)
        {
            SetPhase("CONCLUSION");
            GameOver();
        }
        else if (cpuHealth <= 0)
        {
            SetPhase("CONCLUSION");
            Victory();
        }
        else
        {
            roundIndicator.NextRound();
            SetPhase("CARDSELECT");
        }

    }

    public int IncCardSelectOrder()
    {
        selectedCardsCount++;
        if (selectedCardsCount >= playerSelectable && GetPhase() == "CARDSELECT") startDeploymentButton.Display();
        return selectedCardsCount;
    }
    public int GetCardSelectOrder()
    {
        return selectedCardsCount;
    }
    public void NotifyCardDeselected(int ID)
    {
        selectedCardsCount--;
        if (selectedCardsCount < playerSelectable && GetPhase() == "CARDSELECT") startDeploymentButton.Hide();
        OnCardDeselected?.Invoke(ID);
    }
    public void PlaySound(AudioClip clip)
    {
        AudioPlayer.PlayOneShot(clip);
    }

    public void AddControllerTraits(UnitController controller)
    {
        if(controller.teamNum == -1)
        {
            enemyTraitBuffs.GetTraitsFrom(controller.data);
        }
        else if(controller.teamNum == 1){
            playerTraitBuffs.GetTraitsFrom(controller.data);
        }
        else
        {
            Debug.Log("invalid team number " + controller.teamNum);
        }
    }

    public void RecalculateControllerTraits()
    {
        playerTraitBuffs.ClearTraitBuffs(true);
        enemyTraitBuffs.ClearTraitBuffs(true);

        List<UnitController> firstTeamControllers = unitManager.playerControllers;
        List<UnitController> secondTeamControllers = unitManager.cpuControllers;

        
        foreach (UnitController controller in firstTeamControllers)
        {
            AddControllerTraits(controller);
        }
        foreach (UnitController controller in secondTeamControllers)
        { 
            AddControllerTraits(controller);
        }
        
    }
    //A player took damage, start a
    public void TakeDamage(bool isPlayer, int damage)
    {
        if (isPlayer)
        {
            playerHearts.TakeDamage(1);
            playerTookDamage = true;   
        }
        else
        {
            cpuHearts.TakeDamage(1);
            cpuTookDamage = true;
        }
    }

    //Process how many cards a player can use next turn
    void UpdatePlayerSelectable()
    {
        //Both players ran out of units
        if (playerTookDamage && cpuTookDamage) {
            // Both players reset
            cpuSelectable = defaultCpuSelectable; 
            playerSelectable = defaultPlayerSelectable;
            ValidatePlayerSelectable();

        }
        else if (playerTookDamage) { 
            cpuSelectable = defaultCpuSelectable; // Reset back to default
            playerSelectable++;
            ValidatePlayerSelectable();
        }
        else if (cpuTookDamage)
        {
            playerSelectable = defaultPlayerSelectable;
            cpuSelectable++;
            ValidatePlayerSelectable();
        }

        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);
    }

    public void BoostPlayerSelectable()
    {
        int playerTeamSize = unitManager.playerControllers.Count;
        int cpuTeamSize = unitManager.cpuControllers.Count;
        playerSelectable = defaultPlayerSelectable;
        cpuSelectable = defaultCpuSelectable;


        if (playerTeamSize < cpuTeamSize)
        {
            int countDifference = cpuTeamSize - playerTeamSize;
            playerSelectable = countDifference + 2;
        }
        else if (cpuTeamSize < playerTeamSize)
        {
            int countDifference = playerTeamSize - cpuTeamSize;
            cpuSelectable = countDifference + 2;
        }

        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);
    }

    public void ValidatePlayerSelectable()
    {
        if (playerSelectable > 4) playerSelectable = 4;
        if (playerSelectable <= 0) playerSelectable = 1;
        if (cpuSelectable > 4) cpuSelectable = 4;
        if (cpuSelectable <= 0) cpuSelectable = 1;
    }

    public void ResetPlayerSelectable()
    {

        playerSelectable = defaultPlayerSelectable;
        cpuSelectable = defaultCpuSelectable;
        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);
    }

    //Update health after healthbars are animated, then start the end combat
    public void UpdateHealth(bool isPlayerHealth, int health)
    {
        //Set health
        if (isPlayerHealth)
        {
            playerHealth = health;
        }
        else
        {
            cpuHealth = health;
        }

        SetPhase("ENDCOMBAT");

    }

    public void AddToDamageDone(float additive)
    {
        damageDone += additive;
    }

    public void AddToTimer(float additive)
    {
        timeElapsed += additive;
    }

    public void GameOver()
    {
        SetPhase("CONCLUSION");
        combatEndScreen.SetDamageDone(damageDone);
        combatEndScreen.SetTimeElapsed(timeElapsed);
        combatEndScreen.DisplayGameOver();

        damageDone = 0;
        timeElapsed = 0;
    }
    public void Victory()
    {
        SetPhase("CONCLUSION");
        combatEndScreen.SetDamageDone(damageDone);
        combatEndScreen.SetTimeElapsed(timeElapsed);
        combatEndScreen.DisplayVictory();

        damageDone = 0;
        timeElapsed = 0;
    }

    //Clear everything
    public void ClearStage()
    {
        playerTraitBuffs.ClearTraitBuffs(true);
        enemyTraitBuffs.ClearTraitBuffs(true);
        combatManager.DisableUnitWindow();
        combatManager.HideHeader();
        unitManager.ClearField();
    }

    //Prepare next stage data
    public void NextStage()
    {
        route.DisplayRoute();
        route.AdvanceRoute();
        InitStageData();
    }

    public void RestartGame()
    {
        //unitManager.ClearField();

        SceneManager.LoadScene("Combat");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("GameEnd");
    }

    public void ChangeWeather(int stageID)
    {
        if (stageID == 2)
        {
            rainy.gameObject.SetActive(false);
            rainDropSystem.gameObject.SetActive(false);
            sunset.gameObject.SetActive(true);
        }
        else if(stageID == 3)
        {
            sunset.gameObject.SetActive(false);
            night.gameObject.SetActive(true);
        }
    }


}
