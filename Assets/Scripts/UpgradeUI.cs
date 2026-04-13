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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Upgrade u)
    {
        upgrade = u;
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{upgrade.itemType.ToString()}");

        if (u is ItemUpgrade itemUpgrade)
        {
            
            sb.AppendLine($"{itemUpgrade.rarity.ToString()}");
            foreach (StatModifier modifier in itemUpgrade.modifiers)
            {
                
                sb.AppendLine($"Increases {modifier.stat.ToString()} by {modifier.amount}");
            }
        }

        else
        {
            sb.AppendLine($"{upgrade.Description}");
        }


        string text = sb.ToString();

        uiText.text = text;
    }

    public void SelectUpgrade()
    {
        GameManager.instance.player.AddUpgrade(upgrade);
        GameManager.instance.gameUI.HideLevelUpScreen();
        GameManager.instance.player.upgradeChosen = true;
    }
}
