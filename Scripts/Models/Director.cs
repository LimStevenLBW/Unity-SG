using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Director : MonoBehaviour
{
    public Deck playerDeck;
    private Deck enemyDeck;

    public RouteMap route;
    public StageIntro stageIntro;
    public TextMeshProUGUI playPromptText;

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

    public void GetEnemyDeck(Deck deck)
    {
        enemyDeck = deck;
    }

    public void StartStage()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioPlayer.PlayOneShot(AudioPlayStart);
            route.Initialize(this);
            

            //Turn off the prompt text
            playPromptText.gameObject.SetActive(false);

            //Game Start
            route.AdvanceRoute();

            StartCoroutine(DisplayIntroduction());
        }
    }

    IEnumerator DisplayIntroduction()
    {
        yield return new WaitForSeconds(4);
        route.gameObject.SetActive(false);

        stageIntro.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);

        stageIntro.gameObject.SetActive(false);

    }
}
