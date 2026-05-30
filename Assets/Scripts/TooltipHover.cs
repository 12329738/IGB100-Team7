using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHover :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    TooltipData data;

    void Awake()
    {
        data = GetComponent<TooltipData>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Menus.IsPaused)
            return;

        if (TooltipUI.instance == null)
            return;

        if (data == null)
            return;

        TooltipUI.instance.ShowTooltip(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipUI.instance == null)
            return;

        TooltipUI.instance.HideTooltip();
    }
}