using UnityEngine;

public class BoxProjectileShape : MonoBehaviour, IProjectileShape
{
    public BoxCollider col;

    public Transform visual { get; set; }
    public Vector3 baseScale { get; set; }
    public Vector3 baseSize { get; set; }

    private bool initialized = false;
    public void SetSize(float size)
    {
        if (!initialized)
            Initialize();

        float scale = Mathf.Sqrt(size);
        col.size = baseSize * scale;
        visual.localScale = baseScale * scale;
    }

    public void Initialize()
    {
        if (initialized) return;

        col ??= GetComponent<BoxCollider>();
        visual ??= transform.GetChild(0);
        baseSize = col.size;
        baseScale = visual.localScale;

        initialized = true;
    }

    public void ResetSize()
    {
        col.size = baseSize;
        visual.localScale = baseScale;
    }

    public void SetCollider(bool enabled)
    {
        col.enabled = enabled;
    }
}