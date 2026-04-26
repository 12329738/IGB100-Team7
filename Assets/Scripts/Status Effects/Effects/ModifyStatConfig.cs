using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyStatConfig : EffectOperation, IIntentModifier
{
    public override EffectIntent Type => EffectIntent.ModifyStat;
    public ModifierType type;
    public ValueSource valueSource;
    public List<StatModifier> modifiers;

    public EffectIntent effectToModifiy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Modify(ref CombatIntent intent)
    {
        throw new NotImplementedException();
    }

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        throw new NotImplementedException();
    }
}
