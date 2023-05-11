using Assets.Scripts.Interface;
using System;
using System.Collections;
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
        END
    }

    private int selectedCardsCount = 0;

    public Action<int> OnCardDeselected;
    public Action OnCombatStarted;

    private Phase phase = Phase.INTRO;
    private bool gameNotStarted = true;

    public Deck playerDeckBase;
    private Deck enemyDeckBase;

    private DeckDataStore playerDeck;
    private DeckDataStore enemyDeck;

    public TextMeshProUGUI playPromptText;
    public RouteMap route;
    public StageIntro stageIntro;

    public ManagerCombatUI combatManager;
    public UnitManager unitManager;
    public PlayerHandPanel playerHand;
    public PlayerHandPanel enemyHand;
    public CenterPrompt centerPrompt;
    public CameraControl playerCamera;
    public StartDeploymentButton startDeploymentButton;
    public StartCombatButton startCombatButton;

    public Timer timer;
    [SerializeField] private TraitBuffsList playerTraitBuffs;
    [SerializeField] private TraitBuffsList enemyTraitBuffs;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioPlayStart;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && gameNotStarted)
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
        
        stageIntro.gameObject.SetActive(false);
        playerHand.gameObject.SetActive(true);
        SetPhase("CARDSELECT");
        playerHand.DrawStartingHand();
        playerCamera.UnFocus();
        //End intro, start game
        // combatManager.StartStage(playerDeck, enemyDeck);
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
        if (phase == Phase.END) return "END";

        return "Unknown State";
    }

    public void SetPhase(string phase)
    {
        if (phase == "INTRO") this.phase = Phase.INTRO;
        if (phase == "CARDSELECT")
        {
            this.phase = Phase.CARDSELECT;
            centerPrompt.DisplayPrompt();
            startDeploymentButton.gameObject.SetActive(true);
        }
        if (phase == "DEPLOYMENT")
        {
            this.phase = Phase.DEPLOYMENT;
            centerPrompt.ResetText();
            playerCamera.UnFocus();
            playerHand.gameObject.SetActive(false);
            selectedCardsCount = 0; //reset order

            //Start Unit Deployment
            unitManager.DeployQueuedUnits(playerHand.GetDeployableUnits(), true);
        }
        if(phase == "ENEMYCARDSELECT")
        {
            this.phase = Phase.ENEMYCARDSELECT;
            enemyHand.gameObject.SetActive(true);
            enemyHand.DrawStartingHand();
            enemyHand.CPUSelectCards();
            playerCamera.UnFocus();
        }
        if (phase == "ENEMYDEPLOYMENT")
        {
            this.phase = Phase.ENEMYDEPLOYMENT;
            playerCamera.UnFocus();
            enemyHand.gameObject.SetActive(false);
            selectedCardsCount = 0;

            unitManager.DeployQueuedUnits(enemyHand.GetDeployableUnits(), false);
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
            timer.StartTimer();
            OnCombatStarted?.Invoke();

        }
        if (phase == "END") this.phase = Phase.END;
    }

    public int IncCardSelectOrder()
    {
        selectedCardsCount++;
        return selectedCardsCount;
    }
    public int GetCardSelectOrder()
    {
        return selectedCardsCount;
    }
    public void NotifyCardDeselected(int ID)
    {
        selectedCardsCount--;
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
            enemyTraitBuffs.AddTraitsFrom(controller.data, controller.teamNum);
        }
        else if(controller.teamNum == 1){
            playerTraitBuffs.AddTraitsFrom(controller.data, controller.teamNum);
        }
        else
        {
            Debug.Log("invalid team number " + controller.teamNum);
        }
    }
}
