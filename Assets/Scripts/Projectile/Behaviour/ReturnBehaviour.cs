using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/Return")]
public class ReturnBehaviour : WeaponBehaviour
{
    public float damageMultiplierOnReturn;
    public float speedMultiplierOnReturn;
    public override void OnContact(Projectile proj, Collider other)
    {
        
    }

    public override void OnProjectileCreated(Projectile proj)
    {
        proj.data.isPiercing = true;    
        proj.stats[StatType.Duration] *= 2;
        proj.state = new ReturnState();
    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        ReturnState s = (ReturnState)state;
        Vector3 direction = proj.data.finalDirection;

        if (proj.elapsed >= proj.stats[StatType.Duration] / 2 && !s.hasReturned)
        {
            
            s.hasReturned = true;
            proj.stats[StatType.Damage] *= damageMultiplierOnReturn;
            proj.stats[StatType.MoveSpeed] *= speedMultiplierOnReturn;
            proj.guid = Guid.NewGuid();
        }

        if (s.hasReturned)
        {
            float step = proj.stats[StatType.MoveSpeed] * Time.deltaTime;
            Vector3 toOwner = proj.owner.transform.position - proj.transform.position;

            if (toOwner.magnitude <= step)
            {
                proj.transform.position = proj.owner.transform.position;
                proj.Deactivate(); 
            }
            else
            {
                proj.transform.position += toOwner.normalized * step;
                proj.transform.rotation = Quaternion.LookRotation(toOwner.normalized);
            }
        }

        else
        proj.transform.position += direction * proj.stats[StatType.MoveSpeed] * Time.deltaTime;

    }

}
