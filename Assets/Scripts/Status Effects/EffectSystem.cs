using System;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public Combat combat;
    public void Execute(EffectContext ctx)
    {
        Resolve(ctx);
        var combat = ctx.target.GetComponent<Combat>();
        if (combat == null)
            return;
        switch (ctx.intent)
        {
            case EffectIntent.DealDamage:
                combat.DealDamage(ctx);
                break;

            case EffectIntent.Heal:
                combat.Heal(ctx, ctx.value);
                break;

            case EffectIntent.ApplyStatus:
                ctx.target.GetComponent<StatusEffectManager>()
                    ?.QueueApplyAffect((StatusEffectData)ctx.payload.status, ctx.source);
                break;

            case EffectIntent.SpawnProjectile:
                Debug.Log("Source modifiers currently not in effect for sprojectile spawn effect");
                var weapon = ctx.payload.weapon; 
                weapon?.SpawnProjectiles(ctx);
                break;

            case EffectIntent.Knockback:
                combat.KnockBack(ctx, ctx.value);
                break;
        }
    }

    private void Resolve(EffectContext ctx)
    {
        foreach (ContextModifier m in ctx.modifiers)
        {
            ctx.value *= m.value;
        }
    }
}