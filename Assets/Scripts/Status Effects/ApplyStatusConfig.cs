using System;
using UnityEngine;

[Serializable]
public class ApplyStatusConfig : EffectNodeConfig
{
    public StatusEffectData effectData;
    public override void Execute(EffectContext ctx)
    {
        Debug.Log($"{ctx.source} applies {effectData.name} to {ctx.target}");
        ctx.target.GetComponent<StatusEffectManager>()
            ?.ApplyEffect(effectData, ctx.source);
    }
}