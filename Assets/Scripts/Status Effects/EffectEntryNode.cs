using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EffectEntryNode
{
    public bool hasTick;
    public float tickInterval;
    public List<CombatCondition> conditions;
    
    [SerializeReference]
    public List<EffectNodeData> effectData;
    


    public void Validate()
    {
        if (effectData == null)
            effectData = new List<EffectNodeData>();

        for (int i = 0; i < effectData.Count; i++)
        {
            if (effectData[i] == null)
                effectData[i] = new EffectNodeData
                {
                    type = EffectIntent.ApplyStatusEffect
                };

            effectData[i].Validate();
        }
    }

    public bool Evaluate(CombatCondition condition, EffectContext context)
    {
        if (!condition.useComparison)
            return true;

        float incomingValue = GetValue(condition.triggerEvent, context);

        

        switch (condition.comparisonType)
        {
            case ComparisonType.GreaterThan:
                return incomingValue > condition.value;
            case ComparisonType.LessThan:
                return incomingValue < condition.value;
            case ComparisonType.EqualTo:
                return Mathf.Approximately(incomingValue, condition.value);
            case ComparisonType.GreaterOrEqual:
                return incomingValue >= condition.value;
            case ComparisonType.LessOrEqual:
                return incomingValue <= condition.value;
        }

        return false;
    }

    private float GetValue(CombatEvent triggerEvent, EffectContext context)
    {
        switch (triggerEvent)
        {
            case CombatEvent.OnDamage:
                return context.value;

            case CombatEvent.OnStackCount:
                return (float)context.stacks;

            case CombatEvent.OnHeal:
                return context.value;

            default:
                return 0f;
        }
    }

    public bool CanExecute(EffectContext ctx)
    {
        return conditions.Any(x => x.triggerEvent == ctx.trigger);
    }

    public void Execute(EffectContext ctx, List<CombatIntent> intents)
    {
        if (!CanExecute(ctx))
            return;

        foreach (var node in effectData)
        {
            if (node.effectOperation is not IIntentModifier)
            {
                foreach (CombatCondition condition in conditions)
                {
                    if (Evaluate(condition, ctx))
                    {
                        node.Execute(ctx, intents);
                    }
                    break;

                }
            }
        }
            
    }

    public void Execute(EffectContext ctx, ref List<CombatIntent> intents)
    {
        if (!CanExecute(ctx))
            return;

        foreach (var node in effectData)
        {

            if (node.effectOperation is IIntentModifier)
            {
                foreach (CombatCondition condition in conditions)
                {
                    if (Evaluate(condition, ctx))
                    {
                        node.Modify(ctx, ref intents);
                    }
                    break;
                        
                }
                
            }
        }

    }
}