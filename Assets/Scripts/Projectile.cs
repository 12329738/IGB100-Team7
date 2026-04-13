using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //float damage;
    //float lifetime;
    //float speed;
    //float direction = 0;
    //float angle = 0;
    Vector3 moveDirection;
    Team team;
    WeaponBehaviour weaponBehaviour;
    WeaponData weaponData;
    Stats stats;

    GameObject prefabReference;


    public void Update()
    {      
        if (weaponBehaviour != null)
        {
            weaponBehaviour.Move(this.gameObject, moveDirection, stats.GetStat(StatType.MoveSpeed).currentValue);
        }
    }


    public void Activate(WeaponData data, Stats s, Team t)
    {
        stats = s;
        weaponData = data;
        prefabReference = data.prefab;
        weaponBehaviour = data.behaviour;
        team = t;

        moveDirection = CalculateDirection();

        StopAllCoroutines();

        if (stats.GetStat(StatType.Duration).currentValue > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }


    private Vector3 CalculateDirection()
    {

        float finalAngle = weaponData.direction + UnityEngine.Random.Range(-weaponData.angleVariance * 0.5f, weaponData.angleVariance * 0.5f);

        return Quaternion.Euler(0f, finalAngle, 0f) * Vector3.forward;
    }

    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(stats.GetStat(StatType.Duration).currentValue);
        Deactivate();
    }

    void Deactivate()
    {
        ObjectPool.instance.ReturnObject(prefabReference, gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target == null) return;

        if (target.team != team)
        {
            if (target.IsDamageable())
            {
                target.TakeDamage(stats.GetStat(StatType.Damage).currentValue);
                if (!weaponData.isPiercing)
                {
                    Deactivate();
                }
            }         
        }           
    }
}