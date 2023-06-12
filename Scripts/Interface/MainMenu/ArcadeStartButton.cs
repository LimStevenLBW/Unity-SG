using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArcadeStartButton : UIButton
{
    private Animator animator;
    private MainMenu mainMenu;

    public void Init(MainMenu mainMenu)
    {
        this.mainMenu = mainMenu;
        animator = GetComponent<Animator>();
        Hide();
    }
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnPointerClick(PointerEventData e)
    {
        //Call Super to play attached audio clip
        base.OnPointerClick(e);

      //  animator.SetBool("isHovered", false);
        mainMenu.StartArcadeMode();
        gameObject.SetActive(false);
    }
}
