using System;

public static class CombatEventBus
{
    public static event Action<CombatEvent, EffectContext> OnEvent;

    public static void Raise(CombatEvent type, EffectContext ctx)
    {
        OnEvent?.Invoke(type, ctx);
    }
}