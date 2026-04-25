using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;


public class ProjectileSpawner
{

    public void CreateProjectile(ProjectileData baseData, int count, Vector3? startPosition = null)
    {
        float range = baseData.stats.TryGetValue(StatType.Range, out var r) ? r : 0f;

        Collider[] hits = Physics.OverlapSphere(
            baseData.owner.transform.position,
            range,
            LayerMask.GetMask("Enemy")
        );

        List<Enemy> enemies = new();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var enemy))
                enemies.Add(enemy);
        }


        for (int i = 0; i < count; i++)
        {
            GameObject obj = ObjectPool.instance.GetObject(baseData.prefab);
            Projectile proj = obj.GetComponent<Projectile>();

            ProjectileData data = baseData.Clone();
            proj.Initialize(data);


            Vector3 origin = proj.data.owner.transform.position;
            if (startPosition != null)
                origin = (Vector3)startPosition;

            proj.transform.position = origin;
            proj.transform.rotation = Quaternion.identity;


            Vector3 finalDirection = proj.data.finalDirection;
            finalDirection = Vector3.forward;

            Enemy target = null;

            if (enemies.Count > 0)
            {
                target = enemies[i % enemies.Count];
            }

            
            Vector3 dir = Vector3.forward;
            float angle = proj.data.baseDirection;

            if (target != null)
            {
                Vector3 toTarget = target.transform.position - origin;
                dir = toTarget.normalized;

                angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

                finalDirection = Vector3.forward;

                if (proj.data.aimAtEnemy)
                    proj.data.baseDirection = angle;
            }

            if (proj.data.trackEnemy && target != null)
            {
                finalDirection = dir;
            }

            else
            {

                finalDirection = Quaternion.Euler(0f, proj.data.baseDirection, 0f)
                                 * Vector3.forward;
            }

            if (proj.data.pattern != null)
            {
                finalDirection = proj.data.pattern.ConfigureBase(i, count, proj.data);
            }
            


            if (proj.data.randomDirection)
            {
                Vector2 random = Random.insideUnitCircle * proj.TryGetStat(StatType.Range);

                finalDirection = origin + new Vector3(random.x, 0f, random.y);
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

