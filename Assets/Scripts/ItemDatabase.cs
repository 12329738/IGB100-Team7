using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using static UnityEditor.Progress;

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

            Upgrade chosenUpgrade = Instantiate(pool[index]);

            List<Rarity> rarities = GameManager.instance.rarities;
            Rarity rarity = GenerateRarity(rarities);



            chosenUpgrade.rarity = rarity.rarity;


            if (chosenUpgrade.modifiers != null)
            {
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

    public Rarity GenerateRarity(List<Rarity> rarities)
    {
        float total = 0f;
        foreach (Rarity rarity in rarities)
            total += rarity.chance;

        float random = Random.Range(0, total);

        float cumulative = 0f;

        for (int i = 0; i < rarities.Count; i++)
        {
            cumulative += rarities[i].chance;
            if (random < cumulative)
                return rarities[i];
        }

        return rarities[rarities.Count-1];
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
