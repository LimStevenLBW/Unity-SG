using Assets.Scripts.Interface;
using System.Collections;
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
        StartCoroutine(DrawFull());
       
    }

    // Update is called once per frame
    void Update()
    {
        
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


    public void PlaceCaptains()
    {
        unitManager.AddCaptain(playerDeck.captain.controller, true);
        unitManager.AddCaptain(enemyDeck.captain.controller, false);
    }

}
