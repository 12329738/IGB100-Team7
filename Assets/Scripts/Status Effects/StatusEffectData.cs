using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public string name;
    public string description;
    public float duration;
    public float maxStacks;
    public List<NodeEntry> entries = new List<NodeEntry>();

    private void OnValidate()
    {
        if (entries == null) return;

        foreach (var entry in entries)
        {
            if (entry == null) continue;

            entry.Validate();
        }
    }
}

