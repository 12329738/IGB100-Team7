using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Behaviours/Straight Movement")]
public class StraightMovement : WeaponBehaviour
{
    internal override void Move(GameObject obj, Vector3 direction, float moveSpeed)
    {
        obj.transform.position += direction * moveSpeed * Time.deltaTime;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
