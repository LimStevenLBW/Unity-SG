using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class Hand : MonoBehaviour
    {
        [SerializeField]
        private CardInHand _cardInHand;
        [SerializeField]
        private CardContext _cardContext;

        private Player owner;
        private Deck deck;
        private List<CardInHand> playerHand;

        public void RegisterPlayer(Player owner)
        {
            this.owner = owner;
            deck = owner.GetDeck();

            playerHand = new List<CardInHand>();
            GameEvents.instance.onTurnPlayerDraw += Draw;
        }

        /*
         * Adds new cards to the stored list by 'popping' the objects from a paired deck
         */
        public void Draw(int amount)
        {
            for (int i = 0; i < amount; i++) {
                CardInHand newCard = Instantiate(_cardInHand, transform.position, transform.rotation, transform);

                newCard.InitCard(deck.Pop(), this, owner);
                playerHand.Add(newCard);
            } 
        }

        public void CreateNewContext(CardUnit unit, Vector3 pos)
        {
            _cardContext.CreateNewContext(unit, Input.mousePosition);
        }

    }
}
