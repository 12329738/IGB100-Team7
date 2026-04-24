using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ApplyStatusConfig : EffectOperation
{
    public StatusEffectData effectData;

    public override EffectIntent Type => EffectIntent.ApplyStatusEffect;

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        intents.Add(new CombatIntent
        {
            context = ctx,
            type = Type,
            source = ctx.source,
            target = ctx.target,
        });


        ctx.payload.status = new StatusEffectDataInstance(effectData);
    }


}