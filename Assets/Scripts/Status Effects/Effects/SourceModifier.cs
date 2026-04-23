using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SourceModifier : EffectOperation, IIntentModifier
{
    public DamageSourceDefinition targetTag;
    public float multiplier;
    public ValueSource valueSource;
    public EffectIntent _effectToModify;
    public EffectIntent effectToModifiy { get => _effectToModify; set => _effectToModify = value; }

    public override EffectIntent Type => EffectIntent.SourceModifier;

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        throw new NotImplementedException();
    }

    public void Modify(ref CombatIntent intent)
    {
        if (intent.context.origin != targetTag)
            return;
        float multiplier = valueSource.Evaluate();

        intent.value *= multiplier;
        Debug.Log($"Source {targetTag} {intent} multipled by {multiplier}");
    }
}
