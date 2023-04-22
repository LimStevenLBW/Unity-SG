using Assets.Scripts.Interface;
using System.Collections;
using UnityEngine;

public class PlayerHandPanel : MonoBehaviour
{
    public ManagerCombatUI managerUI;
    public UnitManager unitManager;

    public DeckDataStore playerDeck;
    public DeckDataStore enemyDeck;
    public Card[] cards = new Card[5];

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(DeckDataStore playerDeck, DeckDataStore enemyDeck)
    {
        this.playerDeck = playerDeck;
        this.enemyDeck = enemyDeck;
    }

    void OnEnable()
    {
        StartCoroutine(Draw());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Draw()
    {
        //We draw cards from left to right 
        foreach (Card c in cards)
        {
            yield return new WaitForSeconds(.1f);
            c.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(.1f);
        PlaceCaptains();

    }

    public void PlaceCaptains()
    {
        unitManager.AddCaptain(playerDeck.captain.controller, true);
        unitManager.AddCaptain(enemyDeck.captain.controller, false);
    }

}
