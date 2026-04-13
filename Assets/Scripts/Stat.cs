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
        currentValue = baseValue * CalculateModifiers(modifiers);
    }

    public float CalculateModifiers(List<StatModifier> modifiers)
    {
        float percentageModifiers = 1;
        float additiveModifiers = 0;
        float finalModifier;
        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.isPercentage)
            {
                percentageModifiers += modifier.amount / 100;
            }

            else
            {
                additiveModifiers += modifier.amount;
            }
        }
        finalModifier = percentageModifiers + additiveModifiers;
        return finalModifier;
    }

    public void ResetStat()
    {
        currentValue = baseValue;
    }
}
