using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryTooltip : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public Item item;

    public Weapon weapon;

    public Passive passive;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        StringBuilder sb = new StringBuilder();


        if (weapon != null)
        {
            foreach (var stat in weapon.stats.cachedStats)
            {
                sb.AppendLine($"{stat.Key}: {stat.Value}");
            }
        }

        else if (passive != null)
        {
            Dictionary<StatType, float> totals =
                new Dictionary<StatType, float>();

            foreach (var modifier in passive.Modifiers)
            {
                if (!totals.ContainsKey(modifier.stat))
                    totals.Add(modifier.stat, 0);

                totals[modifier.stat] += modifier.amount;
            }

            foreach (var kvp in totals)
            {
                sb.AppendLine($"{kvp.Key}: +{kvp.Value}");
            }
        }

        TooltipUI.instance.Show(
            item.name,
            item.description,
            sb.ToString()
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}