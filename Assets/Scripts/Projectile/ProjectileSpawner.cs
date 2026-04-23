using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;


public class ProjectileSpawner
{

    public void CreateProjectile(ProjectileData baseData, int count)
    {

        for (int i = 0; i < count; i++)
        {
            GameObject obj = ObjectPool.instance.GetObject(baseData.prefab);
            Projectile proj = obj.GetComponent<Projectile>();

            ProjectileData data = baseData.Clone();
            proj.transform.localScale = baseData.prefab.transform.localScale;
            proj.Initialize(data);

            Vector3 origin = proj.data.owner.transform.position;

            proj.transform.position = origin;
            proj.transform.rotation = Quaternion.identity;

            Enemy target = null;
            Vector3 finalDirection = proj.data.finalDirection;
            if (proj.data.trackEnemy || proj.data.aimAtEnemy)
            {
                 target = GetClosestEnemy(origin, proj.TryGetStat(StatType.Range));
            }
            

            if (target != null)
            {
                if (proj.data.trackEnemy)
                {
                    target = GetClosestEnemy(origin, proj.TryGetStat(StatType.Range));

                    {
                        Vector3 direction = target.transform.position - origin;
                        finalDirection = direction;
                    }
                }

                else if (proj.data.aimAtEnemy)
                {
                    target = GetClosestEnemy(origin, proj.TryGetStat(StatType.Range));


                    Vector3 direction = target.transform.position - origin;
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                    proj.data.baseDirection = angle;

                }
            }

            if (proj.data.randomDirection)
            {
                Vector2 random = Random.insideUnitCircle * proj.TryGetStat(StatType.Range);

                finalDirection = origin + new Vector3(random.x, 0f, random.y);
            }

            if (proj.data.pattern != null)
            {               
                finalDirection = proj.data.pattern.ConfigureBase(i, count, proj.data);     
            }

            

            proj.data.finalDirection = finalDirection;
            proj.transform.rotation = Quaternion.LookRotation(finalDirection);


            if (proj.data.behaviour != null)
            {
                proj.data.behaviour.OnProjectileCreated(proj);
            }
        }

    }
    public Enemy GetClosestEnemy(Vector3 position, float range)
    {
        Collider[] hits = Physics.OverlapSphere(position, range, LayerMask.GetMask("Enemy"));

        Enemy closestEnemy = null;
        float closestDist = float.MaxValue;

        Vector3 origin = position;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var enemy))
            {
                float dist = (enemy.transform.position - origin).sqrMagnitude;

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;

    }

}

