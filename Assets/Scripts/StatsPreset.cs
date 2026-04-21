using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stats Preset")]
[Serializable]
public class StatsPreset : ScriptableObject
{
    public List<StatConfig> statPresets;
    
}