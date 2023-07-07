using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IComparable<CardInHand>
{
    //public int cardNum; //Identify which card in the hand
    public bool isComparingByOrder = true; //When set to true, sort methods will compare this object by cardSelectOrder, other it will use cardValue
    public bool isSelected = false;
    public int cardSelectOrder = 0;
    public int cardValue = 0; //How much this card will be valued by the cpu to play, todo, currently valued at random

    public CardSelectOrder cardSelectOrderDisplay;

    public Card card { get; private set; }

    [SerializeField] private RawImage portraitFromCamera;
    [SerializeField] private Image portraitFromImage;

    public Image factionImageIcon;
    public Image specialImageIcon;
    public Image classImageIcon;

    private Image borderImage;
    private Sprite dRankBG;
    private Sprite cRankBG;
    private Sprite bRankBG;
    private Sprite aRankBG;
    private Sprite sRankBG;
    [SerializeField] private Image bgImage;
    private DetailsFooter footer;
    private PlayerHandPanel panel;

    [SerializeField] private TextMeshProUGUI rankLetter;
    [SerializeField] private ChiContainer costContainer;

    public void Init(PlayerHandPanel panel, DetailsFooter footer)
    {
        this.panel = panel;
        this.footer = footer;
    }
    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Card Art/pixelCardAssest_V01");
        borderImage = GetComponent<Image>();
        dRankBG = sprites[2];
        cRankBG = sprites[81];
        bRankBG = sprites[0];
        aRankBG = sprites[4];
        sRankBG = sprites[4];
    }


    // Start is called before the first frame update
    void Start()
    {
        Director.Instance.OnCardDeselected += UpdateSelectOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Not allowed to see enemy cards
        if (Director.Instance.GetPhase() != "ENEMYCARDSELECT")
        {
            borderImage.color = new Color32(255, 255, 255, 255);
            footer.UpdateData(card);
            panel.UpdateDescription(card);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        borderImage.color = new Color32(255, 255, 255, 0);
        if (panel != null) panel.ResetText();
        if(footer != null) footer.ResetText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Director.Instance.GetPhase() == "CARDSELECT")
        {
            if (!isSelected && card.cost <= Director.Instance.GetChiCount(true))
            {
                Select();
                //Spend the cost to select this card
                Director.Instance.SpendChi(true, card.cost);

                panel.PlayAudioCardSelected();
            }
            else if (isSelected)
            {
                Director.Instance.NotifyCardDeselected(cardSelectOrder);
                //Refund the chi used to select it earlier
                Director.Instance.GainChi(true, card.cost);
                Deselected();
                panel.PlayAudioCardDeselected();
            }
        }
    }
    
    /*
     * Acquire a card from the deck
     */
    public void DrawCard(DeckDataStore deck)
    {
        //If this card doesnt have any data, draw a card
        if (card == null)
        {
            if (!deck.IsEmpty()) card = deck.DrawCard();
        }

        if (card != null)
        {
            SetupCard();
        }
    }

    /*
     * Evaluate what to do with the card. Determines the card type
     */ 
    public void SetupCard()
    {
        gameObject.SetActive(true);
        costContainer.Clear();
        costContainer.Setup(card.cost);
        if (card.IsUnitType())
        {
            UpdateCardDataAsUnit();
        }

        card.unit.FindSkills();
    }


    public void Select()
    {
        cardSelectOrder = Director.Instance.IncCardSelectOrder();
        isSelected = true;
        
        cardSelectOrderDisplay.gameObject.SetActive(true);
        cardSelectOrderDisplay.UpdateOrder(cardSelectOrder);


        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + 25, pos.z);
    }

    public void Deselected()
    {
        cardSelectOrder = 0;

        isSelected = false;

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - 25, pos.z);

        cardSelectOrderDisplay.gameObject.SetActive(false);
    }

    public void ClearCard()
    {
        card = null;
        gameObject.SetActive(false);
    }




    //Acquire a card without drawing from the deck
    public void GetCard(UnitDataStore unitToStore)
    {
        /*
        unit = unitToStore;
   
        if (unit != null)
        {
            SetupCard();
        }
        */
    }

    //Set the card without interacting with the deck
    public void SetCard(Card card)
    {
        this.card = card;
    }

    public void UpdatePortrait(PortraitRoom room)
    {
        if (card.IsUnitType())
        {
            if (room != null) room.UpdatePortrait(card.unit);
            portraitFromCamera.gameObject.SetActive(true);
            portraitFromCamera.texture = room.GetRenderTexture();

        }

    }
    void UpdateCardDataAsUnit()
    {
        UnitDataStore unit = card.unit;
        if(unit.faction && factionImageIcon) factionImageIcon.sprite = unit.faction.icon;
        else return;  //Skip filling image data if it is the enemy's hand

        if(unit.special && specialImageIcon) specialImageIcon.sprite = unit.special.icon;
        if(unit.unitClass && classImageIcon) classImageIcon.sprite = unit.unitClass.icon;

        if (rankLetter)
        {
            string rankText = unit.GetRank();
            if (rankText == "D") rankLetter.color = Color.grey;
            if (rankText == "C") rankLetter.color = new Color32(222, 222, 222, 255);
            if (rankText == "B") rankLetter.color = new Color32(73, 156, 255, 255);
            if (rankText == "A") rankLetter.color = new Color32(255, 30, 0, 255);

            rankLetter.SetText(unit.GetRank());
        }

        if (unit.GetRank() == "D") bgImage.sprite = dRankBG;
        else if (unit.GetRank() == "C") bgImage.sprite = cRankBG;
        else if (unit.GetRank() == "B") bgImage.sprite = bRankBG;
        else if (unit.GetRank() == "A") bgImage.sprite = aRankBG;
        else if (unit.GetRank() == "S") bgImage.sprite = sRankBG;
        
    }
    

    void UpdateSelectOrder(int ID)
    {
        if(cardSelectOrder > ID)
        {
            cardSelectOrder--;
            cardSelectOrderDisplay.UpdateOrder(cardSelectOrder);
        }
    }

    public int CompareTo(CardInHand other)
    {
        
        if(isComparingByOrder) return other.cardSelectOrder.CompareTo(cardSelectOrder);
        else
        {
            //Compare by card value
            //
            //Currently the cpu won't calculate that
            return other.cardSelectOrder.CompareTo(cardSelectOrder);
        }
    }
    public void MoveImmediate(Vector3 targetPos)
    {
        transform.position = targetPos;
    }

    public void Move(Vector3 startingPos, Vector3 targetPos)
    {
        StartCoroutine(AnimateMovement(startingPos,targetPos));
    }

    IEnumerator AnimateMovement(Vector3 startingPos, Vector3 targetPos)
    {
        float duration = 0.25f;
        float timeElapsed = 0;
        transform.position = startingPos;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }
}
