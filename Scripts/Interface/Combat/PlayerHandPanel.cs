using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Reminder, have this be inactive in the scene, need graphic raycaster and to be screen space overlay
public class PlayerHandPanel : MonoBehaviour
{
    public ManagerCombatUI managerUI;
    public UnitManager unitManager;

    public DeckDataStore myDeck;
    public CardInHand[] cards = new CardInHand[5];
    public DeckCountDisplay deckCounter;

    public TextMeshProUGUI traitsText;
    public TextMeshProUGUI basicText;
    public TextMeshProUGUI basicDescription;
    public TextMeshProUGUI specialText;
    public TextMeshProUGUI specialDescription;
    private int playerSelectAmount; // The amount of cards that the cpu can select
    private int cpuSelectAmount; // The amount of cards that the cpu can select

    public void Init(DeckDataStore deck)
    {
        myDeck = deck;
        deckCounter.UpdateCount(deck.GetDeckCount());
        foreach (CardInHand c in cards)
        {
            c.ClearCard();
            c.SetNumberOfSelectable(playerSelectAmount); //not used by cpu
        }

        ResetText();
    }

    void OnEnable()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Move cards around so the cards that contains units are on the lefthand side
    public void RearrangeCards()
    {
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
    }

    // Determines how to draw
    public void FillHand()
    {
        StartCoroutine(DrawFull());
    }

    //Draw Cards until hand is full
    IEnumerator DrawFull()
    {
        //We draw cards from left to right 
        foreach (CardInHand c in cards)
        {
            yield return new WaitForSeconds(.105f);

            //If there is no card, draw a card from the deck and update the counter display
            if(c.unit == null)
            {
                c.DrawCard(myDeck);
                deckCounter.UpdateCount(myDeck.GetDeckCount());
            }
            else
            {
                c.SetupCard();
            }
           
        }


        yield return new WaitForSeconds(.1f);
        //PlaceCaptains();

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

        foreach(CardInHand c in selectedCards)
        {
            unitQueue.Enqueue(c.unit);
            c.ClearCard();
        }

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
            if (c.unit != null)
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
    }

    /*
     * For displaying unit details in the panel
     */
    public void UpdateDescription(UnitDataStore data)
    {
        if (traitsText != null)
        {
            traitsText.gameObject.SetActive(true);
            traitsText.SetText(data.faction.traitName + " " + data.unitClass.traitName);
        }
        if (basicText != null && data.skill1 != null)
        {
            basicText.gameObject.SetActive(true);
            basicDescription.gameObject.SetActive(true);
            basicText.SetText(data.skill1.GetSkillName());
            basicDescription.SetText(data.skill1.GetDescription());
        }
        if (specialText != null && data.skill2 != null)
        {
            specialText.gameObject.SetActive(true);
            specialDescription.gameObject.SetActive(true);
            specialText.SetText(data.skill2.GetSkillName());
            specialDescription.SetText(data.skill2.GetDescription());
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
