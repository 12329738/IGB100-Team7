using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashWhite : MonoBehaviour
{
    public float flashDuration = 0.05f; // duration of the white flash

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Material whiteMaterial;
    private Coroutine flashRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        whiteMaterial = new Material(Shader.Find("GUI/Text Shader"));
        whiteMaterial.color = Color.white; // full white
    }

    public void TriggerFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {

        spriteRenderer.material = whiteMaterial;


        yield return new WaitForSeconds(flashDuration);


        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}