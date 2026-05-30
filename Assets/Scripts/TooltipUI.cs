using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;

    public GameObject panel;
    public RectTransform TooltipBox;

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        HideTooltip();
    }

    void Update()
    {
        if (!panel.activeSelf)
            return;

        Vector2 pos = Input.mousePosition;

        RectTransform box = TooltipBox;

        Vector2 size = box.rect.size;

        pos.x += 150f;
        pos.y -= 60f;

        if (pos.x + size.x > Screen.width)
        {
            pos.x = Input.mousePosition.x - size.x - 30f;
        }

        if (pos.x < 10f)
        {
            pos.x = 10f;
        }

        if (pos.y - size.y < 0)
        {
            pos.y = size.y + 10f;
        }

        if (pos.y > Screen.height - 10f)
        {
            pos.y = Screen.height - 10f;
        }

        panel.transform.position = pos;
    }

    public void ShowTooltip(TooltipData data)
    {
        panel.SetActive(true);

        itemNameText.text = data.itemName;
        descriptionText.text = data.description;
        statsText.text = data.statsText;
    }

    public void Show(string itemName, string description, string stats)
    {
        panel.SetActive(true);

        itemNameText.text = itemName;
        descriptionText.text = description;
        statsText.text = stats;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void HideTooltip()
    {
        panel.SetActive(false);
    }
}