using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddModifier : EffectOperation
{
    public override EffectIntent Type => EffectIntent.ModifyStat;
    public StatusEffectData effect;
    public List<StatModifier> modifiers;


    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        StatusEffectOverrides.AddModifier(effect, modifiers);
    }
}
