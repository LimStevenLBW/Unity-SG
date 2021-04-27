using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    /*
     * Manages the size of the game object, should be generated on startup in future
     */
    public class DeployedDeck : MonoBehaviour
    {
        public GameObject cardPrefab;

        // Start is called before the first frame update
        void Start()
        {
            InitDeck();
        }

        void InitDeck()
        {
            //deck = deck.Shuffle();
           
            for (int i = 0; i < 30; i++)
            {
                PushCard(i * 0.025f);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void PushCard(float offset)
        {
            // Instantiate at position (0, 0, 0) and zero rotation.
            Instantiate(cardPrefab, transform.position + new Vector3(0, offset, 0), transform.rotation, transform);
        }
    }
}
