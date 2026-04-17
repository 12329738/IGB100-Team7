using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    //Team team;
    //public WeaponBehaviour weaponBehaviour;
    //public Stats stats;
    //bool isPiercing;
    //bool hasKnockback;
    //float knockbackMagnitude;
    //public Vector3 moveDirection;
    //public float angleVariance;
    //GameObject prefabReference;
    public IProjectileState state;
    public ProjectileData projectileData;


    public void Update()
    {      
        if (projectileData.behaviour != null)
        {
            projectileData.behaviour.Move(this, state);
        }
    }


    public void Initialize(ProjectileData data)
    {
        projectileData = new ProjectileData(data); 

        //CalculateDirection();

        StopAllCoroutines();

        if (projectileData.stats.GetStat(StatType.Duration).currentValue > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }




    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(projectileData.stats.GetStat(StatType.Duration).currentValue);
        Deactivate();
    }

    void Deactivate()
    {
        ObjectPool.instance.ReturnObject(projectileData.prefab, gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target == null) return;

        if (target.team != projectileData.team)
        {
            if (target.IsDamageable())
            {
                target.TakeDamage(projectileData.stats.GetStat(StatType.Damage).currentValue);
                
                if (projectileData.hasKnockback)
                {
                    target.KnockBack(projectileData.knockbackMagnitude, transform.position);
                }

                if (!projectileData.isPiercing)
                {
                    Deactivate();
                }
            }         
        }           
    }
}