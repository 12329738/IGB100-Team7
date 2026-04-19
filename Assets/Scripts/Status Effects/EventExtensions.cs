using UnityEngine;

public static class EventExtensions
{
    public static void RaiseEvent(this GameObject obj, CombatEvent type, EffectContext ctx)
    {
        obj.GetComponent<IEventHandler>()?.eventHandler.RaiseEvent(type, ctx);
        if (obj.GetComponent<IEventHandler>() is Projectile projectile && type == CombatEvent.OnHit)
        {
            projectile.projectileData.owner.GetComponent<IEventHandler>().eventHandler.RaiseEvent(type, ctx);
        }
    }
}