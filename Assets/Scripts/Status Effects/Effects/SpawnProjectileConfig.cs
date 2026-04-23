using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class SpawnProjectileConfig : EffectOperation
{
    public Weapon projectile;
    public string effect = "Spawn Projectile";

    public override EffectIntent Type => EffectIntent.SpawnProjectile;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        ctx.intent = EffectIntent.SpawnProjectile;
        projectile.Initialize();  
        ctx.payload.weapon = projectile;
        //IModifierProvider provider;
        //provider = projectile.owner.GetComponent<IModifierProvider>();
        //if (provider != null)
        //{
        //    projectile.stats.AddModifierProvider(provider);

        //    projectile.SpawnProjectiles(ctx);
        //}
        
    }   
}