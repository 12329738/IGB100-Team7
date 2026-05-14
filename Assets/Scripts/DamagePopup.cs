using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Color = UnityEngine.Color;


public class DamagePopup : MonoBehaviour
{
    public DamagePopupText damageTextPrefab; 
    public float textHeightOffset = 2f;  
    public float floatSpeed = 10f;        
    public float fadeTime = 10f;
    public float textSize = 50;
    public static DamagePopup instance;
    public Canvas damageCanvas;
    public Vector3 cameraForward;

    void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        cameraForward = GameManager.instance.camera.transform.forward;
    }

    public void ShowCombatText(CombatIntent intent)
    {

        if (intent.value < 1)
            return;
        Component comp = intent.target as Component;

        SpriteRenderer renderer = comp.gameObject.GetComponentInChildren<SpriteRenderer>();

        Vector3 worldPosition =
            renderer.bounds.center +
            comp.transform.up * textHeightOffset;

        GameObject damageText = ObjectPool.instance.GetObject(damageTextPrefab.gameObject);
        damageText.transform.SetParent(damageCanvas.transform, false);
        DamagePopupText textPopup = damageText.GetComponent<DamagePopupText>();


        float size = textSize;
        Color color = (comp.gameObject == GameManager.instance.player.gameObject)
            ? Color.red
            : intent.source.definition.textColour;

        if (intent.intent == EffectIntent.Heal)
        {
            color = Color.green;
        }

        else if (intent.context.isCrit)
        {
            color = Color.yellow;
            size = textSize * 1.5f;
        }
        textPopup.Setup(cameraForward, worldPosition,color,size,fadeTime, floatSpeed);
        textPopup.text.SetText("{0:0}", intent.value);

    }

    public void ShowStatusEffect(StatusEffectInstance instance)
    {


        Component comp = instance.context.target as Component;

        SpriteRenderer renderer = comp.gameObject.GetComponentInChildren<SpriteRenderer>();

        Vector3 worldPosition =
            renderer.bounds.center +
            comp.transform.up * textHeightOffset;

        GameObject damageText = ObjectPool.instance.GetObject(damageTextPrefab.gameObject);
        damageText.transform.SetParent(damageCanvas.transform, false);
        DamagePopupText textPopup = damageText.GetComponent<DamagePopupText>();

        float size = textSize;
       
        textPopup.Setup(cameraForward, worldPosition, Color.white, size, fadeTime, floatSpeed);
        textPopup.text.SetText(instance.data.name);
    }



   
}