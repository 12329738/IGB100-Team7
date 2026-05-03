using System;
using UnityEngine;
[System.Serializable]
public class StatModifier 
{
    public StatType stat;
    public float amount;
    public ModifierType type;
    public IModifierProvider source;
    public DamageSourceDefinition appliedTo;
    public float duration = 0;


    public StatModifier(StatModifier mod)
    {
        stat = mod.stat;
        amount = mod.amount;
        type = mod.type;
        source = mod.source;
        duration = mod.duration;
        appliedTo = mod.appliedTo;
    }
}
