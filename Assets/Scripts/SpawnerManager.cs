
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    public float spawnRate;
    public float spawnModifier;
    public GameObject[] enemies;
    public GameObject experienceGem;
    public float padding;
    float timer;
    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;

            foreach (GameObject enemy in enemies)
            {
                SpawnEnemy(enemy);
            }
            
        }
        
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        Ray rayBL = cam.ViewportPointToRay(new Vector3(0, 0));
        Ray rayTR = cam.ViewportPointToRay(new Vector3(1, 1));

        ground.Raycast(rayBL, out float enterBL);
        ground.Raycast(rayTR, out float enterTR);

        Vector3 bottomLeft = rayBL.GetPoint(enterBL);
        Vector3 topRight = rayTR.GetPoint(enterTR);

        Vector3 spawnLocation = GetSpawnLocation(bottomLeft, topRight);

        

        GameObject enemy = ObjectPool.instance.GetObject(enemyPrefab);
        enemy.transform.position = spawnLocation;
        enemy.transform.rotation = Quaternion.identity;
    }

    private Vector3 GetSpawnLocation(Vector3 bottomLeft, Vector3 topRight)
    {
        float minX = bottomLeft.x;
        float maxX = topRight.x;

        float minZ = bottomLeft.z;
        float maxZ = topRight.z;

        float x = Random.value < 0.5f
            ? minX - padding
            : maxX + padding;

        float z = Random.Range(minZ - padding, maxZ + padding);

        if (Random.value < 0.5f)
        {
            x = Random.Range(minX - padding, maxX + padding);
            z = Random.value < 0.5f ? minZ - padding : maxZ + padding;
        }

        return new Vector3(x, 0, z);
    }

    public void SpawnExperienceGem(Vector3 location, float amount)
    {
        GameObject obj = ObjectPool.instance.GetObject(experienceGem);
        ExperienceGem exp = obj.GetComponent<ExperienceGem>();
        exp.transform.position = location;
        exp.transform.rotation = Quaternion.identity;   
        exp.experienceValue = amount;
        
    }

}
