using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IComparable<Card>
{
    public int cardNum; //Identify which card in the hand
    public bool isComparingByOrder = true; //When set to true, sort methods will compare this object by cardSelectOrder, other it will use cardValue
    public bool isSelected = false;
    public int cardSelectOrder = 0;
    private int cardValue = 0; //How much this card will be valued by the cpu to play

    public DetailsFooter footer;
    public UnitDataStore unit;
    public PortraitRoom portraitRoom;
    public CardSelectOrder cardSelectOrderDisplay;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioDeselect;
    [SerializeField] private AudioClip AudioSelect;
    [SerializeField] private AudioClip AudioAppear;

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

            if (!isSelected && Director.Instance.GetCardSelectOrder() < 3)
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
        else { portraitRoom.UpdatePortrait(unit); }
    }

    void UpdateCardValue()
    {

    }

    void OnEnable()
    {
        AudioPlayer.PlayOneShot(AudioAppear);
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
