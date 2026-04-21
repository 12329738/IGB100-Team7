using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyStatConfig : EffectNodeConfig
{
    public override EffectNodeType Type => EffectNodeType.ModifyStat;

    public float duration;
    public List<StatModifier> modifiers;

    public override void Execute(EffectContext ctx)
    {
        IStats stats = ctx.source.GetComponent<IStats>();
        if (stats != null)
        {
             stats.stats.AddModifierSource(ctx.source, modifiers);
             stats.stats.MarkDirty();
        }
    }
}
