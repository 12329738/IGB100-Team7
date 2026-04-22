using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StatusCountModifier : MonoBehaviour
{
    public float valuePerStack;
    public EffectNodeType effect;
    public List<StatusEffectData> countedEffects;
    public void Modify(EffectContext ctx)
    {
        if (ctx.effectType != effect) 
            return;

        int totalStacks = 0;

        foreach (var effectData in countedEffects)
        {
            totalStacks += StatusEffectRegistry.instance.GetStacksFromSource(effectData, ctx.source);
        }

        float multiplier = 1f + (valuePerStack * totalStacks);

        ctx.value *= multiplier;
    }
}
