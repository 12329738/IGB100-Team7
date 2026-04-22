using System;
using UnityEngine;

[Serializable]

public class ApplyStatusConfig : EffectNodeConfig
{
    public StatusEffectData effectData;

    public override EffectType Type => EffectType.ApplyStatusEffect;

    public override void Execute(EffectContext ctx)
    {
        ctx.intent = EffectIntent.ApplyStatus;

        ctx.payload.status = effectData;

        Debug.Log($"{ctx.source} applies {effectData.name} to {ctx.target}");
    }
}