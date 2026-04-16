using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
[Serializable]
public class Upgrade : ScriptableObject
{
    [HideInInspector]
    public string name;
    [HideInInspector]
    public string description;
    public List<StatModifier> modifiers;
    [HideInInspector]
    public RarityEnum rarity;
    public ItemList itemType;
    public List<int> levelsAvaliable;
}





