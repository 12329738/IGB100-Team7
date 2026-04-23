using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.Video.VideoPlayer;

public class Combat : MonoBehaviour
{
    private Dictionary<(object damageId, GameObject target), float> lastHitTimes = new();

    public void DealDamage(CombatIntent intent)
    {
        var effectKey = (intent.context.effectInstance, intent.context.target);
        var sourceKey = (intent.context.sourceInstanceId, intent.context.target);
        float lastHit;
        if (lastHitTimes.TryGetValue(effectKey, out lastHit))
        {
            if (Time.time - lastHit < intent.context.hitInterval)
                return;
        }
        

        else if (lastHitTimes.TryGetValue(sourceKey, out lastHit))
        {
            if (Time.time - lastHit < intent.context.hitInterval)
                return;
        }

        lastHitTimes[effectKey] = Time.time;

        var damageable = intent.target.GetComponent<IDamageable>();
        if (damageable == null || !damageable.IsDamageable())
            return;

        damageable.TakeDamage(intent);
        DamagePopup.instance.ShowCombatText(intent);

        if (intent.context.isHit)
        {
            EffectContext hitCtx = intent.context.Clone();
            hitCtx.trigger = CombatEvent.OnHit;
            GameManager.instance.effectHandler.Dispatch(intent.context);
            intent.context.source.GetComponent<StatusEffectManager>().Dispatch(hitCtx);
        }

    }

    public void Heal(CombatIntent intent)
    {
        var damageable = intent.context.target.GetComponent<IDamageable>();
        damageable.Heal(intent.value);
        DamagePopup.instance.ShowCombatText(intent);

    }

    public void KnockBack(CombatIntent intent)
    {
        var damageable = intent.target.GetComponent<IDamageable>();
        damageable.KnockBack(intent.value, intent.source.transform.position);
    }
}