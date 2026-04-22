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

    public void Initialize(Upgrade upgrade)
    {
        this.upgrade = upgrade;
        StringBuilder sb = new StringBuilder();
        

        sb.AppendLine($"{this.upgrade.itemType.ToString()}");

        if (upgrade.description != "")
        {
            sb.AppendLine($"{this.upgrade.description}");
        }

        if (upgrade.modifiers  != null) 
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
