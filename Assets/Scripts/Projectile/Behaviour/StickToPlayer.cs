using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/StickToPlayer")]
public class StickToPlayer : WeaponBehaviour
{
    public float offset;
    public bool usePlayerAsBase;

    public override void OnProjectileCreated(Projectile proj)
{
    if (usePlayerAsBase)
    {
        Transform player = proj.data.owner.transform;

        proj.transform.position =player.transform.position + new Vector3(0.1f, 0, 0);


    }
}
    internal override void Move(Projectile proj, IProjectileState state)
    {
        Transform player = GameManager.instance.player.transform;

        if (usePlayerAsBase)
        {
            float speed = proj.data.stats[StatType.MoveSpeed];


            proj.transform.RotateAround(player.position, Vector3.up, speed * Time.deltaTime);


            Vector3 direction = proj.transform.position - player.position;


            proj.transform.rotation = Quaternion.LookRotation(direction);

        }
        else
        {
            proj.transform.position = player.position + Vector3.up * offset;
        }
    }

}
