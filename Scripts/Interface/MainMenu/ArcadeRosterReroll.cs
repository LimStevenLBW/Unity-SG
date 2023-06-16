using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArcadeRosterReroll : UIButton
{
    private bool isOnCooldown = false;
    private Animator animator;
    private MainMenu mainMenu;
    private Image image;
    private Color storedColor;

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
        image = GetComponent<Image>();
        storedColor = image.color;
        animator = GetComponent<Animator>();
    }
    public override void OnPointerClick(PointerEventData e)
    {
        if (!isOnCooldown)
        {        
            //Call Super to play attached audio clip
            base.OnPointerClick(e);
            image.color = Color.grey;
            isOnCooldown = true;
            StopAllCoroutines();
            StartCoroutine(Rerolling());
        }
 
        mainMenu.RerollGuildRoster();

    }

    
    IEnumerator Rerolling()
    {
        yield return new WaitForSeconds(1);
        image.color = storedColor;
        isOnCooldown = false;

    }
}
