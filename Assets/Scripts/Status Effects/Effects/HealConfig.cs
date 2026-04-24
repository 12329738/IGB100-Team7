using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class HealConfig : EffectOperation
{
    public float amount;
    public override EffectIntent Type => EffectIntent.Heal;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        intents.Add(new CombatIntent
        {
            type = Type,
<<<<<<< Updated upstream
            source = ctx.damageSource,
            target = ctx.target,
=======
            damageInstanceSource = ctx.damageInstanceSource,
            target = GameManager.instance.player,
>>>>>>> Stashed changes
            value = amount,
            context = ctx
        });
    }
}