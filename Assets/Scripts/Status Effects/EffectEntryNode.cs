using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectEntryNode
{
    public List<CombatEvent> triggers;
    public bool hasTick;
    public float tickInterval;
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

    public bool CanExecute(EffectContext ctx)
    {
        return triggers.Contains(ctx.trigger);
    }

    public void Execute(EffectContext ctx, List<CombatIntent> intents)
    {
        if (!CanExecute(ctx))
            return;

        foreach (var node in effectData)
        {
            if (node.effectOperation is not IIntentModifier)
            {
                node.Execute(ctx, intents);
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
                node.Modify(ctx, ref intents);
            }
        }

    }
}