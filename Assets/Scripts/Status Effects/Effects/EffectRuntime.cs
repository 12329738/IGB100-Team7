//using UnityEngine;
//using System.Collections.Generic;

//public class EffectRuntime
//{
//    public EffectEntryNode definition;

//    public GameObject owner;
//    public GameObject source;
//    public GameObject target;

//    public float lastTickTime;
//    public int stacks;

//    public HashSet<CombatEvent> subscribedEvents = new();

//    public float TickInterval => definition.tickInterval;
//    public bool HasTick => definition.hasTick;

//    public EffectRuntime(EffectEntryNode def, GameObject owner, GameObject source, GameObject target)
//    {
//        definition = def;
//        this.owner = owner;
//        this.source = source;
//        this.target = target;

//        foreach (var e in def.triggers)
//            subscribedEvents.Add(e);
//    }


//    public bool ShouldTick(float now)
//    {
//        return HasTick && (now - lastTickTime >= TickInterval);
//    }

//    public void MarkTick(float now)
//    {
//        lastTickTime = now;
//    }
//}