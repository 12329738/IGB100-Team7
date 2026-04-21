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
        Entity owner;
        owner = projectile.owner.GetComponent<Entity>();
        if (owner != null)
        {
            foreach (var kvp in owner.stats.modifierSources)
            {
                projectile.stats.AddModifierSource(kvp.Key, kvp.Value);
            }
        }    
        projectile.Spawnprojectiles();
    }   
}