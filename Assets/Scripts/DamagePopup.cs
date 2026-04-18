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
    public float floatSpeed = 1f;        
    public float fadeTime = 5f;         
    public static DamagePopup instance;
    public Canvas damageCanvas;
    public enum CombatTextType { EnemyDamage,PlayerDamage,Healing}

    void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public void ShowCombatText(EffectContext context)
    {
        GameObject target = context.target;

        SpriteRenderer renderer = target.GetComponentInChildren<SpriteRenderer>();

        Vector3 topCenter = new Vector3(
            renderer.bounds.center.x,
            renderer.bounds.center.y,
            renderer.bounds.max.z
        );
        Vector3 worldPosition = topCenter + target.transform.up * textHeightOffset;

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
        text.text = context.damage.ToString();

        text.color = (context.target == GameManager.instance.player.gameObject)
            ? Color.red
            : Color.white;

        StartCoroutine(FadeOut(damageText, target));
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