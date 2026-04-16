using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Pattern/Circle")]
public class CirclePattern : ProjectilePattern
{
    public float spread = 360f;

    public override Vector3 ConfigureBase(int index, int count, ProjectileData data)
    {
        float step = spread / count;
        float angle = step * index;

        Vector3 direction =
            Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

        return direction;
    }
}
