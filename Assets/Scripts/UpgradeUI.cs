using System.ComponentModel;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    Upgrade upgrade;
    public TextMeshProUGUI uiText;
    public Image background;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Upgrade upgrade)
    {
        this.upgrade = upgrade;
        StringBuilder sb = new StringBuilder();

        ItemUpgrade itemUpgrade = null;
        if (upgrade is ItemUpgrade u)
        {
            itemUpgrade = u;
            if (itemUpgrade.levelsAvaliable.Count == 0)
                upgrade.rarity = RarityEnum.Common;
        }

        background.color = GameManager.instance.rarityColors[upgrade.rarity];

        


        if (upgrade is TransformationUpgrade)
        {
            background.color = GameManager.instance.rarityColors[RarityEnum.Transformation];
        }
        
        if (upgrade is ItemUpgrade)
        {
            
            Item playerItem = GameManager.instance.player.TryGetItem(itemUpgrade.itemType);
            string level = "";
            if (itemUpgrade.levelsAvaliable.Count == 0)
            {
                
            }

            else if (playerItem == null)
            {
                level = "Level 1";
            }

            else
            {
                level = $"Level {playerItem.currentLevel}";
            }


            sb.AppendLine($"{GameManager.instance.database.GetDescription(itemUpgrade.itemType)} {level}");
        }
 

        if (!string.IsNullOrWhiteSpace(upgrade.description))
        {
            sb.AppendLine(upgrade.description);
        }

        if (upgrade.modifiers  != null && upgrade.modifiers.Count >0) 
        {

            sb.AppendLine($"{upgrade.rarity.ToString()}");
            foreach (StatModifier modifier in upgrade.modifiers)
            {
                string symbol = modifier.type == ModifierType.Percentage ? "%" : "";
                string direction = modifier.amount > 0 ? "Increases" : "Decreases";
                sb.AppendLine($"{direction} {modifier.stat.ToString()} by {Mathf.Abs(modifier.amount)}{symbol}");
            }
        }

        


        string text = sb.ToString();

        uiText.text = text;
    }

    public void SelectUpgrade()
    {
        GameManager.instance.gameUI.HideLevelUpScreen();
        GameManager.instance.player.AddUpgrade(upgrade);
        GameManager.instance.player.upgradeChosen = true;
    }
}
