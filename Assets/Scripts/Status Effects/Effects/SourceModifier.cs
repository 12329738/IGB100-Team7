using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SourceModifier : IIntentModifier
{
    public DamageSourceDefinition targetTag;
    public float multiplier;

    public EffectIntent effectToModifiy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Modify(ref CombatIntent intent)
    {
        if (intent.context.origin != targetTag)
            return;


        intent.value *= multiplier;
    }
}
