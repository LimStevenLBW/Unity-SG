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
    public MenuReturnButton menuReturnButton;
    [SerializeField] private ArcadeStartButton arcadeStartButton;
    [SerializeField] private GameObject arcadePanel;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private CameraControl mainCamera;
    [SerializeField] private GameObject arcadeCameraView;
    [SerializeField] private GameObject settingCameraView;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioSelect;
    [SerializeField] private TransitionBlack transition;
    // Start is called before the first frame update

    void Awake()
    {
        arcadePanel.SetActive(false);
        arcadeStartButton.Init(this);
        settingsMenu.Hide();
        menuPanel.Init(this);

        menuReturnButton.Init(this);
        titleText.SetActive(false);
        promptText.SetActive(false);
        studioText.SetActive(false);
        
    }
    void Start()
    {
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
        mainCamera.Focus(arcadeCameraView.transform);

       // StartCoroutine(ReEnableControl());
    }

    public void StartArcadeMode()
    {
        StartCoroutine(Transition());
    }
    
    IEnumerator Transition()
    {
        transition.Enter();
        yield return new WaitForSeconds(1.5f);
        transition.Exit();
        SceneManager.LoadScene("Combat");
    }


    public void ShowSettingsMenu()
    {
        menuPanel.Hide();
        menuReturnButton.Show();
        settingsMenu.Show();
        mainCamera.DisableAllControl();
        mainCamera.Focus(settingCameraView.transform);

       // StartCoroutine(ReEnableControl());
    }

    public void ReturnToMainMenu()
    {
        arcadePanel.SetActive(false);
        arcadeStartButton.Hide();

        //Save settings
        if (settingsMenu.isActiveAndEnabled)
        {
            settingsMenu.Hide();
            GameSettings.Instance.Save();
        }

        menuReturnButton.Hide();
        menuPanel.Show();

        mainCamera.ResetPosition();
    }

}
