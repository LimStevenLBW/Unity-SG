using UnityEngine;

/** Example Usage
 * Student s1 = new StudentBuilder().name("Eli").buildStudent();
Student s2 = new StudentBuilder()
                 .name("Spicoli")
                 .age(16)
                 .motto("Aloha, Mr Hand")
                 .buildStudent();

    LeaderBuilder is not needed as I am switching to the ScriptableObjects toolset
    */
public class LeaderBuilder
{
    //Leader Properties
    string _FirstName, _LastName;
    int _ID, _TroopCount, _Attack, _Vitality, _Speed, _Charisma, _Duplicity, _Statecraft;

    public LeaderBuilder() { }

    /*
    public Leader build()
    {
       // return new Leaderx(_ID, _FirstName, _LastName, _TroopCount, _Attack, _Vitality, _Speed, _Charisma, _Duplicity, _Statecraft);
    }
    */

    public LeaderBuilder ID(int _ID)
    {
        this._ID = _ID;
        return this;
    }

    public LeaderBuilder FirstName(string _FirstName)
    {
        this._FirstName = _FirstName;
        return this;
    }

    public LeaderBuilder LastName(string _LastName)
    {
        this._LastName = _LastName;
        return this;
    }

    public LeaderBuilder TroopCount(int _TroopCount)
    {
        this._TroopCount = _TroopCount;
        return this;
    }
    public LeaderBuilder Attack(int _Attack)
    {
        this._Attack = _Attack;
        return this;
    }
    public LeaderBuilder Vitality(int _Vitality)
    {
        this._Vitality = _Vitality;
        return this;
    }
    public LeaderBuilder Speed(int _Speed)
    {
        this._Speed = _Speed;
        return this;
    }
    public LeaderBuilder Charisma(int _Charisma)
    {
        this._Charisma = _Charisma;
        return this;
    }
    public LeaderBuilder Duplicity(int _Duplicity)
    {
        this._Duplicity = _Duplicity;
        return this;
    }
    public LeaderBuilder Statecraft(int _Statecraft)
    {
        this._Statecraft = _Statecraft;
        return this;
    }
}
