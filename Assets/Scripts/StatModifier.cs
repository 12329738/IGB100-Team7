using System;
using UnityEngine;
[System.Serializable]
public class StatModifier 
{
    public StatType stat;
    public float amount;
    public ModifierType type;
    public IModifierProvider source;
    public float duration = 0;

}
