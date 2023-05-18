using Assets.Scripts.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public int playerHealth;
    public int cpuHealth;
    public HeartBar playerHearts;
    public HeartBar cpuHearts;

    public int playerSelectable = 3;
    public int cpuSelectable = 3;
    private int defaultPlayerSelectable;
    private int defaultCpuSelectable;

    //for counting selected cards
    private int selectedCardsCount = 0;

    public Action<int> OnCardDeselected;
    public Action OnCombatStarted;
    public Action OnCombatEnded;

    private Phase phase = Phase.INTRO;
    private bool gameNotStarted = true;

    public Deck playerDeckBase;
    private Deck enemyDeckBase;

    private DeckDataStore playerDeck;
    private DeckDataStore enemyDeck;

    public TextMeshProUGUI playPromptText;
    public RouteMap route;
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

    public RoundIndicator roundIndicator;

    public CameraControl playerCamera;
    public StartDeploymentButton startDeploymentButton;
    public StartCombatButton startCombatButton;

    public Timer timer;
    [SerializeField] private TraitBuffsList playerTraitBuffs;
    [SerializeField] private TraitBuffsList enemyTraitBuffs;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioPlayStart;
    [SerializeField] private AudioClip AudioSortie;

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
        playerTraitBuffs.team = 1;
        enemyTraitBuffs.team = -1;
        playerHearts.SetHearts(playerHealth);
        playerHearts.HealMax();
        cpuHearts.SetHearts(cpuHealth);
        cpuHearts.HealMax();
        defaultPlayerSelectable = playerSelectable;
        defaultCpuSelectable = cpuSelectable;

        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && gameNotStarted)
        {
            gameNotStarted = false;

            AudioPlayer.PlayOneShot(AudioPlayStart);
            route.Initialize(this);
            
            //Turn off the prompt text
            playPromptText.gameObject.SetActive(false);

            //Game Start
            route.AdvanceRoute();

            //Deck is available now to use by monobehaviours
            playerDeck = new DeckDataStore(playerDeckBase); //Preset currently in editor
            enemyDeck = new DeckDataStore(enemyDeckBase); //Obtained from stage

            stageIntro.Init(playerDeck, enemyDeck);
            playerHand.Init(playerDeck);
            enemyHand.Init(enemyDeck);

            StartCoroutine(DisplayIntroduction());
        }
    }

    IEnumerator DisplayIntroduction()
    {
        yield return new WaitForSeconds(1);

        route.gameObject.SetActive(false);
        stageIntro.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        //Player Card Selection phase
        SetPhase("CARDSELECT");
        stageIntro.gameObject.SetActive(false);


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
        if (phase == "INTRO") this.phase = Phase.INTRO;
        if (phase == "CARDSELECT")
        {
            roundIndicator.Init();
            this.phase = Phase.CARDSELECT;
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
            unitManager.DeployQueuedUnits(playerHand.PlayCards(), true);
            playerHand.RearrangeCards();
            playerHand.gameObject.SetActive(false);
        }
        if(phase == "ENEMYCARDSELECT")
        {
            this.phase = Phase.ENEMYCARDSELECT;
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
            unitManager.DeployQueuedUnits(enemyHand.PlayCards(), false);
            enemyHand.RearrangeCards();
            enemyHand.gameObject.SetActive(false);
        }

        if (phase == "REPOSITIONING")
        {
            startCombatButton.gameObject.SetActive(true);
            this.phase = Phase.REPOSITIONING;
            combatManager.ClearSelectionKeepWindow();
            playerTraitBuffs.ApplyTraitBuffs();
            enemyTraitBuffs.ApplyTraitBuffs();
            playerCamera.UnFocus();
            //Skipping repositioning for now

            //SetPhase("COMBAT");
        }
        if (phase == "COMBAT")
        {
            this.phase = Phase.COMBAT;
            StartCoroutine(BeginCombat());
        }
        if (phase == "ENDCOMBAT")
        {
            this.phase = Phase.ENDCOMBAT;
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
        if (playerHealth <= 0)
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
        if (selectedCardsCount > 0 && GetPhase() == "CARDSELECT") startDeploymentButton.Display();
        return selectedCardsCount;
    }
    public int GetCardSelectOrder()
    {
        return selectedCardsCount;
    }
    public void NotifyCardDeselected(int ID)
    {
        selectedCardsCount--;
        if (selectedCardsCount <= 0 && GetPhase() == "CARDSELECT") startDeploymentButton.Hide();
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
    public void TakeDamage(bool isPlayer, int damage)
    {
        if (isPlayer)
        {
            playerHearts.TakeDamage(1);
            cpuSelectable = defaultCpuSelectable; // Reset back to default
            playerSelectable++;

            ValidatePlayerSelectable();

            playerHand.UpdateSelectableAmount(true, playerSelectable);
            enemyHand.UpdateSelectableAmount(false, cpuSelectable);
        }
        else
        {
            cpuHearts.TakeDamage(1);
            playerSelectable = defaultPlayerSelectable;
            cpuSelectable++;

            ValidatePlayerSelectable();

            playerHand.UpdateSelectableAmount(true, playerSelectable);
            enemyHand.UpdateSelectableAmount(false, cpuSelectable);
        }
    }

    
    void ValidatePlayerSelectable()
    {
        if (playerSelectable > 5) playerSelectable = 5;
        if (playerSelectable <= 0) playerSelectable = 1;
        if (cpuSelectable > 5) cpuSelectable = 5;
        if (cpuSelectable <= 0) cpuSelectable = 1;
    }

    public void ResetPlayerSelectable()
    {
        playerSelectable = defaultPlayerSelectable;
        cpuSelectable = defaultCpuSelectable;
        playerHand.UpdateSelectableAmount(true, playerSelectable);
        enemyHand.UpdateSelectableAmount(false, cpuSelectable);
    }

    public void UpdateHealth(bool isPlayerHealth, int health)
    {
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

    public void NextStage()
    {

    }
    public void RestartGame()
    {

    }

}
