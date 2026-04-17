using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Combat : MonoBehaviour
{
    public event Action<CombatEvent, EffectContext> OnEvent;
    private Dictionary<(GameObject source, GameObject target), float> lastHitTimes = new();

    public void Hit(EffectContext context)
    {
        var key = (context.source, context.target);

        if (lastHitTimes.TryGetValue(key, out float lastHit))
        {
            if (Time.time - lastHit < context.hitInterval)
                return;
        }

        lastHitTimes[key] = Time.time;

        var damageable = context.target.GetComponent<IDamageable>();
        if (damageable == null || !damageable.IsDamageable())
            return;

        damageable.TakeDamage(context);
        


        CombatEventBus.Raise(CombatEvent.Hit, context);
    }
}