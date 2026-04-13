using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject levelUpScreenPrefab;
    public GameObject levelUpScreen;
    public UpgradeUI upgradeUI;
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
}
