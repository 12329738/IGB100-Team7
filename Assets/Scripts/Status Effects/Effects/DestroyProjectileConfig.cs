using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectileConfig : EffectOperation
{
    public override EffectIntent Type => EffectIntent.DestroyProjectile;

    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
            if (ctx.damageSource.owner.team != ctx.target.owner.team)
                if (ctx.target is Projectile projectile)
                    projectile.Deactivate();
    }
}
