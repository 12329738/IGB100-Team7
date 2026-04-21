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
        Dictionary<object, List<StatModifier>> dict = new Dictionary<object, List<StatModifier>>();
        dict.Add(this, modifiers);
        if (stats != null)
        {
             stats.stats.AddModifierSource(ctx.source, dict);
             stats.stats.MarkDirty();
        }
    }
}
