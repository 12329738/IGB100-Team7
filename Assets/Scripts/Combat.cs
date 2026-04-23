using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.Video.VideoPlayer;

public class Combat : MonoBehaviour
{
    private Dictionary<(object damageId, GameObject target), float> lastHitTimes = new();
    private EventHandler eventHandler;

    public void DealDamage(EffectContext ctx)
    {
        var key = (ctx.effectInstanceId, ctx.target);

        if (lastHitTimes.TryGetValue(key, out float lastHit))
        {
            if (Time.time - lastHit < ctx.hitInterval)
                return;
        }

        lastHitTimes[key] = Time.time;

        var damageable = ctx.target.GetComponent<IDamageable>();
        if (damageable == null || !damageable.IsDamageable())
            return;

        damageable.TakeDamage(ctx);

        ctx.target.RaiseEvent(CombatEvent.OnDamageTaken, ctx);

        if (ctx.isHit)
        {
            EffectContext hitCtx = ctx.Clone();
            hitCtx.trigger = CombatEvent.OnHit;
            ctx.eventHandler?.RaiseEvent(hitCtx); 

            ctx.source
                ?.GetComponent<Entity>()
                ?.eventHandler
                ?.RaiseEvent(hitCtx); 
        }

        if (ctx.target.GetComponent<Entity>() == null) return;
    }

    public void Heal(EffectContext context, float amount)
    {
        var damageable = context.target.GetComponent<IDamageable>();
        damageable.Heal(amount);
        context.target.RaiseEvent(CombatEvent.OnHeal, context);
    }

    public void KnockBack(EffectContext context, float magnitude)
    {
        var damageable = context.target.GetComponent<IDamageable>();
        damageable.KnockBack(magnitude, context.source.transform.position);
    }
}