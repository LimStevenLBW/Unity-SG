using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool introControl = false;

    public GameObject titleText;
    public GameObject promptText;
    public GameObject studioText;
    public GameObject menuPanel;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioSelect;
    [SerializeField] private AudioClip AudioHover;
    // Start is called before the first frame update

    void Awake()
    {
        menuPanel.SetActive(false);
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
        menuPanel.SetActive(true);
        AudioSource.PlayOneShot(AudioSelect);
    }


}
