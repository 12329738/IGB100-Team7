using System;
using TMPro;
using UnityEngine;

public class DamagePopupText : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    private float lifetime;
    private float fadeTime;
    private float floatSpeed;
    private Action onFinished;
    private Color startColor;
    public bool finished;

    public void Setup(Vector3 cameraPosition, Vector3 position,
        Color color,
        float size,
        float fade,
        float speed)
    {
        finished = false;
        text.color = color;
        text.fontSize = size;

        startColor = color;

        fadeTime = fade;
        floatSpeed = speed;
        lifetime = 0f;

        canvasGroup.alpha = 1f;
        transform.position = position;
        transform.forward = cameraPosition;

        float posx = transform.position.x;
        float posy = 0;
        float posz = transform.position.z;
        Vector3 newPosition = new Vector3(posx, posy, posz);
        transform.position = newPosition;
    }

    public void Tick()
    {
        lifetime += Time.deltaTime;

        float t = lifetime / fadeTime;


        canvasGroup.alpha = 1f - t;

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (lifetime >= fadeTime)
        {
            finished = true;
            ObjectPool.instance.ReturnObject(gameObject);
        }
    }
}