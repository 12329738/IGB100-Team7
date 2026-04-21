using System;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class SpawnProjectileConfig : EffectNodeConfig
{
    public Weapon projectile;

    public override EffectNodeType Type => EffectNodeType.SpawnProjectile;
    public override void Execute(EffectContext ctx)
    {
        projectile.Initialize();
        IModifierProvider provider;
        provider = projectile.owner.GetComponent<IModifierProvider>();

        projectile.stats.AddModifierProvider(provider);
  
        projectile.Spawnprojectiles();
    }   
}