using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public int cardNum; //unused, just to identify which card in the hand

    public bool isSelected = false;
    public int cardSelectOrder = 0;
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
        footer.UpdateData(unit);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if(Director.Instance.GetPhase() == "CARDSELECT")
        {

            if (!isSelected)
            {
                cardSelectOrder = Director.Instance.GetCardSelectOrder();
                isSelected = true;

                cardSelectOrderDisplay.gameObject.SetActive(true);
                cardSelectOrderDisplay.UpdateOrder(cardSelectOrder);
               
                
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + 25, pos.z);
                AudioPlayer.PlayOneShot(AudioSelect);
            }
            else if (isSelected)
            {
                Director.Instance.NotifyCardDeselected(cardSelectOrder);
                cardSelectOrder = 0;
                isSelected = false;

                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y - 25, pos.z);

                cardSelectOrderDisplay.gameObject.SetActive(false);
                AudioPlayer.PlayOneShot(AudioDeselect);
            }
        }
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
        }
    }

    void UpdatePortrait()
    {
        if (portraitRoom == null) Debug.Log("null?");
        portraitRoom.UpdatePortrait(unit);
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

}
