using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    private Dictionary<CombatEvent, List<EffectRuntime>> eventMap = new();
    private readonly List<EffectRuntime> activeEffects = new();

    private void Update()
    {
        float now = Time.time;

        for (int i = 0; i < activeEffects.Count; i++)
        {
            var effect = activeEffects[i];

            if (effect.ShouldTick(now))
            {
                effect.MarkTick(now);

                var ctx = new EffectContext
                {
                    source = effect.source,
                    target = effect.target,
                    trigger = CombatEvent.OnTick,
                    stacks = effect.stacks
                };

                effect.definition.Execute(ctx);
                GameManager.instance.effectExecutor.Execute(ctx);
            }
        }
    }
    public void Dispatch(EffectContext ctx)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            var effect = activeEffects[i];

            if (!effect.subscribedEvents.Contains(ctx.trigger))
                continue;

            var localCtx = ctx.Clone();
            localCtx.stacks = effect.stacks;
            localCtx.source = effect.source;
            localCtx.target = ctx.target;

            effect.definition.Execute(localCtx);
            GameManager.instance.effectExecutor.Execute(localCtx);
        }
    }

public void Register(EffectRuntime runtime)
    {
        activeEffects.Add(runtime);
    }

    public void Unregister(EffectRuntime runtime)
    {
        activeEffects.Remove(runtime);
    }
}