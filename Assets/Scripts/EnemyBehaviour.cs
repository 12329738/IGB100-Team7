using UnityEngine;

public abstract class EnemyBehaviour : ScriptableObject
{
    public abstract void Move(Player player, Enemy enemy);
}