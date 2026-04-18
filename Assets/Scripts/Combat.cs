using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Combat : MonoBehaviour
{
    public event Action<CombatEvent, EffectContext> OnEvent;
    private Dictionary<(object damageId, GameObject target), float> lastHitTimes = new();

    public void Damage(EffectContext context)
    {
        var key = (context.damageId, context.target);
        Debug.Log(
    $"DamageID: {context.damageId.GetHashCode()} | Target: {context.target.GetInstanceID()}"

);
        int source = context.damageId.GetHashCode();
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

        damageable.TakeDamage(context);
        


        //CombatEventBus.Raise(CombatEvent.Hit, context);
        if (context.isHit)
        {
            OnEvent?.Invoke(CombatEvent.Hit, context);
        }
        
    }
}