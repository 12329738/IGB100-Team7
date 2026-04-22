using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StackCountModifier : EffectNodeConfig
{
    public float valuePerStack;
    public EffectType effect;
    public List<StatusEffectData> effects;

    public override EffectType Type => EffectType.StackCount;


    public override void Execute(EffectContext ctx)
    {
        float total = 0;

        foreach (var e in effects)
        {
            total += StatusEffectRegistry.instance
                .GetStacksFromSource(e, ctx.target);
        }

        var modifier = new ContextModifier
        {
            value = 1f + (total * valuePerStack),
            source = this,
            tag = "stack_count"
        };

        ctx.modifiers.Add(modifier);
    }
}
