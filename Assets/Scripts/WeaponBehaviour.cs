using UnityEngine;


public abstract class WeaponBehaviour : ScriptableObject
{
    internal abstract void Move(Projectile proj, IProjectileState state);
    public abstract void OnProjectileCreated(Projectile proj);
    
    //public abstract void CreateProjectile(Weapon weapon, GameObject prefab, Team team);
}
