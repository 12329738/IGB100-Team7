using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Pattern/Cone")]
public class ConePattern : ProjectilePattern
{
    public float spreadAngle;
    public override Vector3 ConfigureBase(int index, int count, ProjectileData data)
    {
        if (count <= 1)
        {
            float angle = data.baseDirection;
            return Quaternion.Euler(0, angle, 0) * Vector3.forward;
        }

        float step = spreadAngle / (count - 1);
        float angleOffset = -spreadAngle / 2f + step * index;

        float angleFinal = angleOffset + data.baseDirection;

        return Quaternion.Euler(0, angleFinal, 0) * Vector3.forward;
    }
}
