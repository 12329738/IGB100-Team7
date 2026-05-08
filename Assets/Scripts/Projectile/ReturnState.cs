using UnityEngine;

public class ReturnState : IProjectileState
{
    public bool hasReturned;
    internal Vector3 returnStartOffset;
    internal float returnDuration;
}
