using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Drawing;
using Color = UnityEngine.Color;


public class DamagePopup : MonoBehaviour
{
    public GameObject damageTextPrefab; // Reference to the prefab of the damage text
    public float textHeightOffset = 2f;  // Height above the enemy to show the damage
    public float floatSpeed = 1f;        // Speed at which the damage text floats upwards
    public float fadeTime = 5f;          // Time for the text to fade out
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

        Renderer renderer = target.GetComponentInChildren<Renderer>();

        Vector3 worldPosition = renderer.bounds.max + Vector3.up * textHeightOffset;

        GameObject damageText = Instantiate(damageTextPrefab, damageCanvas.transform);

        RectTransform rt = damageText.GetComponent<RectTransform>();
        rt.position = worldPosition;
        damageText.transform.forward = Camera.main.transform.forward;

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


            // Move the text upwards in world space
            damageText.transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            yield return null;
        }

        // Destroy the damage text after fading
        Destroy(damageText);
    }

   
}