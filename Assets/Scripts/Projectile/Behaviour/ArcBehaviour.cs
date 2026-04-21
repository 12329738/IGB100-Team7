using NUnit;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/Arc")]
public class ArcBehaviour : WeaponBehaviour
{
    public float height;
    public float duration;
    public override void OnProjectileCreated(Projectile proj)
    {
        proj.state = new ArcState
        {
            start = proj.transform.position,
            end = proj.data.finalDirection,
            time = 0f
        };

    }

    internal override void Move(Projectile proj, IProjectileState state)
    {
        ArcState a = (ArcState)state;

        a.time += Time.deltaTime / duration;

        float t = Mathf.Clamp01(a.time);

        Vector3 dir = (a.end - a.start).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, dir);

        Vector3 pos = Vector3.Lerp(a.start, a.end, t);

        float arc = Mathf.Sin(t * Mathf.PI) * height;
        pos += right * arc;

        proj.transform.position = pos;

        if (t >= 1f)
            proj.Deactivate();
    }
}
