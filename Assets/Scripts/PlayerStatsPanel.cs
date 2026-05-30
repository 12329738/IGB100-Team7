using TMPro;
using UnityEngine;

public class PlayerStatsPanel : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public Entity player;

    void Update()
    {
        if (player == null)
            return;

        //statsText.text =
            //"Health: " + player.MaxHealth + "\n" +
            //"Damage: " + player.Damage + "\n" +
            //"Move Speed: " + player.MoveSpeed + "\n" +
            //"Crit Chance: " + player.CritChance;
    }
}