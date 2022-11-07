using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitBase : ScriptableObject
{
    public string unitName;
    public string bio;
    public int level;
    public Sprite portrait;

    public float baseHealth;
    public float baseAttack;
    public float baseDefense;
    public float baseSpeed;

    public float healthGrowthRate;
    public float attackGrowthRate;
    public float defenseGrowthRate;
    public float speedGrowthRate;

    public UnitClass unitClass;

    public Skill skill_1;
    public Skill skill_2;
    public Skill skill_3;
    public Skill ultimate;

    //public string classes
    //public string abilities
    //public string star rating
} 
