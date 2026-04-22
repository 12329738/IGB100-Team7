using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Upgrade/Item Upgrade")]
[Serializable]
public class ItemUpgrade : Upgrade
{
    public ItemList itemType;
    public List<int> levelsAvaliable;
}





