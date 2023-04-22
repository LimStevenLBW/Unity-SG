using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler
{
    public int cardNum;
    public DetailsFooter footer;
    public UnitDataStore unit;
    public PortraitRoom portraitRoom;

    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioHover;
    [SerializeField] private AudioClip AudioClick;
    [SerializeField] private AudioClip AudioAppear;

    public void OnPointerEnter(PointerEventData eventData)
    {
        footer.UpdateData(unit);
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

    public void Init()
    {
       
        
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
