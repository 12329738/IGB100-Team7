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

        if (intent.target is not IDamageable damageable ||
            !damageable.IsDamageable() ||
            !CanHit(intent.source, intent.target))
        {
            return;
        }

        intent.context.trigger = CombatEvent.OnDamage;
        EffectContext context = intent.context;
        IDamageSource damageSource = context.damageSource;
        DamageSourceDefinition definition = damageSource.definition;

        Projectile proj = null;

        if (damageSource is Projectile p)
            proj = p;
        

        if (definition.usesValueSource)
        {
            intent.value = definition.source.Evaluate(intent);
        }

        if (definition.ignoreModifiers)
        {
            GameManager.instance.effectHandler.Modify(intent.context, ref intent);
        }

        if (proj != null)
        {
            if (proj.stats.TryGetValue(StatType.CritChance, out float critChance))
            {
                bool isCrit = UnityEngine.Random.Range(0f, 100f) < critChance;
                intent.context.isCrit = isCrit;

                if (isCrit &&
                    proj.stats.TryGetValue(StatType.CritDamage, out float critDamage))
                {
                    intent.value *= 1f + critDamage * 0.01f;
                }
            }

        }

        if (intent.target is Entity e)
            intent.value *= e.stats.GetStat(StatType.DamageTaken);

        damageable.TakeDamage(intent);
        DamagePopup.instance.ShowCombatText(intent);

        if ((object)intent.target == GameManager.instance.player)
        {
            EffectContext ctx = intent.context.Clone();
            ctx.trigger = CombatEvent.OnDamageTaken;
            GameManager.instance.effectHandler.Dispatch(ctx);
        }

        

        if (intent.context.isHit)
        {
            EffectContext ctx = intent.context.Clone();
            ctx.trigger = CombatEvent.OnHit;
            GameManager.instance.effectHandler.Dispatch(ctx);

            if (damageSource.owner is Entity entity)
              entity.status.Dispatch(ctx);
        }

    }

    public void Heal(CombatIntent intent)
    {
        if (intent.target is IDamageable damageable)
        {
            damageable.Heal(intent.value);
            DamagePopup.instance.ShowCombatText(intent);
        }
    }

    public void KnockBack(CombatIntent intent)
    {
        if (intent.context.target is IDamageable damageable)
            if (intent.source.owner is Component comp)
                damageable.KnockBack(intent);
    }

    internal void TriggerContact(CombatIntent intent)
    {
        if (!CanHit(intent.source, intent.target))
            return;

        if (intent.target is Component comp)
            GameManager.instance.effectHandler.Dispatch(intent.context);
    }

    public bool CanHit(IDamageSource source, IDamageSource target)
    {
        float lastHit;
       
        var effectKey = (source.guid, target);
        if (lastHitTimes.TryGetValue(effectKey, out lastHit))
        {
            if (Time.time - lastHit < source.hitInterval)
                return false;
        }
        lastHitTimes[effectKey] = Time.time;
        return true;

    }

    public bool CheckHitTime(IDamageSource source, IDamageSource target)
    {
        float lastHit;

        var effectKey = (source.guid, target);
        if (lastHitTimes.TryGetValue(effectKey, out lastHit))
        {
            if (Time.time - lastHit < source.hitInterval)
                return false;
        }
        return true;

    }

}