using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Behaviours/Straight Movement")]
public class StraightMovement : WeaponBehaviour
{
    public override void OnProjectileCreated(Projectile proj)
    {
       
    }

    public override void OnContact(Projectile proj, Collider other)
    {
        throw new System.NotImplementedException();
    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        proj.transform.position += proj.data.finalDirection.normalized * proj.stats[StatType.MoveSpeed] * Time.deltaTime;
    }


}
