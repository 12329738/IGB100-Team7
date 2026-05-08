using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DealDamageToArea : EffectOperation
{
    public override EffectIntent Type => EffectIntent.DealDamageToArea;
    public float damageMultiplier;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        CombatIntent intent = new CombatIntent
        {
            intent = EffectIntent.DealDamageToArea,
            source = ctx.damageSource,
            context = ctx
        };
        if (ctx.damageSource is Projectile proj)
        {
            proj.guid = Guid.NewGuid();
            intent.value = proj.stats[StatType.Damage] * damageMultiplier;
            var shape = proj.GetComponent<IProjectileShape>();

            Collider[] hits = shape.GetColliders();
            foreach (Collider collider in hits)
            {
                if (collider.gameObject.GetComponent<Player>() != null)
                    continue;
                if (collider.gameObject.TryGetComponent(out Projectile projTarget))
                    continue;
                if (!collider.gameObject.TryGetComponent(out IDamageSource target))
                    continue;


                CombatIntent hit = new CombatIntent();
                hit = intent;
                hit.target = target;
                intents.Add(hit);

            }
        }
    }
}
