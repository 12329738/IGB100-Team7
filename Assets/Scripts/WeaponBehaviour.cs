using UnityEngine;


public abstract class WeaponBehaviour : ScriptableObject
{
    internal abstract void Move(GameObject obj, Vector3 direction, float moveSpeed);
}
