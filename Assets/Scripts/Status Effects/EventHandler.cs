using System;
using UnityEngine;

public class EventHandler 
{
    public event Action<EffectContext> OnEvent;

    public void RaiseEvent(EffectContext context)
    {
        OnEvent?.Invoke(context);
    }
}
