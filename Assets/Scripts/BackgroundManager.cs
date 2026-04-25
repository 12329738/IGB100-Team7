using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform[] tiles;
    private float tileSize;
    Vector3[] originalPositions;

    public Transform player => GameManager.instance.player.transform;

    void Awake()
    {
        Renderer r = tiles[0].GetComponentInChildren<MeshRenderer>();
        tileSize = r.bounds.size.x;

        originalPositions = new Vector3[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            originalPositions[i] = tiles[i].position;
    }

    void Update()
    {
        Vector2 playerGrid = new Vector2(
            Mathf.Floor(player.position.x / tileSize),
            Mathf.Floor(player.position.z / tileSize) 
        );

        for (int i = 0; i < tiles.Length; i++)
        {
            Vector3 basePos = originalPositions[i];

            Vector3 offset = new Vector3(
                playerGrid.x * tileSize,
                0,
                playerGrid.y * tileSize 
            );

            tiles[i].position = basePos + offset;
        }
    }
}