
using UnityEngine;

public class SphereProjectileShape : MonoBehaviour, IProjectileShape
{
    public SphereCollider col;

    public Transform visual { get; set; }
    public Vector3 baseScale { get; set; }
    public float baseSize { get; set; }
    private bool initialized = false;

    public void SetSize(float size)
    {
        if (!initialized)
            Initialize();

        float sphereScale = Mathf.Sqrt(size);
        col.radius = baseSize * sphereScale;
        visual.localScale = baseScale * sphereScale;
    }
    public void ResetSize()
    {
        col.radius = baseSize;
        visual.localScale = baseScale;
    }

    public void Initialize()
    {
        if (initialized) return;

        col ??= GetComponent<SphereCollider>();
        visual ??= transform.GetChild(0);
        baseSize = col.radius;
        baseScale = visual.localScale;

        initialized = true;
    }
}