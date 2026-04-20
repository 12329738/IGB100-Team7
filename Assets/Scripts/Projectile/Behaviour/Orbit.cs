using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/Orbit")]
public class Orbit : WeaponBehaviour
{
    
    public float radius;

    public override void OnProjectileCreated(Projectile proj)
    {
        float angle = Mathf.Atan2(proj.projectileData.finalDirection.z, proj.projectileData.finalDirection.x);

        proj.state = new OrbitState
        {
            angle = angle
        };

        SetPosition(proj);
    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        OrbitState s = (OrbitState)state;

        s.angle += proj.projectileData.stats.GetStat(StatType.MoveSpeed) * Time.deltaTime;

        float x = Mathf.Cos(s.angle) * radius;
        float z = Mathf.Sin(s.angle) * radius;

        proj.transform.position = GameManager.instance.player.transform.position + new Vector3(x, 0, z);
    }

    private void SetPosition(Projectile proj)
    {
        OrbitState s = (OrbitState)proj.state;
        float x = Mathf.Cos(s.angle) * radius;
        float z = Mathf.Sin(s.angle) * radius;

        proj.transform.position =
            GameManager.instance.player.transform.position + new Vector3(x, 0, z);
    }
}
