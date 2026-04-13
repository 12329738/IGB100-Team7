using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public ItemList itemType { get; set; }

}

[CreateAssetMenu(menuName = "Upgrade/Item")]
public class Item : Upgrade
{
    [HideInInspector]
    public List<int> levelsAvaliable = new List<int> { 0 };
}

[CreateAssetMenu(menuName = "Upgrade/Item upgrade")]
public class ItemUpgrade : Upgrade
{
    public List<StatModifier> modifiers;
    [HideInInspector]
    public UpgradeRarity rarity;
    public List<int> levelsAvaliable;
}




