using System;
using System.Collections;
using UnityEngine;


public class FlashWhite : MonoBehaviour
{


    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Material whiteMaterial;
    private Coroutine flashRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        whiteMaterial = new Material(Shader.Find("GUI/Text Shader"));
        whiteMaterial.color = Color.white; 
    }   

    public void TriggerFlash()
    {
        flashRoutine = null;

        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {

        spriteRenderer.material = whiteMaterial;


        yield return new WaitForSeconds(GameManager.instance.flashDuration);


        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }

    internal void ResetMaterial()
    {
        flashRoutine = null;
        spriteRenderer.material = originalMaterial;
        
    }
}