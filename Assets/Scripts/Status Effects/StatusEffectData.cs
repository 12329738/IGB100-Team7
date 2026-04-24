using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public string name;
    public string description;
    public float duration;
    public int maxStacks;
    public DamageSourceDefinition definition;
    
    public bool hasTick = true;
    public float tickInterval = 0.5f;
    public List<EffectEntryNode> entries = new List<EffectEntryNode>();


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

