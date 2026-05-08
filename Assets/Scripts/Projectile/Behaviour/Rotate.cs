using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/Rotate")]

public class Rotate : WeaponBehaviour
{
    public float speed;

    public override void OnProjectileCreated(Projectile proj)
    {

    }

    public override void OnContact(Projectile proj, Collider other)
    {
        throw new System.NotImplementedException();
    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        proj.transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }

}
