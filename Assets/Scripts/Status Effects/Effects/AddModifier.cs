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
    public List<EffectEntryNode> effects;


    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        if (modifiers != null)
            StatusEffectOverrides.AddModifier(effect, modifiers);

        if (effects != null)
            StatusEffectOverrides.AddEffects(effect, effects);
    }

    public override void Validate()
    {
        if (effects == null) return;

        foreach (var entry in effects)
        {
            if (entry == null) continue;

            entry.Validate();
        }
    }

}
