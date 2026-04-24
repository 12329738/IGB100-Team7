using System.Collections.Generic;
using UnityEngine;

public static class ModifierResolver
{
    public static List<StatModifier> Resolve(EffectContext ctx)
    {
        var providers = BuildProviders(ctx);

        return CollectModifiers(providers);
    }

    private static HashSet<IModifierProvider> BuildProviders(EffectContext ctx)
    {
        HashSet<IModifierProvider> set = new();

        //Add(set, ctx.damageSourceOwner);
        //Add(set, ctx.damageSource);

        //if (ctx.target is GameObject go)
        //{
        //    var status = go.GetComponent<StatusEffectManager>();
        //    if (status != null)
        //    {
        //        foreach (var p in status.GetProviders())
        //            set.Add(p);
        //    }
        //}

        //if (ctx.extraProviders != null)
        //{
        //    foreach (var p in ctx.extraProviders)
        //        set.Add(p);
        //}

        return set;
    }

    private static void Add(HashSet<IModifierProvider> set, IModifierProvider p)
    {
        if (p != null)
            set.Add(p);
    }

    private static List<StatModifier> CollectModifiers(HashSet<IModifierProvider> providers)
    {
        List<StatModifier> result = new();
        HashSet<IModifierProvider> visited = new();

        foreach (var provider in providers)
        {
            CollectRecursive(provider, visited, result);
        }

        return result;
    }

    private static void CollectRecursive(
    IModifierProvider provider,
    HashSet<IModifierProvider> visited,
    List<StatModifier> output)
    {
        if (provider == null || !visited.Add(provider))
            return;

        if (provider is ModifierProvider mp)
        {
            foreach (var mod in mp.Modifiers)
                output.Add(mod);

            foreach (var child in mp.children)
                CollectRecursive(child, visited, output);
        }
        else
        {
            foreach (var mod in provider.Modifiers)
                output.Add(mod);
        }
    }
}