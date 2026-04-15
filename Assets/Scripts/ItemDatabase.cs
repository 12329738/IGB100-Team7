using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemDatabase 
{
    public List<Item> itemDatabase;

    public Dictionary<ItemList, Item> itemDictionary;


    public void Initialize()
    {
        CreateItemDatabase();
        CreateDictionary();      
        CreateItemUpgrades();

    }

    private void CreateItemDatabase()
    {
        Item[] items = Resources.LoadAll<Item>("Items");
        itemDatabase = new List<Item>(items);
    }

    public void CreateDictionary()
    {
        itemDictionary = new Dictionary<ItemList, Item>();
        foreach (Item item in itemDatabase)
        {
            itemDictionary[item.itemType] = item;
        }
    }

    public Item GetItem(ItemList itemType)
    {
        return itemDictionary[itemType];
    }


    public List<Upgrade> GetAvaliableUpgrades()
    {
        List<Upgrade> avaliableUpgrades = new List<Upgrade>();

        int upgradeChoices = GameManager.instance.upgradesPerLevel;


        foreach (Item item in itemDatabase)
        {
            Item playerItem = GameManager.instance.player.TryGetItem(item.itemType);

            if (playerItem == null && item is Weapon weapon)
            {
                avaliableUpgrades.Add(weapon.baseUpgrade);
            }

            else
            {
                int level = (playerItem != null) ? playerItem.currentLevel : 1;

                avaliableUpgrades.AddRange(item.upgradeTree[level-1]);
            }


        }

 
        List<Upgrade> chosenUpgrades = new List<Upgrade>();
        List<Upgrade> pool = new List<Upgrade>(avaliableUpgrades);

        for (int i = 0; i < upgradeChoices && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);

            int rarity = Random.Range(0, 10);

            chosenUpgrades.Add(pool[index]);
            pool.RemoveAt(index); 
        }

        return chosenUpgrades;
    }


    private void CreateItemUpgrades()
    {
        foreach (Item item in itemDatabase)
        {
            List<List<Upgrade>> upgradeList = new List<List<Upgrade>>();

            for (int i = 0; i < GameManager.instance.weaponUpgradeLimit; i++)
            {
                upgradeList.Add(new List<Upgrade>());
            }

            if (item is Weapon weapon)
            {
                weapon.CreateBaseUpgrade();
            }
      

            foreach (Upgrade itemUpgrade in item.upgrades)
            {
                if (itemUpgrade != null)
                {
                    foreach (int level in itemUpgrade.levelsAvaliable)
                    {
                        if (level < upgradeList.Count)
                        {
                            upgradeList[level-1].Add(itemUpgrade);
                        }
                    }
                }
                
            }

            item.upgradeTree = upgradeList;
        }
    }

    
}
