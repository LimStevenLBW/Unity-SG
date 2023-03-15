using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    /*  
     * Root of the combat scene. Exists as a singleton that can be referenced by any
     * game object in the scene to get approved global data.
     */
    public class Director : MonoBehaviour
    {
        public static Director Scene = null;

        [SerializeField] private Player _player;
        [SerializeField] private Player _opponent;
        [SerializeField] private Field _playerField;
        [SerializeField] private Field _oppField;

        private PhaseController phaseController;

        void Awake()
        {
            if (Scene == null)
            {
                Scene = this;
                DontDestroyOnLoad(this.gameObject);

            }
            else { Destroy(this); Debug.Log("Error: Duplicate Director was generated"); }
        }

        // Start is called before the first frame update, all monobehaviours that subscribe to phaseController should do so in Awake()
        void Start()
        {
            Debug.Log("Game Start");

            //Players are initialized here to ensure that deck and hand monobehaviours are ready
            _player.InitPlayer();
            _opponent.InitPlayer();

            //Deploys the leader units for both decks onto the field
            _playerField.DeployLeader(_player.Leader);
            _oppField.DeployLeader(_opponent.Leader);

            //Begin phase processing
            phaseController = new PhaseController(_player, _opponent);
        }
         
        // Handle global input
        void Update()
        {
            //On Right Click Detected
            if (Input.GetMouseButton(1))
            {
                CombatEventBroker.instance.ResetWindows();
            }
        }

        public Phase GetCurrentPhase()
        {
            return phaseController.GetCurrentPhase();
        }

        //public SubPhase GetCurrentSubPhase()
       // {

        //}

       
    }
}
