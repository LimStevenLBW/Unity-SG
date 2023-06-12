using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private Animator animator;
    private MainMenu mainMenu;

    [SerializeField] private ArcadeButton arcadeButton;
    [SerializeField] private SettingsButton settingsButton;
    
    public void Init(MainMenu mainMenu)
    {
        this.mainMenu = mainMenu;
    }

    public void Show()
    {
        animator.SetBool("isVisible", true);
    }

    public void Hide()
    {
        animator.SetBool("isVisible", false);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        arcadeButton.Init(this);
        settingsButton.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressArcade()
    {
        mainMenu.ShowArcadeMenu();
    }

    public void PressSettings()
    {
        mainMenu.ShowSettingsMenu();
    }
}
