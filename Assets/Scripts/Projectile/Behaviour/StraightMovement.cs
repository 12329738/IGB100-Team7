using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Behaviours/Straight Movement")]
public class StraightMovement : WeaponBehaviour
{
    public override void OnProjectileCreated(Projectile proj)
    {
       
    }

    //public override void CreateProjectile(Weapon weapon, GameObject prefab, Team team)
    //{

    //    int count = (int)weapon.stats.GetStat(StatType.ProjectileCount).currentValue;

    //    float spread = weapon.projectileSpreadAngle;

    //    float step = count > 1 ? spread / (count - 1) : 0f;
    //    float start = -spread / 2f;

    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject obj = ObjectPool.instance.GetObject(prefab);
    //        Projectile proj = obj.GetComponent<Projectile>();
    //        proj.transform.position = GameManager.instance.player.transform.position;
    //        proj.baseDirection = weapon.direction + start + (step * i);

    //        proj.Initialize(weapon, team);
    //    }
    //}


    internal override void Move(Projectile proj, IProjectileState state)
    {
        proj.transform.position += proj.projectileData.finalDirection.normalized * proj.projectileData.stats.GetStat(StatType.MoveSpeed).currentValue * Time.deltaTime;
    }


}
