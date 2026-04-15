using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject levelUpScreenPrefab;
    public GameObject levelUpScreen;
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public UpgradeUI upgradeUI;
    public TextMeshProUGUI displayedPlayerHealth;
    public TextMeshProUGUI displayedPlayerLevel;
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
        DisplayExperienceAmount();
    }

    private void DisplayExperienceAmount()
    {
        playerExperienceBar.fillAmount = player.currentExperience / GameManager.instance.GetExperienceAtLevel(player.level);
        displayedPlayerLevel.text = $"Level {player.level.ToString()} {player.currentExperience}/{GameManager.instance.GetExperienceAtLevel(player.level)}";
    }

    public void DisplayHealthAmount()
    {
        playerHealthBar.fillAmount = trackedDamageablePlayer.GetCurrentHealthPercent();
        int currentHealth = Convert.ToInt32(Math.Floor(trackedDamageablePlayer.GetCurrentHealth()[0]));
        int maxHealth = Convert.ToInt32(Math.Floor(trackedDamageablePlayer.GetCurrentHealth()[1]));
        displayedPlayerHealth.text = $"{currentHealth}/{maxHealth}";
    }
}
