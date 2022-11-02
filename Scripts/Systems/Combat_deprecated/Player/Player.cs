using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Deck _playerDeck;
        [SerializeField] private Hand _playerHand;
        public CardLeader Leader { get; private set; }

        // C# Auto-Properties generate the field to pair with mutators
        //public int _playerId { get; set; }
        public int lifeTotal { get; set; } = 1000;
        public int actionTotal { get; set; } = 3;
        public bool isActive { get; set; } = false;

        void Awake()
        {
            Leader = _playerDeck.GetLeader();
        }

        void Start()
        {

        }

        /**
         * Called by the Directory class, assigns the identifier and pairs the player's cards with the owner
         */
        public void InitPlayer()
        {
            _playerDeck.RegisterPlayer(this);

            _playerHand.RegisterPlayer(this);
            _playerHand.Draw(5);
        }

        public Deck GetDeck()
        {
            return _playerDeck;
        }

    }
}
