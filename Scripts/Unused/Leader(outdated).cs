using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Leaders are recruitable characters within the game that can
 * be represented by these statistics by default.
 */
//[CreateAssetMenu(fileName = "New Leader", menuName = "Leader")]
public class Leaderx : ScriptableObject
{
    public int ID;
    public string FirstName;
    public string LastName;
    public Sprite Portrait;
    public string Description;

    //Combative Stats
    public int TroopCount; //HP of the unit, negatively influences unit performance in battle as it depletes
    public int MaxTroopCount; //The Current Max amount of troops this unit may contain
    public int Attack; //Self explanatory
    public int Vitality; //Resistance to opposing unit attacks
    public int Speed; //Unit's ability to act before others

    //Utility Stats
    public int Charisma; //Unit's ability to convince others and lead
    public int Duplicity; //Unit's ability to conduct schemes
    public int Statecraft; //Unit's ability to manage affairs of the state

    public Leaderx(int ID, string FirstName, string LastName, int TroopCount, int Attack, int Vitality, int Speed, int Charisma, int Duplicity, int Statecraft)
    {
        this.ID = ID;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.TroopCount = TroopCount;
        this.Attack = Attack;
        this.Vitality = Vitality;
        this.Speed = Speed;
        this.Charisma = Charisma;
        this.Duplicity = Duplicity;
        this.Statecraft = Statecraft;
    }

}
