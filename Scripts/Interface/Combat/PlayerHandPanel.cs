using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reminder, have this be inactive in the scene, need graphic raycaster and to be screen space overlay
public class PlayerHandPanel : MonoBehaviour
{
    public ManagerCombatUI managerUI;
    public UnitManager unitManager;

    public DeckDataStore myDeck;
    public Card[] cards = new Card[5];

    public void Init(DeckDataStore deck)
    {
        myDeck = deck;
    }

    void OnEnable()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DrawStartingHand()
    {
        StartCoroutine(DrawFull());
    }

    //Draw Cards until hand is full
    IEnumerator DrawFull()
    {
        //We draw cards from left to right 
        foreach (Card c in cards)
        {
            yield return new WaitForSeconds(.1f);
            c.DrawCard(myDeck);
        }


        yield return new WaitForSeconds(.1f);
        //PlaceCaptains();

    }

    /*
     * Get a list of units to summon based on the selection order
     */
    public Queue<UnitDataStore> GetDeployableUnits()
    {
        List<Card> temp = new List<Card>();
        foreach (Card c in cards)
        {
            if(c.isSelected) temp.Add(c);
            c.Deselected();
        }
        temp.Sort();
        Queue<UnitDataStore> unitQueue = new Queue<UnitDataStore>();

        foreach(Card c in temp)
        {
            unitQueue.Enqueue(c.unit);
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
        int randomChoice;
        int selectedCount = 0;
        while (selectedCount < 3)
        {
            randomChoice = Random.Range(1, 5);
            foreach (Card c in cards)
            {
                if (c.isSelected) continue; //We skip already selected cards
                if (randomChoice == c.cardNum)
                {
                    c.Select();
                    selectedCount++;
                    yield return new WaitForSeconds(0.2f);
                }

            }
        }
    }

}
