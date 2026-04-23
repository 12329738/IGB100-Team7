using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StackCountModifier : EffectNodeConfig
{
    public float valuePerStack;
    public EffectType effect;
    public List<StatusEffectData> effects;
    public ModifierType modifierType;
    public override EffectType Type => EffectType.StackCount;


    public override void Execute(EffectContext ctx)
    {
        float total = 0;

        foreach (var e in effects)
        {
            total += GameManager.instance.statusEffectRegistry.GetStacksFromSource(e, ctx.source);
        }

        var modifier = new ContextModifier
        {
            source = this,
            tag = "stack_count"
        };

        if (modifierType == ModifierType.Flat)
        {
            modifier.value += (total * valuePerStack);
        }

        else if (modifierType == ModifierType.Percentage)
        {
            modifier.value = 1f + (total * (valuePerStack/100));
        }

        ctx.modifiers.Add(modifier);
    }
}

