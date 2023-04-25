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
        COMBAT,
        END
    }

    private int selectedCardsCount = 0;

    public Action<int> OnCardDeselected;

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
    public PlayerHandPanel playerHand;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioHover;
    [SerializeField] private AudioClip AudioPlayStart;
    [SerializeField] private AudioClip AudioClickClose;

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
        if (Input.GetKeyDown(KeyCode.Return) && gameNotStarted)
        {
            gameNotStarted = false;

            AudioPlayer.PlayOneShot(AudioPlayStart);
            route.Initialize(this);
            
            //Turn off the prompt text
            playPromptText.gameObject.SetActive(false);

            //Game Start
            route.AdvanceRoute();

            //Deck is available now
            playerDeck = new DeckDataStore(playerDeckBase); //Preset currently
            enemyDeck = new DeckDataStore(enemyDeckBase); //Obtained from stage

            stageIntro.Init(playerDeck, enemyDeck);
            playerHand.Init(playerDeck, enemyDeck);

            StartCoroutine(DisplayIntroduction());
        }
    }

    IEnumerator DisplayIntroduction()
    {
        yield return new WaitForSeconds(1);

        route.gameObject.SetActive(false);
        stageIntro.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        stageIntro.gameObject.SetActive(false);
        playerHand.gameObject.SetActive(true);

        phase = Phase.CARDSELECT;
        //End intro, start game
       // combatManager.StartStage(playerDeck, enemyDeck);
    }

    public string GetPhase()
    {
        if (phase == Phase.INTRO) return "INTRO";
        if (phase == Phase.CARDSELECT) return "CARDSELECT";
        if (phase == Phase.DEPLOYMENT) return "DEPLOYMENT";
        if (phase == Phase.COMBAT) return "COMBAT";
        if (phase == Phase.END) return "END";

        return "Unknown State";
    }

    public int GetCardSelectOrder()
    {
        selectedCardsCount++;
        return selectedCardsCount;
    }

    public void NotifyCardDeselected(int ID)
    {
        selectedCardsCount--;
        OnCardDeselected?.Invoke(ID);
    }
}
