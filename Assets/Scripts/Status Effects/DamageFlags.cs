using UnityEngine;

[System.Flags]
public enum DamageFlags
{
    None = 0,
    Hit = 1 << 0,
    NotAHit = 1 << 1,
    DamageOverTime = 1 << 2,
}
