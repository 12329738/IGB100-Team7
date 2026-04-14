using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Item upgrade")]
public class ItemUpgrade : Upgrade
{
    public List<StatModifier> modifiers;
    [HideInInspector]
    public UpgradeRarity rarity;
    public List<int> levelsAvaliable;
}