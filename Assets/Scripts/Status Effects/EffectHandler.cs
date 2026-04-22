using System.Collections.Generic;

public class EffectHandler
{
    private Dictionary<CombatEvent, List<EffectEntryNode>> eventMap = new();

    private EventHandler eventHandler;
    private EffectSystem effectSystem;

    public EffectHandler(EventHandler eventHandler)
    {
        this.eventHandler = eventHandler;
        this.effectSystem = GameManager.instance.effectSystem;

        eventHandler.OnEvent += HandleEvent;
    }

    public void AddToMap(EffectEntryNode effect)
    {
        if (!eventMap.TryGetValue(effect.trigger, out var list))
        {
            list = new List<EffectEntryNode>();
            eventMap[effect.trigger] = list;
        }

        list.Add(effect);
    }

    private void HandleEvent(EffectContext ctx)
    {
        if (!eventMap.TryGetValue(ctx.trigger, out var list))
            return;

        var snapshot = list.ToArray();

        foreach (var entry in snapshot)
        {
            var localCtx = ctx.Clone();

            entry.Execute(localCtx);         
            effectSystem.Execute(localCtx);  
        }
    }
}