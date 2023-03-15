using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

namespace GameSystems
{
    /**
     * Set up as a singleton, only one of this type can exist and it persists across all scenes
     */
    internal sealed class GameManager : MonoBehaviour
    {
        //Singleton Instance
        public static GameManager instance = null;
       
        private GameState gameState; //Game State is an object containing fields
        private EventBroker eventBroker;
        private SaveDataManager saveDataManager;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //When we load a new scene, normally all objects in the hierarchy are destroyed, this prevents that so it the object persists
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            saveDataManager = new SaveDataManager(); //Responsible for Serializing and Deserializing game state
            eventBroker = new EventBroker();         //Responsible for defining the pubsub messaging system
            gameState = new GameState();          //Storage of current game state




            //Load a new gamestate from savedata
            //saveData = saveDataManager.LoadGame();

            //If no SaveState available, start with default

            /*
            //Load Scriptable Object Reference Variables
            if (saveData != null)
            {
                Debug.Log("GameData found and loaded");
            }
            else
            {
                saveData = new SaveableData();
                Debug.Log("Game Manager Says: No SaveData found?");
            }
            */
            //  EventManager.TriggerDataLoaded(GameData);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnApplicationQuit()
        {
            instance = null;
        }
    }

}