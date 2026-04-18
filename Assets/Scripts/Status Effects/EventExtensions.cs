using UnityEngine;

public static class EventExtensions
{
    public static void RaiseEvent(this GameObject obj, CombatEvent type, EffectContext ctx)
    {
        obj.GetComponent<EventHandler>()?.RaiseEvent(type, ctx);
    }
}