using UnityEngine;

public class DestroyProjectile : WeaponBehaviour
{
    public override void OnContact(Projectile proj, Collider other)
    {
        if (other.gameObject.TryGetComponent<Projectile>(out Projectile contactProj))
        {
            if (contactProj.owner.team != proj.owner.team)
                proj.Deactivate();
        }
    }

    public override void OnProjectileCreated(Projectile proj)
    {
        throw new System.NotImplementedException();
    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        throw new System.NotImplementedException();
    }


}
