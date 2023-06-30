using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool introControl = false;

    public GameObject titleText;
    public GameObject promptText;
    public GameObject studioText;
    public MenuPanel menuPanel;

    private bool inDifferentArea = false;
    [SerializeField] private MenuReturnButton menuReturnButton;
    [SerializeField] private ArcadeRosterStart arcadeRosterStart;
    [SerializeField] private ArcadeRosterReroll arcadeRosterReroll;
    [SerializeField] private ArcadeStartButton arcadeStartButton;
    [SerializeField] private GameObject arcadePanel;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GuildRoster guildRoster;

    [SerializeField] private CameraControl mainCamera;
    [SerializeField] private GameObject arcadeCameraView;
    [SerializeField] private GameObject settingCameraView;
    [SerializeField] private GameObject studyCameraView1;
    [SerializeField] private GameObject studyCameraView2;

    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioSelect;
    [SerializeField] private TransitionBlack transition;
    [SerializeField] DropRate dropRateTool;

    private Deck arcadeDeck;
    private int deckSize = 25;
    // Start is called before the first frame update

    void Awake()
    {
        arcadePanel.SetActive(false);
        arcadeStartButton.Init(this);
        settingsMenu.Hide();
        settingsMenu.Init();

        guildRoster.Hide();
        menuPanel.Init(this);

        arcadeRosterReroll.Init(this);
        arcadeRosterStart.Init(this);
        menuReturnButton.Init(this);

        titleText.SetActive(false);
        promptText.SetActive(false);
        studioText.SetActive(false);
        
    }
    void Start()
    {
        arcadeDeck = RandomizeArcadeDeck();
        titleText.SetActive(true);
        StartCoroutine(Introduction());
    }    
  
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && introControl) StartGame();

    }

    IEnumerator Introduction()
    {
        yield return new WaitForSeconds(1.2f);
        promptText.SetActive(true);
        introControl = true;
    }

    void StartGame()
    {
        introControl = false;
        titleText.SetActive(false);
        promptText.SetActive(false);

        studioText.SetActive(true);
        menuPanel.Show();
        AudioSource.PlayOneShot(AudioSelect);
    }

    public void ShowArcadeMenu()
    {
        arcadePanel.SetActive(true);
        arcadeStartButton.Show();
        menuPanel.Hide();
        menuReturnButton.Show();
        mainCamera.DisableAllControl();
        mainCamera.Focus(arcadeCameraView.transform, 0.3f);

       // StartCoroutine(ReEnableControl());
    }

    public void StartGuildRoster()
    {
        transition.ResetPosition();
        inDifferentArea = true;
        StartCoroutine(TransitionIntoGuildRoster());
    }
    
    IEnumerator TransitionIntoGuildRoster()
    {
        menuReturnButton.Hide();
        transition.Enter();
        yield return new WaitForSeconds(0.5f);
        arcadePanel.SetActive(false);
        mainCamera.Focus(studyCameraView1.transform, 0.3f);
        transition.Exit();
        
        yield return new WaitForSeconds(1f);
        transition.ResetPosition();

        mainCamera.Focus(studyCameraView2.transform, 0.7f);

        yield return new WaitForSeconds(0.2f);
        guildRoster.Show();
        guildRoster.Init(arcadeDeck.cardList);
        yield return new WaitForSeconds(0.2f);

        arcadeRosterStart.Show();
        arcadeRosterReroll.Show();
        menuReturnButton.Show();

    }


    public void StartArcadeMode()
    {
        StartCoroutine(TransitionIntoArcadeMode());
    }
    
    IEnumerator TransitionIntoArcadeMode()
    {
        arcadeRosterStart.Hide();
        arcadeRosterReroll.Hide();
        menuReturnButton.Hide();

        transition.Enter();
        yield return new WaitForSeconds(0.5f);
        guildRoster.Hide();

        transition.Exit();

        SceneManager.LoadScene("Combat");
        
    }
    public void ShowSettingsMenu()
    {
        menuPanel.Hide();
        menuReturnButton.Show();
        settingsMenu.Show();
        mainCamera.DisableAllControl();
        mainCamera.Focus(settingCameraView.transform, 0.3f);

       // StartCoroutine(ReEnableControl());
    }

    public void ReturnToMainMenu()
    {

        arcadePanel.SetActive(false);
        guildRoster.Hide();
        arcadeStartButton.Hide();

        //Save settings
        if (settingsMenu.isActiveAndEnabled)
        {
            settingsMenu.Hide();
            GameSettings.Instance.Save();
        }

        arcadeRosterReroll.Hide();
        arcadeRosterStart.Hide();
        menuReturnButton.Hide();
        menuPanel.Show();

        if (inDifferentArea) StartCoroutine(Transition());
        else mainCamera.ResetPosition();

    }

    public void RerollGuildRoster()
    {
        arcadeDeck = RandomizeArcadeDeck();
        guildRoster.Init(arcadeDeck.unitList);
        guildRoster.AnimateReroll();
    }

    IEnumerator Transition()
    {
        transition.Enter();
        yield return new WaitForSeconds(0.5f);
        transition.Exit();
        inDifferentArea = false;
        mainCamera.ResetPosition();
    }


    public Deck RandomizeArcadeDeck()
    {
        Deck arcadeDeck = dropRateTool.GetRandomDeck(deckSize);
        //Store new Arcade Deck
        GamePersistentData.Instance.SetArcadeDeck(arcadeDeck);

        return arcadeDeck;
        //DeckDataStore arcadeDeck = new DeckDataStore(playerDeckBase);
        //arcadeDeck.SortByClassAndRank();

    }
}
