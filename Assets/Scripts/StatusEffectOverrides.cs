using System.Collections.Generic;

public static class StatusEffectOverrides
{
    private static Dictionary<StatusEffectData, int> maxStackOverrides = new();

    public static void SetMaxStacks(StatusEffectData data, int value)
    {
        maxStackOverrides[data] = value;
    }

    public static int GetMaxStacks(StatusEffectData data)
    {
        return maxStackOverrides.TryGetValue(data, out int v)
            ? v
            : data.maxStacks;
    }
}