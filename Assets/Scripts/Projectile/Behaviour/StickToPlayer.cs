using UnityEngine;

public class StickToPlayer : WeaponBehaviour
{
    public override void OnProjectileCreated(Projectile proj)
    {

    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        proj.transform.position = GameManager.instance.player.transform.position;
    }

}
