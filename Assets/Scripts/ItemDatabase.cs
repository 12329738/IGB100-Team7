using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponData> items;

    public Dictionary<ItemList, WeaponData> itemDictionary;


    public void Initialize()
    {
        CreateDictionary();      
        CreateItemUpgrades();
    }
    public void CreateDictionary()
    {
        itemDictionary = new Dictionary<ItemList, WeaponData>();
        foreach (WeaponData weaponData in items)
        {
            itemDictionary[weaponData.itemType] = weaponData;
        }
    }

    public WeaponData GetItem(ItemList itemType)
    {
        return itemDictionary[itemType];
    }


    public List<Upgrade> GetAvaliableUpgrades()
    {
        List<Upgrade> avaliableUpgrades = new List<Upgrade>();

        int upgradeChoices = GameManager.instance.upgradesPerLevel;


        foreach (WeaponData weaponData in items)
        {
            Weapon weapon = GameManager.instance.player.TryGetWeapon(weaponData.itemType);

            int level = (weapon != null) ? weapon.currentLevel : 0;

            level = Mathf.Clamp(level, 0, weaponData.upgradeList.Count);

            avaliableUpgrades.AddRange(weaponData.upgradeList[level]);
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
        foreach (WeaponData weaponData in items)
        {
            List<List<Upgrade>> upgradeList = new List<List<Upgrade>>();

            for (int i = 0; i < GameManager.instance.weaponUpgradeLimit; i++)
            {
                upgradeList.Add(new List<Upgrade>());
            }

            Item item = new Item();
            item.itemType = weaponData.itemType;
            item.Name = weaponData.name;
            item.Description = weaponData.weaponDescription;

            upgradeList[0].Add(item);

            // Distribute upgrades into levels
            foreach (ItemUpgrade upgrade in weaponData.upgrades)
            {
                if (upgrade != null)
                {
                    foreach (int level in upgrade.levelsAvaliable)
                    {
                        if (level < upgradeList.Count)
                        {
                            upgradeList[level].Add(upgrade);
                        }
                    }
                }
                
            }

            weaponData.upgradeList = upgradeList;
        }
    }

    
}
