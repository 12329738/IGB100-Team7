using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 moveDirection;
    Team team;
    WeaponBehaviour weaponBehaviour;
    Stats stats;
    bool isPiercing;
    float direction;
    float angleVariance;
    GameObject prefabReference;


    public void Update()
    {      
        if (weaponBehaviour != null)
        {
            weaponBehaviour.Move(this.gameObject, moveDirection, stats.GetStat(StatType.MoveSpeed).currentValue);
        }
    }


    public void Activate(Weapon weapon, Team t)
    {
        stats = weapon.stats;
        prefabReference = weapon.prefab;
        weaponBehaviour = weapon.behaviour;
        isPiercing = weapon.isPiercing;
        team = t;
        direction = weapon.direction;
        angleVariance = weapon.angleVariance;

        moveDirection = CalculateDirection();

        StopAllCoroutines();

        if (stats.GetStat(StatType.Duration).currentValue > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }


    private Vector3 CalculateDirection()
    {

        float finalAngle = direction + UnityEngine.Random.Range(-angleVariance * 0.5f, angleVariance * 0.5f);

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
                if (!isPiercing)
                {
                    Deactivate();
                }
            }         
        }           
    }
}