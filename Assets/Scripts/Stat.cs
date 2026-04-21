using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
[Serializable]
public class Stat 
{
    public float baseValue;
    [HideInInspector]
    public float currentValue;
    [HideInInspector]


    public void Initialize(float value)
    {
        baseValue = value;
        currentValue = value;
    }

    public void ApplyModifiers(List<StatModifier> modifiers)
    {
        currentValue = CalculateModifiers(modifiers);
    }

    public float CalculateModifiers(List<StatModifier> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0)
            return 1f;

        float multiplier = 1f;
        float additive = baseValue;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.isPercentage)
                multiplier += modifier.amount / 100f;
            else
                additive += modifier.amount;
        }

        return (additive) * multiplier;
    }

    public void ResetStat()
    {
        currentValue = baseValue;
    }
}
