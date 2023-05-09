using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IComparable<Card>
{
    public int cardNum; //Identify which card in the hand
    public bool isComparingByOrder = true; //When set to true, sort methods will compare this object by cardSelectOrder, other it will use cardValue
    public bool isSelected = false;
    public int cardSelectOrder = 0;
   // private int cardValue = 0; //How much this card will be valued by the cpu to play
    private int numberOfSelectable = 5;
    public DetailsFooter footer;
    public UnitDataStore unit;
    public PortraitRoom portraitRoom;
    public CardSelectOrder cardSelectOrderDisplay;

    public Image factionImageIcon;
    public Image specialImageIcon;
    public Image classImageIcon;

    private Image bgImage;
    private Sprite dRankBG;
    private Sprite cRankBG;
    private Sprite bRankBG;
    private Sprite aRankBG;
    private Sprite sRankBG;


    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioDeselect;
    [SerializeField] private AudioClip AudioSelect;
    [SerializeField] private AudioClip AudioAppear;


    void OnEnable()
    {
        AudioPlayer.PlayOneShot(AudioAppear);

    }

    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Card Art/pixelCardAssest_V01");
        bgImage = GetComponent<Image>();
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


    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Not allowed to see enemy cards
        if(Director.Instance.GetPhase() != "ENEMYCARDSELECT") footer.UpdateData(unit);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        footer.ResetText();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        if(Director.Instance.GetPhase() == "CARDSELECT")
        {

            if (!isSelected && Director.Instance.GetCardSelectOrder() < numberOfSelectable)
            {
                Select();
            }
            else if (isSelected)
            {
                Director.Instance.NotifyCardDeselected(cardSelectOrder);
                Deselected();
                AudioPlayer.PlayOneShot(AudioDeselect); //placed here to avoid the sound being used when called outside of pointer event
            }
        }
    }

    public void Select()
    {
        AudioPlayer.PlayOneShot(AudioSelect); //Always play selected sound
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

    public void DrawCard(DeckDataStore deck)
    {
        //If this unit doesnt have any data, draw a card
        if(unit == null)
        {
            if(!deck.IsEmpty()) unit = deck.DrawCard();
        }

        if (unit != null) {
            gameObject.SetActive(true);
            UpdatePortrait();
            UpdateCardValue();
        }
    }

    void UpdatePortrait()
    {
        if (portraitRoom == null) Debug.Log("null?");
        else {
            portraitRoom.UpdatePortrait(unit);
            if(unit.faction && factionImageIcon) factionImageIcon.sprite = unit.faction.icon;
            else { return; } //Skip the rest for enemy hand
            if(unit.special && specialImageIcon) specialImageIcon.sprite = unit.special.icon;
            if(unit.unitClass && classImageIcon) classImageIcon.sprite = unit.unitClass.icon;

            if (unit.GetRank() == "D") bgImage.sprite = dRankBG;
            else if (unit.GetRank() == "C") bgImage.sprite = cRankBG;
            else if (unit.GetRank() == "B") bgImage.sprite = bRankBG;
            else if (unit.GetRank() == "A") bgImage.sprite = aRankBG;
            else if (unit.GetRank() == "S") bgImage.sprite = sRankBG;
        }
    }

    void UpdateCardValue()
    {

    }

    void UpdateSelectOrder(int ID)
    {
        if(cardSelectOrder > ID)
        {
            cardSelectOrder--;
            cardSelectOrderDisplay.UpdateOrder(cardSelectOrder);
        }
    }

    public int CompareTo(Card other)
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
}
