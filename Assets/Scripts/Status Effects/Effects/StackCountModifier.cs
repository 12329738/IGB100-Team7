using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StackCountModifier : EffectOperation, IIntentModifier
{
    public float valuePerStack;
    public List<StatusEffectData> effects;
    public ModifierType modifierType;
    public EffectIntent _effectToModify;
    public EffectIntent effectToModifiy { get => _effectToModify; set => _effectToModify = value; }

    public override EffectIntent Type => EffectIntent.StackCount;

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        throw new System.NotImplementedException();
    }

    public void Modify(ref CombatIntent intent)
    {
        if (intent.intent != effectToModifiy)
            return;
        float total = 0;

        foreach (var e in effects)
        {
            total += GameManager.instance.statusEffectRegistry.GetStacksFromSource(e.definition);
        }


        if (modifierType == ModifierType.Flat)
        {
            intent.value += (total * valuePerStack);
        }

        else if (modifierType == ModifierType.Percentage)
        {
            intent.value = 1f + (total * (valuePerStack/100));
        }

        Debug.Log($"{_effectToModify.ToString()} modified by {total * valuePerStack} due to {total} stacks of {string.Join(",", effects.Select(i => i.name))} ");
        
    }
}

