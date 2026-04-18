using System;
using UnityEngine;

[Serializable]
public class SpawnProjectileConfig : EffectNodeConfig
{
    public Weapon projectile;

    public override EffectNodeType Type => EffectNodeType.SpawnProjectile;
    public override void Execute(EffectContext ctx)
    {
        ProjectileData data = new ProjectileData(projectile);

        GameManager.instance.projectileSpawner.CreateProjectile(data);
    }
}