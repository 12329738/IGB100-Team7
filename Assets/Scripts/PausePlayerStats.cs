using TMPro;
using System.Text;
using UnityEngine;

public class PausePlayerStats : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    void Update()
    {
        if (!Time.timeScale.Equals(0))
            return;

        StringBuilder sb = new StringBuilder();

        foreach (var stat in GameManager.instance.player.stats.cachedStats)
        {
            sb.AppendLine($"{stat.Key}: {stat.Value}");
        }

        statsText.text = sb.ToString();
    }
}