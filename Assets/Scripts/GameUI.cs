using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject levelUpScreenPrefab;
    public GameObject levelUpScreen;
    public Image playerHealthBar;
    public Image transformationBar;
    public Image miniHealthBar;
    public Image playerExperienceBar;
    public UpgradeUI upgradeUI;
    public TextMeshProUGUI displayedPlayerHealth;
    public TextMeshProUGUI displayedPlayerLevel;
    public TextMeshProUGUI playerStats;
    public TextMeshProUGUI weaponStats;
    private IDamageable trackedDamageablePlayer;
    private Player player;

    public void Awake()
    {
        player = GameManager.instance.player;
        trackedDamageablePlayer = player.GetComponent<IDamageable>();
    }
    public void ShowUpgradeOptions(List<Upgrade> upgrades)
    {
        levelUpScreen = Instantiate(levelUpScreenPrefab);

        levelUpScreen.transform.SetParent(this.transform, false);

        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradeUI upgrade = Instantiate(upgradeUI, levelUpScreen.transform);
            upgrade.Initialize(upgrades[i]);
        }
    }

    public void HideLevelUpScreen()
    {
        Destroy(levelUpScreen);
    }

    private void LateUpdate()
    {
        DisplayHealthAmount();
        DisplayTransformationAmount();
        DisplayExperienceAmount();
        DisplayPlayerStats();
        DisplayWeaponStats();
    }

    private void DisplayWeaponStats()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Weapon weapon in GameManager.instance.player.weapons)
        {
            sb.AppendLine($"{weapon.itemType}");
            foreach (var kvp in weapon.weaponStats.statDictionary)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value.currentValue}");
            }
        }
        weaponStats.text = sb.ToString();
    }

    private void DisplayPlayerStats()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in GameManager.instance.player.stats.statDictionary)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value.currentValue}");
        }
        playerStats.text = sb.ToString();
    }

    private void DisplayTransformationAmount()
    {
        transformationBar.fillAmount = player.currentTransformationAmount / player.stats.GetStat(StatType.MaxTransformation).currentValue;
    }

    private void DisplayExperienceAmount()
    {
        playerExperienceBar.fillAmount = player.currentExperience / GameManager.instance.GetExperienceAtLevel(player.level);
        displayedPlayerLevel.text = $"Level {player.level.ToString()} {player.currentExperience}/{GameManager.instance.GetExperienceAtLevel(player.level)}";
    }

    public void DisplayHealthAmount()
    {
        miniHealthBar.fillAmount = trackedDamageablePlayer.GetCurrentHealthPercent();
        playerHealthBar.fillAmount = trackedDamageablePlayer.GetCurrentHealthPercent();
        int currentHealth = Convert.ToInt32(Math.Floor(trackedDamageablePlayer.GetCurrentHealth()[0]));
        int maxHealth = Convert.ToInt32(Math.Floor(trackedDamageablePlayer.GetCurrentHealth()[1]));
        displayedPlayerHealth.text = $"{currentHealth}/{maxHealth}";
    }
}
