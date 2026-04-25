using UnityEngine;

public interface IProjectileShape
{
    void SetSize(float size);
    public Transform visual { get; set; }
    public Vector3 baseScale { get; set; }
    void Initialize();
    void ResetSize();

   
}