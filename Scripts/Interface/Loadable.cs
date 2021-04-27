using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;


/*
 * A class that implements this must set its initial fields from save data
 */
public interface ILoadable
{
    //Defines how the class will interact with savedata
    void UnpackLoadedData(GameState gameState);
}
