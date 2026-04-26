using System;
using UnityEngine;
[System.Serializable]
public class StatModifier 
{
    public StatType stat;
    public float amount;
    public bool isPercentage = true;
    public ModifierType type;
    public IModifierProvider source;

    public StatModifier()
    {

    }

}
