using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using States;


namespace GameSystems
{
    internal sealed class EventBroker
    {
        public static EventBroker instance = null;
        public EventBroker()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                //ERROR
            }
        }

        //Define a Delegate, Define an event based on that delegate, and then Raise the event
        //Can be viewed as a list of pointers to events

        public delegate void EventZeroArgs();
        public delegate void EventGameState(GameState gameState);

        public static event EventZeroArgs OnIncrementTurn;    //Event: When the Next Turn Occurs, 
        public static event EventGameState OnDataLoaded;      //Event: After savedata is loaded, update all objects with savedata

        public void getEventMap()
        {

        }

        /* Delegate invocation, callable event to signal the next turn*/
        public static void InvokeIncrementTurn()
        {
            if (OnIncrementTurn != null) OnIncrementTurn();
        }

        /*
         * Event Function to initialize all subscribers that require new data from a save file
         * @param savedata, the data that was loaded
         */
        public static void InvokeDataLoaded(GameState gameState)
        {
            if (OnDataLoaded != null) OnDataLoaded(gameState);
        }
    }

}
