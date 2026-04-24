using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class RemoveStatusEffect : EffectOperation
{
    public StatusEffectData effect;
    public override EffectIntent Type => EffectIntent.RemoveStatusEffect;

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        Entity target = ctx.target.GetComponent<Entity>();
        if (target != null)
        {
            StatusEffectManager manager = target.GetComponent<StatusEffectManager>();
            if (effect == null)
            {
                manager.ResetStatusEffects();
            }

            else
            manager.RemoveStatus(effect);
        }
    }

}
