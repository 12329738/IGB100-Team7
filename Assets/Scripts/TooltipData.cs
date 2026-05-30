using UnityEngine;

public class TooltipData : MonoBehaviour
{
    [Header("Tooltip")]
    public string itemName;
    
    [TextArea(3,5)]
    public string description;

    [TextArea(3,10)]
    public string statsText;
}