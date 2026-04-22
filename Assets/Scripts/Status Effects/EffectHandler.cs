using System.Collections.Generic;

public class EffectHandler
{
    private List<EffectEntryNode> entries = new();

    public void AddToMap(EffectEntryNode node)
    {
        entries.Add(node);
    }

    public void Execute(EffectContext ctx)
    {
        foreach (var entry in entries)
            entry.Execute(ctx);
    }
}