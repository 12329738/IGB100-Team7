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
        background.color = GameManager.instance.rarityColors[upgrade.rarity];
        if (upgrade is TransformationUpgrade)
        {
            background.color = GameManager.instance.rarityColors[RarityEnum.Transformation];
        }
        
        if (upgrade is ItemUpgrade itemUpgrade)

        {
            sb.AppendLine($"{GameManager.instance.database.GetDescription(itemUpgrade.itemType)}");
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
