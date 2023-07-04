using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Image classImage;
    [SerializeField] private Image factionImage;

    [SerializeField] private Image cantripImage;
    [SerializeField] private Color32 elementColor;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip transferSFX;

    private CardSummaryBox cardSummaryBox;

    private GuildRoster guildRoster;
    private Card card;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardSummaryBox != null) cardSummaryBox.Display(card, elementColor);
        Image image = GetComponent<Image>();
        image.color = new Color32(70, 70, 70, 255);
       // guildRoster.PlaySound(hoverSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardSummaryBox != null) cardSummaryBox.Hide();
        Image image = GetComponent<Image>();
        image.color = elementColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        if (guildRoster.GetEditableStatus()){
            guildRoster.PlaySound(transferSFX);
            guildRoster.Transfer(data);
        }
        */
    }

    public void GetData(Card card, GuildRoster guildRoster)
    {
        animator = GetComponent<Animator>();
        this.guildRoster = guildRoster;
        this.card = card;

        if (card.IsUnitType())
        {
            classImage.sprite = card.classIcon;
            string rankText = card.rank;
            rank.SetText(rankText);

            if (rankText == "D") elementColor = Color.grey;
            if (rankText == "C") elementColor = new Color32(139, 35, 136, 255);
            if (rankText == "B") elementColor = new Color32(73, 156, 255, 255);
            if (rankText == "A") elementColor = new Color32(205, 75, 0, 255);
        }
        else if (card.IsCantripType())
        {
            //Show cantrip type items
            cantripImage.gameObject.SetActive(true);

            //Hide unit type items
            classImage.gameObject.SetActive(false);
            rank.gameObject.SetActive(false);
            elementColor = new Color32(60, 205, 0, 255);
        }

        Image image = GetComponent<Image>();
        image.color = elementColor;

        cardName.SetText(card.cardName);


    }

    public void Enter()
    {
        animator.SetTrigger("Enter");
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void SetCardSummaryBox(CardSummaryBox cardSummaryBox)
    {
        this.cardSummaryBox = cardSummaryBox;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
