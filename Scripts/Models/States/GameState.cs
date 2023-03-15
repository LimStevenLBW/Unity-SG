using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace States
{
    /*
 * Contains all personal data pertaining to a player's progress
 * Only purpose is to be serialized and unserialized for saving/loading data
 */
    [System.Serializable]
    public class GameState
    {
        public int TurnCounterValue { get; private set; } //The current turn the player has advanced to

        public int[] Leaders { get; private set; }  //The leaders that a player owns, represented by int IDs

        /*
         * Representation of a Leader in a player save which may
         * have certain statistics customized 
         */
        public struct SaveableLeader
        {
            public int ID;
            public string FirstName;
            public string LastName;
            public string Description;
            public int TroopCount; //HP of the unit, negatively influences unit performance in battle as it depletes
            public int MaxTroopCount; //The Current Max amount of troops this unit may contain
            public int Attack; //Self explanatory
            public int Vitality; //Resistance to opposing unit attacks
            public int Speed; //Unit's ability to act before others
            public int Charisma; //Unit's ability to convince others and lead
            public int Duplicity; //Unit's ability to conduct schemes
            public int Statecraft; //Unit's ability to manage affairs of the state
        }

        /* Default, no save was found so set default values; */
        public GameState()
        {
            TurnCounterValue = 0;
            Leaders = new int[] { 0, 1, 2 }; //Assign the initial 3 Leaders
        }

        public GameState(TurnCounter turncounter)
        {
            //Need to construct Data
            // TurnCounterValue = turncounter.TurnCounterValue;
            Leaders = new int[] { 0, 1, 2 }; //Assign the initial 3 Leaders
        }
    }

}
