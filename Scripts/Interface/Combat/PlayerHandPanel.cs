using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reminder, have this be inactive in the scene, need graphic raycaster and to be screen space overlay
public class PlayerHandPanel : MonoBehaviour
{
    public ManagerCombatUI managerUI;
    public UnitManager unitManager;

    public DeckDataStore playerDeck;
    public DeckDataStore enemyDeck;
    public Card[] cards = new Card[5];

    public void Init(DeckDataStore playerDeck, DeckDataStore enemyDeck)
    {
        this.playerDeck = playerDeck;
        this.enemyDeck = enemyDeck;
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
            c.DrawCard(playerDeck);
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
        unitManager.AddCaptain(playerDeck.captain.controller, true);
        unitManager.AddCaptain(enemyDeck.captain.controller, false);
    }

}
