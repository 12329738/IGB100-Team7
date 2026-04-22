using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/Rotate")]

public class Rotate : WeaponBehaviour
{
    public float speed;

    public override void OnProjectileCreated(Projectile proj)
    {

    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        proj.transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }

}
