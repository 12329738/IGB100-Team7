using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyStatConfig : EffectNodeConfig
{
    public override EffectType Type => EffectType.ModifyStat;

    public float duration;
    public List<StatModifier> modifiers;

    public override void Execute(EffectContext ctx)
    {

    }
}
