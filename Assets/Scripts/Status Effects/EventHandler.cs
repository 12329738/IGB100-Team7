using System;
using UnityEngine;

public class EventHandler 
{
    public event Action<CombatEvent, EffectContext> OnEvent;

    public void RaiseEvent(CombatEvent type, EffectContext context)
    {
        OnEvent?.Invoke(type, context);
    }
}
