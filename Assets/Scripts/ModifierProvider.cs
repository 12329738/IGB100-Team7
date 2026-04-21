using System;
using System.Collections.Generic;

public class ModifierProvider : IModifierProvider
{
    public event Action OnDirty;

    protected List<StatModifier> modifiers = new();
    protected List<IModifierProvider> children = new();

    
    public List<StatModifier> Modifiers { get => modifiers; set => modifiers = value; } 

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        MarkDirty();
    }

    public void RemoveModifier(StatModifier mod)
    {
        if (modifiers.Remove(mod))
            MarkDirty();
    }

    public void AddChild(IModifierProvider child)
    {
        if (children.Contains(child)) return;

        children.Add(child);
        child.OnDirty += HandleChildDirty;

        MarkDirty();
    }

    public void RemoveChild(IModifierProvider child)
    {
        if (children.Remove(child))
        {
            child.OnDirty -= HandleChildDirty;
            MarkDirty();
        }
    }

    private void HandleChildDirty()
    {
        MarkDirty(); 
    }

    protected void MarkDirty()
    {
        OnDirty?.Invoke();
    }

    public void ForceDirty()
    {
        MarkDirty();
    }


    public IEnumerable<StatModifier> GetAllModifiers()
    {
        foreach (var mod in modifiers)
            yield return mod;

        foreach (var child in children)
        {
            if (child is ModifierProvider mp)
            {
                foreach (var mod in mp.GetAllModifiers())
                    yield return mod;
            }
            else
            {
                foreach (var mod in child.Modifiers)
                    yield return mod;
            }
        }
    }
}