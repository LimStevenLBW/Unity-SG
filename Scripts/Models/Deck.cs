using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Systems.Combat;

[CreateAssetMenu(fileName = "New Deck", menuName = "Card/Deck")]
public class Deck : ScriptableObject
{
    public List<CardUnit> deck;
    public CardLeader leader;
    private List<CardUnit> deckClone; //Deck used during active combat phases

    public Deck()
    {

    }

    public void RegisterPlayer(Player player)
    {
        deckClone = new List<CardUnit>(deck); //Todo, change from shallow to a proper Deep Copy

        for(int i = 0; i < deckClone.Count; i++)
        {
            deckClone[i].SetOwner(player);
        }

        Shuffle();
    }

    public CardLeader GetLeader()
    {
        return leader;
    }

    // Removes the top card of the active deck
    public CardUnit Pop()
    {
        CardUnit unitCard = deckClone[deckClone.Count - 1];
        deckClone.RemoveAt(deckClone.Count - 1);
        return unitCard;
    }

    /*
     * Shuffles a deck based on the Fisher-Yates algorithm
     */
    public void Shuffle()
    {
        int length = deckClone.Count;
        
        for(int i = length; i > 1; i--)
        {
            int randomIdx = Random.Range(0, i);

            CardUnit unit = deckClone[randomIdx];
            deckClone[randomIdx] = deckClone[i - 1];
            deckClone[i - 1] = unit;
        }
        

    }
}
