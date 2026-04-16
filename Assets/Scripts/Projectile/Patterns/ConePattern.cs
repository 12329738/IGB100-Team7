using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Pattern/Cone")]
public class ConePattern : ProjectilePattern
{
    public float spreadAngle;
    public override Vector3 ConfigureBase(int index, int count, ProjectileData data)
    {
        float step = spreadAngle / (count - 1);
        float angle = -spreadAngle / 2f + step * index + data.baseDirection;

        return Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }
}
