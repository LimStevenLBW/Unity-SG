using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuReturnButton : UIButton
{
    private Animator animator;
    private MainMenu mainMenu;

    public override void OnPointerEnter(PointerEventData e)
    {
        animator.SetBool("isHovered", true);
        //PlayAudioClip(AudioHover);
        //Debug.Log("Button Hovered");
    }
    public override void OnPointerExit(PointerEventData e)
    {
        animator.SetBool("isHovered", false);
    }

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
    }

    public override void OnPointerClick(PointerEventData e)
    {
        //Call Super to play attached audio clip
        base.OnPointerClick(e);

        Hide();
        animator.SetBool("isHovered", false);
        mainMenu.ReturnToMainMenu();

    }
}