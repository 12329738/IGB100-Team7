using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Combat : MonoBehaviour
{
    private Dictionary<(object damageId, GameObject target), float> lastHitTimes = new();

    public void DealDamage(EffectContext context)
    {
        var key = (context.valueId, context.target);


        int source = context.valueId.GetHashCode();
        int target = context.target.GetInstanceID();
        if (lastHitTimes.TryGetValue(key, out float lastHit))
        {
            if (Time.time - lastHit < context.hitInterval)
                return;
        }

        lastHitTimes[key] = Time.time;

        var damageable = context.target.GetComponent<IDamageable>();
        if (damageable == null || !damageable.IsDamageable())
            return;

        context.target.RaiseEvent(CombatEvent.OnDamageTaken, context);
        damageable.TakeDamage(context);
        

        if (context.isHit)
        {
            context.source.RaiseEvent(CombatEvent.OnHit, context);
        }
        
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