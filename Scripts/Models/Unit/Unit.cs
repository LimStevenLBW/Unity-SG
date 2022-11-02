using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string unitName;
    public Sprite portrait;

    public int baseHealth;
    public int baseAttack;
    public int baseSpeed;
} 
