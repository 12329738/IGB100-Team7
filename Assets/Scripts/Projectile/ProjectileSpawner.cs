using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ProjectileSpawner
{

    public void CreateProjectile(ProjectileData data)
    {
        int count = (int)data.stats.GetStat(StatType.ProjectileCount).currentValue;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = ObjectPool.instance.GetObject(data.prefab);
            Projectile proj = obj.GetComponent<Projectile>();
            proj.Initialize(data);
            proj.transform.position = GameManager.instance.player.transform.position;

            if (data.pattern != null)
            {
                proj.projectileData.finalDirection = data.pattern.ConfigureBase(i, count, data);
            }

            if  (proj.projectileData.behaviour != null)
            {
                proj.projectileData.behaviour.OnProjectileCreated(proj);
            }          
        }
    }

}

