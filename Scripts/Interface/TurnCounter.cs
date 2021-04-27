using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using GameSystems;
using States;
/*
 Notes: When new to C#, it may be tempting to just give all properties both getters and setters, which unless they explicitly need to be mutable, is bad practice.

“Tell an object what to do, don’t ask it for information and manipulate it yourself”. With that in mind, don’t add setters and getters by default.
If a variable does not need to be accessed at all by other classes, make it private instead.
And if there is need for external class access, try to have a good reason to use a setter!

/*
 * Tracks the amount of turns the player has taken since the start of the game
 * TurnCounterValue is stored within savedata
 */

public class TurnCounter : MonoBehaviour, ILoadable
{
    //TurnCounterValue needs to be accessible by the SaveData class
    private int TurnCounterValue;
    private GameObject TextMeshProObject;
    private TextMeshProUGUI counter;

    void Start()
    {
        //Get the first child of the TurnCounter which should be a TMP
        TextMeshProObject = transform.GetChild(0).gameObject;
        counter = TextMeshProObject.GetComponent<TextMeshProUGUI>();

        TurnCounterValue = 0;
    }

    //Called when the gameobject is toggled active
    void OnEnable()
    {
        //Registers the handlers
        EventBroker.OnIncrementTurn += UpdateTurnCounter;
        EventBroker.OnDataLoaded += UnpackLoadedData;
    }

    void OnDisable()
    {
        //Removes the handlers
        EventBroker.OnIncrementTurn -= UpdateTurnCounter;
        EventBroker.OnDataLoaded -= UnpackLoadedData;
    }

    /*Event Function, Increments the Turn Counter*/
    private void UpdateTurnCounter()
    {
        Debug.Log("Turn Counter is being updated");
        TurnCounterValue++;
        counter.SetText("Turn " + TurnCounterValue);
       // EventBroker.invokeStateUpdate(GameState gameState);
    }

    /*Event Function, Sets the turn counter*/
    public void UnpackLoadedData(GameState gameState)
    {
        Debug.Log("TurnCounter: Loading Turn Counter Value");
        TurnCounterValue = gameState.TurnCounterValue;
        counter.SetText("Turn " + TurnCounterValue);
    }
}
