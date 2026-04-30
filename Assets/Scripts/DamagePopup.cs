using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Drawing;
using Color = UnityEngine.Color;


public class DamagePopup : MonoBehaviour
{
    public GameObject damageTextPrefab; 
    public float textHeightOffset = 2f;  
    public float floatSpeed = 10f;        
    public float fadeTime = 10f;         
    public static DamagePopup instance;
    public Canvas damageCanvas;

    void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public void ShowCombatText(CombatIntent intent)
    {

        if (intent.value < 1)
            return;
        Component comp = intent.target as Component;

        SpriteRenderer renderer = comp.gameObject.GetComponentInChildren<SpriteRenderer>();

        Vector3 topCenter = new Vector3(
            renderer.bounds.center.x,
            renderer.bounds.center.y,
            renderer.bounds.max.z
        );
        Vector3 worldPosition = topCenter + comp.gameObject.transform.up * textHeightOffset;

        GameObject damageText = Instantiate(damageTextPrefab, damageCanvas.transform);

        RectTransform rt = damageText.GetComponent<RectTransform>();
        rt.position = worldPosition;
        damageText.transform.forward = Camera.main.transform.forward;

        Vector3 position = damageText.transform.position;
        float posx = position.x;
        float posy = 0;
        float posz = position.z;
        Vector3 newPosition = new Vector3 (posx, posy, posz);
        damageText.transform.position = newPosition;

        TextMeshProUGUI text = damageText.GetComponent<TextMeshProUGUI>();
        int amount = (int)intent.value;
        text.text = amount.ToString();

        text.color = (comp.gameObject == GameManager.instance.player.gameObject)
            ? Color.red
            : Color.white;

        if (intent.type == EffectIntent.Heal)
        {
            text.color = Color.green;
        }

        if (intent.context.isCrit)
        {
            text.color = Color.yellow;
            text.fontSize = text.fontSize * 1.5f;
        }

        StartCoroutine(FadeOut(damageText, comp.gameObject));
    }
            

    private IEnumerator FadeOut(GameObject damageText, GameObject target)
    {
        if (damageText == null) yield break;

        TextMeshProUGUI text = damageText.GetComponent<TextMeshProUGUI>();
        Color startColor = text.color;
        float elapsedTime = 0f;


        while (elapsedTime < fadeTime)
        {
            if (damageText == null) yield break;

            elapsedTime += Time.deltaTime;
            text.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1, 0, elapsedTime / fadeTime));



            damageText.transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            yield return null;
        }


        Destroy(damageText);
    }

   
}