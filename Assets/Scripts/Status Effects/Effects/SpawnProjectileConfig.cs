using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class SpawnProjectileConfig : EffectOperation
{
    public Weapon weaponData;

    public override EffectIntent Type => EffectIntent.SpawnProjectile;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        ctx.intent = EffectIntent.SpawnProjectile;
        Weapon playerWeapon = null;
        foreach (Weapon weapon in GameManager.instance.player.weapons)
        {
            playerWeapon = weapon.subWeaponInstances.Find(w => w.definition = weaponData.definition);
            if (playerWeapon != null)
                break;
        }

        if (playerWeapon == null)
        {
            playerWeapon = weaponData;
            playerWeapon.Initialize();
        }
        ctx.payload.weapon = playerWeapon;
        playerWeapon.SpawnProjectiles(ctx);
        
        
    }
}