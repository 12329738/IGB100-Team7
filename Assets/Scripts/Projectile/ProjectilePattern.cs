using UnityEngine;

public abstract class ProjectilePattern : ScriptableObject
{
    public abstract Vector3 ConfigureBase(int index, int count, ProjectileData data);
}