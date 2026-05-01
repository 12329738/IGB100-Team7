using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Video.VideoPlayer;

public class Combat : MonoBehaviour
{
    private Dictionary<(object damageId, IDamageSource target), float> lastHitTimes = new();

    public void DealDamage(CombatIntent intent)
    {
        if (!CanHit(intent))
            return;

        IDamageable damageable = intent.context.target as IDamageable;

        if (damageable == null || !damageable.IsDamageable())
            return;

        IModifierReceiver owner = null;
        Projectile proj = null;



        if (intent.context.damageSourceOwner is IModifierReceiver r)
            owner = r;

        if (intent.context.damageSource is Projectile p)
            proj = p;
        intent.context.trigger = CombatEvent.OnDamage;

        if (intent.context.definition.usesValueSource)
        {
            intent.value = intent.context.definition.source.Evaluate(intent);
        }

        if (!intent.context.definition.ignoreModifiers)
        {
            GameManager.instance.effectHandler.Modify(intent.context, ref intent);
        }

        if (proj != null)
        {
            if (proj.stats.TryGetValue(StatType.CritChance, out float critChance))
            {
                bool isCrit = UnityEngine.Random.Range(0f, 100f) < critChance;
                intent.context.isCrit = isCrit;
                if (isCrit)
                    intent.value *= 1 + (proj.stats[StatType.CritDamage] / 100);
            }
            
        }
        

        damageable.TakeDamage(intent);
        DamagePopup.instance.ShowCombatText(intent);

        if (intent.context.isHit)
        {
            EffectContext ctx = intent.context.Clone();
            ctx.trigger = CombatEvent.OnHit;
            GameManager.instance.effectHandler.Dispatch(ctx);

            if (ctx.damageSourceOwner is Component comp)
            comp.gameObject.GetComponent<StatusEffectManager>().Dispatch(ctx);
        }

    }

    public void Heal(CombatIntent intent)
    {
        if (intent.context.target is IDamageable damageable)
        {
            damageable.Heal(intent.value);
            DamagePopup.instance.ShowCombatText(intent);
        }



    }

    public void KnockBack(CombatIntent intent)
    {
        if (intent.context.target is IDamageable damageable)
            if (intent.source.owner is Component comp)
            damageable.KnockBack(intent.value, comp.gameObject.transform.position);
    }

    internal void TriggerContact(CombatIntent intent)
    {
        if (!CanHit(intent))
            return;

        if (intent.target is Component comp)
            GameManager.instance.effectHandler.Dispatch(intent.context);
    }

    public bool CanHit(CombatIntent intent)
    {
        float lastHit;
        if (intent.context.effectInstance != null)
        {
            var effectKey = (intent.context.effectInstance, intent.context.target);
            if (lastHitTimes.TryGetValue(effectKey, out lastHit))
            {
                if (Time.time - lastHit < intent.context.hitInterval)
                    return false;
            }
            lastHitTimes[effectKey] = Time.time;
            return true;
        }

        else if (intent.context.sourceInstanceId != null)
        {

            var sourceKey = (intent.context.sourceInstanceId, intent.context.target);

            if (lastHitTimes.TryGetValue(sourceKey, out lastHit))
            {
                if (Time.time - lastHit < intent.context.hitInterval)
                    return false;
            }
            lastHitTimes[sourceKey] = Time.time;
            return true;
        }

        return false;
    }
}