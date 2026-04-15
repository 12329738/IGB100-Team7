using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stats Preset")]
[Serializable]
public class StatsPreset : ScriptableObject
{
    public List<StatConfig> statConfigs = new List<StatConfig>
    {
        new StatConfig { stat = StatType.MaxHealth},
        new StatConfig { stat = StatType.MoveSpeed },
        new StatConfig { stat = StatType.Damage },
        new StatConfig { stat = StatType.Area },
        new StatConfig { stat = StatType.Range },
        new StatConfig { stat = StatType.CritChance },
        new StatConfig { stat = StatType.CritDamage },
        new StatConfig { stat = StatType.Duration },
        new StatConfig { stat = StatType.ProjectileCount },
        new StatConfig { stat = StatType.Cooldown },
        new StatConfig { stat = StatType.Collection },
    };
    
}