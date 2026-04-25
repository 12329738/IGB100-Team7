
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
[Serializable]
public class Stat 
{
    public float baseValue;
    [HideInInspector]
    public float cachedValue;
    [HideInInspector]
    public bool isDirty;

    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    //public void Recalculate()
    //{
    //    float finalValue = baseValue;
    //}


    //public void ApplyModifiers(List<StatModifier> modifiers)
    //{
    //    currentValue = baseValue * CalculateModifiers(modifiers);
    //}

    //public float CalculateModifiers(List<StatModifier> modifiers)
    //{
    //    float percentageModifiers = 1;
    //    float additiveModifiers = 0;
    //    float finalModifier;
    //    foreach (StatModifier modifier in modifiers)
    //    {
    //        if (modifier.isPercentage)
    //        {
    //            percentageModifiers += modifier.amount / 100;
    //        }

    //        else
    //        {
    //            additiveModifiers += modifier.amount;
    //        }
    //    }
    //    finalModifier = percentageModifiers + additiveModifiers;
    //    return finalModifier;
    //}

    //public void ResetStat()
    //{
    //    currentValue = baseValue;

    //    float additive = 0f;
    //    float multiplicative = 1f;

    //    foreach (var mod in modifiers)
    //    {
    //        switch (mod.type)
    //        {
    //            case ModifierType.Flat:
    //                finalValue += mod.value;
    //                break;

    //            case ModifierType.Additive:
    //                additive += mod.value;
    //                break;

    //            case ModifierType.Multiplicative:
    //                multiplicative *= (1 + mod.value);
    //                break;
    //        }
    //    }
    //}

    //public Stat(float baseValue)
    //{
    //    this.baseValue = baseValue;
    //}

    //public void AddModifier(StatModifier mod)
    //{
    //    modifiers.Add(mod);
    //    isDirty = true;
    //}

    //public void RemoveModifier(StatModifier mod)
    //{
    //    modifiers.Remove(mod);
    //    isDirty = true;
    //}

    //public void ClearModifiers()
    //{
    //    modifiers.Clear();
    //    isDirty = true;
    //}

    //public float GetValue()
    //{
    //    if (isDirty)
    //    {
    //        Recalculate();
    //        isDirty = false;
    //    }

    //    return cachedValue;

    //}
}
