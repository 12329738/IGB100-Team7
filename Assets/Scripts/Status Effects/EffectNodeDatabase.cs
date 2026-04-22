using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EffectNodeDatabase
{
    private static Dictionary<EffectType, System.Type> map = new();

    public static void Register(EffectType type, System.Type t)
    {
        map[type] = t;
    }

    public static EffectNodeConfig Create(EffectType type)
    {
        if (!map.TryGetValue(type, out var t))
            return null;

        return (EffectNodeConfig)System.Activator.CreateInstance(t);
    }

    public static Type Get(EffectType type)
    {
        if (!map.TryGetValue(type, out var t))
            return null;

        return t;
    }

    public static void ScanAndRegister()
    {
        var baseType = typeof(EffectNodeConfig);

        foreach (var t in System.AppDomain.CurrentDomain.GetAssemblies()
                     .SelectMany(a => a.GetTypes()))
        {
            if (t.IsAbstract || !baseType.IsAssignableFrom(t))
                continue;

            var instance = (EffectNodeConfig)System.Activator.CreateInstance(t);
            Register(instance.Type, t);
        }
    }

    [InitializeOnLoadMethod]
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    static void Init()
    {
        EffectNodeDatabase.ScanAndRegister();
    }
}