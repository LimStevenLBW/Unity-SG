using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Reminder, have this be inactive in the scene, need graphic raycaster and to be screen space overlay
//Contains the player's hand and displays the cards on screen
public class PlayerHandPanel : MonoBehaviour
{
    public ManagerCombatUI managerUI;
    public UnitManager unitManager;
    public DeckDataStore myDeck;
    public CardInHand[] cards = new CardInHand[5];

    public TextMeshProUGUI traitsText;
    public TextMeshProUGUI basicText;
    public TextMeshProUGUI basicDescription;
    public TextMeshProUGUI specialText;
    public TextMeshProUGUI specialDescription;

    [SerializeField] private List<CardInHand> hand = new List<CardInHand>();
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioCardDeselect;
    [SerializeField] private AudioClip AudioCardSelect;
    [SerializeField] private AudioClip AudioCardAppear;

    [SerializeField] private CardInHand prefab;
    [SerializeField] private DeckCountDisplay deckCounter;
    [SerializeField] private PortraitRoomContainer portraitRooms;
    [SerializeField] private DetailsFooter footer;

    private int playerSelectAmount; // The amount of cards that the cpu can select
    private int cpuSelectAmount; // The amount of cards that the cpu can select

    //Contain the positions that the cards in hand will use
    private PlayerHandLayout activeLayout;
    [SerializeField] private PlayerHandLayout layout5;
    [SerializeField] private PlayerHandLayout layout6;
    [SerializeField] private PlayerHandLayout layout7;
    [SerializeField] private PlayerHandLayout layout8;
    [SerializeField] private PlayerHandLayout layout9;

    public void Init(DeckDataStore deck)
    {
        myDeck = deck;
        deckCounter.UpdateCount(deck.GetDeckCount());
        /*
        foreach (CardInHand c in cards)
        {
            c.ClearCard();
            c.SetNumberOfSelectable(playerSelectAmount); //not used by cpu
        }
        */

        ResetText();
    }

    /*
     * Simulates drawing from the deck, we instantiate CardInHand prefabs to contain cards pulled from the deck.
     * 
     */
    public void Draw(int amount)
    {
        //Determine the active layout
        int handSize = hand.Count + amount;
        if (handSize <= 5) activeLayout = layout5;
        else if (handSize == 6) activeLayout = layout6;
        else if (handSize == 7) activeLayout = layout7;
        else if (handSize == 8) activeLayout = layout8;
        else if (handSize == 9) activeLayout = layout9;

        for (int i = 0; i < amount; i++)
        {
            CardInHand cardElement = Instantiate(prefab);
            hand.Add(cardElement);

            cardElement.gameObject.transform.SetParent(gameObject.transform);
            cardElement.gameObject.transform.position = prefab.transform.position;
            cardElement.gameObject.transform.localScale = new Vector3(0, 0, 0); //The cards start off hidden
            cardElement.Init(this, footer);
        }

        //We have the gameobjects readied, now we animate and get the data
        StartCoroutine(AnimateDraw(amount));
    }

    //Animate Drawing cards
    IEnumerator AnimateDraw(int amount)
    {
        int cardNumber = 1;
        //We draw cards from left to right 
        foreach (CardInHand c in hand)
        {
            Vector3 targetPos = activeLayout.GetPosition(cardNumber);
            Vector3 startingPos = new Vector3(1500, targetPos.y, targetPos.z);

            yield return new WaitForSeconds(.105f);

            //If there is no card data, reveal the prefab, draw a card from the deck and update the counter display. Then animate with sound
            if (c.card == null)
            {
                c.gameObject.transform.localScale = new Vector3(1, 1, 1);
                c.DrawCard(myDeck);
                c.UpdatePortrait(portraitRooms.GetPortraitRoom(cardNumber));

                c.Move(startingPos, targetPos);
                deckCounter.UpdateCount(myDeck.GetDeckCount());
                if (AudioPlayer && AudioCardAppear) AudioPlayer.PlayOneShot(AudioCardAppear);
            }
            else
            {
                c.SetupCard();
            }

            cardNumber++;
        }


        yield return new WaitForSeconds(.1f);
        //PlaceCaptains();

    }

    
    //Move cards around so the cards that contains units are on the lefthand side
    public void RearrangeCards()
    {
        /*
        List<UnitDataStore> unitList = new List<UnitDataStore>();
        foreach(CardInHand c in cards)
        {
            if (c.unit != null)
            {
                unitList.Add(c.unit);
                c.ClearCard();
            }
        }

        for(int i = 0; i < unitList.Count; i++)
        {
            cards[i].unit = unitList[i];
        }
        */
    }
    

    // Determines how to draw
    public void FillHand()
    {
        //StartCoroutine(DrawFull());
    }


    /*
     * Create a list of units to summon based on the selection order
     */

    public Queue<UnitDataStore> PlayCards()
    {

        List<CardInHand> selectedCards = new List<CardInHand>();
        foreach (CardInHand c in cards)
        {
            if (c.isSelected)
            {
                selectedCards.Add(c);
                c.Deselected();
            }
            else { c.gameObject.SetActive(false);  }
           
        }
        selectedCards.Sort();
        Queue<UnitDataStore> unitQueue = new Queue<UnitDataStore>();
        /*
        foreach(CardInHand c in selectedCards)
        {
            unitQueue.Enqueue(c.unit);
            c.ClearCard();
        }
        */
        return unitQueue;

    }

    public void PlaceCaptains()
    {
       // unitManager.AddCaptain(playerDeck.captain.controller, true);
       // unitManager.AddCaptain(enemyDeck.captain.controller, false);
    }

    /*
     * Enemy CPU will determine which cards to play
     * todo, currently the cpu will just play a random 3 cards
     */
    public void CPUSelectCards()
    {
        StartCoroutine(RandomSelect());
      

    }
    IEnumerator RandomSelect()
    {
        yield return new WaitForSeconds(1.1f);
        int randomValue;

        List<CardInHand> selectionPriority = new List<CardInHand>();
        //Fill each card with a random value
        foreach(CardInHand c in cards)
        {
            if (c.card != null)
            {
                randomValue = Random.Range(1, 99);
                c.cardValue = randomValue;
                selectionPriority.Add(c);
            }
        }

        //Sort from greatest to least, compare cards by their card value
        selectionPriority.Sort((left, right) => right.cardValue.CompareTo(left.cardValue));
        if (cpuSelectAmount > selectionPriority.Count) cpuSelectAmount = selectionPriority.Count;
   

        for(int i=0; i < cpuSelectAmount; i++)
        {
            selectionPriority[i].Select();
            yield return new WaitForSeconds(0.2f);
        }

        Director.Instance.SetPhase("ENEMYDEPLOYMENT");
    }

    public void UpdateSelectableAmount(bool isPlayer, int amount)
    {
        /*
        if (isPlayer) {
            playerSelectAmount = amount;
            foreach(CardInHand c in cards)
            {
                c.SetNumberOfSelectable(playerSelectAmount);
            }
        }
        else
        {
            cpuSelectAmount = amount;
        }
        */
    }

    /*
     * For displaying unit details in the panel
     */
    public void UpdateDescription(Card card)
    {
        UnitDataStore unit = card.unit;
        if (traitsText != null)
        {
            traitsText.gameObject.SetActive(true);
            traitsText.SetText(unit.faction.traitName + " " + unit.unitClass.traitName);
        }
        if (basicText != null && unit.skill1 != null)
        {
            basicText.gameObject.SetActive(true);
            basicDescription.gameObject.SetActive(true);
            basicText.SetText(unit.skill1.GetSkillName());
            basicDescription.SetText(unit.skill1.GetDescription());
        }
        if (specialText != null && unit.skill2 != null)
        {
            specialText.gameObject.SetActive(true);
            specialDescription.gameObject.SetActive(true);
            specialText.SetText(unit.skill2.GetSkillName());
            specialDescription.SetText(unit.skill2.GetDescription());
        }

    }

    public void ResetText()
    {
        if (traitsText != null) traitsText.gameObject.SetActive(false);
        if (basicText != null) basicText.gameObject.SetActive(false);
        if (basicDescription != null) basicDescription.gameObject.SetActive(false);
        if (specialText != null) specialText.gameObject.SetActive(false);
        if (specialDescription != null) specialDescription.gameObject.SetActive(false);
    }

}
