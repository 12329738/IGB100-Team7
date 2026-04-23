using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyStatConfig : IIntentModifier
{
    //public override EffectIntent Type => EffectIntent.ModifyStat;

    public float duration;
    public List<StatModifier> modifiers;

    public EffectIntent effectToModifiy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Modify(ref CombatIntent intent)
    {
        throw new NotImplementedException();
    }
}
