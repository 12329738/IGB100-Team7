using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject
{
    [HideInInspector]
    public int currentLevel = 1;

    public List<Upgrade> _upgrades;
    public virtual List<Upgrade> upgrades { get => _upgrades; set => _upgrades = value; }

    public virtual List<List<Upgrade>> upgradeTree { get; set; }
    public ItemList _itemType;
    
    public virtual ItemList itemType { get => _itemType; set => _itemType = value; }

    public string _name;
    public virtual string name { get => _name; set => _name = value; }

    public string _description;
    public virtual string description { get => _description; set => _description = value; }

    public Dictionary<Guid, StatModifier> modifiers = new Dictionary<Guid, StatModifier>();


}
