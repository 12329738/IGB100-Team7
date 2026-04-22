using System;
using UnityEngine;

[Serializable]
public class SourceModifier : EffectNodeConfig
{
    public DamageSourceDefinition targetTag;
    public float multiplier;
    public override EffectType Type => EffectType.SourceModifier;

    public override void Execute(EffectContext ctx)
    {
        if (ctx.origin != targetTag)
            return;

        var modifier = new ContextModifier
        {
            value = multiplier,
            source = this,
            tag = "source_modifier"
        };
        ctx.modifiers.Add(modifier);

    }
}
