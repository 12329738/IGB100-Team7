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

        float scale = Mathf.Pow(size, 1f / 3f);
        col.size *= scale;
        float factor = Mathf.Sqrt(size);
        visual.localScale *= factor;
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


}