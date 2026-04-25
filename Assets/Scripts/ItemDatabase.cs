using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;

using UnityEngine;
using System.ComponentModel;


public class ItemDatabase : MonoBehaviour
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
        Player player = GameManager.instance.player;
        List<ItemUpgrade> avaliableUpgrades = new List<ItemUpgrade>();

        int upgradeChoices = GameManager.instance.upgradesPerLevel;


        foreach (Item item in itemDatabase)
        {
            Item playerItem = player.TryGetItem(item.itemType);

            if (playerItem == null)
            {
                if (item is Weapon weapon)
                {
                    if (player.weapons.Count < GameManager.instance.weaponLimit)
                    {
                        avaliableUpgrades.Add(weapon.baseUpgrade);
                    }

                    continue;
                }

                if (item is Passive)
                {
                    if (player.passives.Count >= GameManager.instance.passiveLimit)
                    {
                        continue;
                    }
                    int level = (playerItem != null) ? playerItem.currentLevel : 1;

                    avaliableUpgrades.AddRange(item.upgradeTree[level - 1]);
                    continue;
                }
            }

            else if (playerItem != null)
            {
                int level = playerItem.currentLevel;
                avaliableUpgrades.AddRange(item.upgradeTree[level - 1]);
            }
        }


        List<Upgrade> chosenUpgrades = new List<Upgrade>();
        List<ItemUpgrade> pool = new List<ItemUpgrade>(avaliableUpgrades);

        for (int i = 0; i < upgradeChoices && pool.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);

            ItemUpgrade chosenUpgrade = Instantiate(pool[index]);

            Rarity[] rarities = GameManager.instance.rarities;
            Rarity rarity = GenerateRarity(rarities);



            


            if (chosenUpgrade.modifiers != null)
            {
                chosenUpgrade.rarity = rarity.rarity;
                foreach (StatModifier modifier in chosenUpgrade.modifiers)
                {
                    modifier.amount *= rarity.valueModifier;
                }
            }
            

            chosenUpgrades.Add(chosenUpgrade);
            pool.RemoveAt(index); 
        }

        return chosenUpgrades;
    }

    public Rarity GenerateRarity(Rarity[] rarities)
    {
        float total = 0f;
        foreach (Rarity rarity in rarities)
            total += rarity.chance;

        float random = UnityEngine.Random.Range(0, total);

        float cumulative = 0f;

        for (int i = 0; i < rarities.Length; i++)
        {
            cumulative += rarities[i].chance;
            if (random < cumulative)
                return rarities[i];
        }

        return rarities[rarities.Length-1];
    }

    private void CreateItemUpgrades()
    {
        foreach (Item item in itemDatabase)
        {
            List<List<ItemUpgrade>> upgradeList = new List<List<ItemUpgrade>>();

            for (int i = 0; i < GameManager.instance.weaponUpgradeLimit; i++)
            {
                upgradeList.Add(new List<ItemUpgrade>());
            }

            if (item is Weapon weapon)
            {
                weapon.CreateBaseUpgrade();
            }
      

            foreach (ItemUpgrade itemUpgrade in item.upgrades)
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

    public string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

        return attr != null ? attr.Description : value.ToString();
    }



}
