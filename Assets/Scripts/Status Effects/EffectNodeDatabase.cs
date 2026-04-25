using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class EffectNodeDatabase
{
    private static Dictionary<EffectIntent, System.Type> map = new();

    public static void Register(EffectIntent type, System.Type t)
    {
        map[type] = t;
    }

    public static EffectOperation Create(EffectIntent type)
    {
        if (!map.TryGetValue(type, out var t))
            return null;

        return (EffectOperation)System.Activator.CreateInstance(t);
    }

    public static Type Get(EffectIntent type)
    {
        if (!map.TryGetValue(type, out var t))
            return null;

        return t;
    }

    public static void ScanAndRegister()
    {
        var baseType = typeof(EffectOperation);

        foreach (var t in System.AppDomain.CurrentDomain.GetAssemblies()
                     .SelectMany(a => a.GetTypes()))
        {
            if (t.IsAbstract || !baseType.IsAssignableFrom(t))
                continue;

            var instance = (EffectOperation)System.Activator.CreateInstance(t);
            Register(instance.Type, t);
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void EditorInit()
    {
        EffectNodeDatabase.ScanAndRegister();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RunTimeInit()
    {
        EffectNodeDatabase.ScanAndRegister();
    }
}