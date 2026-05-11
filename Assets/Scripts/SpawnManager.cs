using UnityEngine;
using TMPro;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("Timer")]
    [Tooltip("Set this to whatever length you want (in seconds)")]
    public float startingTime = 300f; // Change this to determine starting timer amount (300f = 5 min)
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI waveText;

    [Header("Waves (1 per minute)")]
    public WaveData[] waves;
    private int currentMinute = 0;

    [Header("Player")]
    public Transform player;
    public float minSpawnDistance = 10f;

    [Header("Last Stand")]
    public float spawnInterval = 2f;
    public float difficultyRamp = 0.2f;
    private float difficultyMultiplier = 1f;
    private float spawnTimer;
    private bool lastStandActive = false;

    [Header("Enemy Tracking")]
    public int currentNormalEnemies;
    public int currentBossEnemies;
    public int minimumEnemyNumber = 5;

    [Header("Spawning")]
    public float padding = 2f;
    public GameObject experienceGem;
    public GameObject chest;

    [Header("Minimum Spawn Control")]
    public float minSpawnInterval = 1f;
    private float minSpawnTimer;

    [Header("Last Stand UI")]
    public TextMeshProUGUI lastStandNotification;
    public float notificationDuration = 3f;

    [Header("Final Stand")]
    public EnemySpawnData[] finalStandEnemies;

    private Camera cam;
    private WaveData activeWave;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        cam = Camera.main;
        currentTime = startingTime;
        UpdateTimerUI();

        currentMinute = 0;

        // Start Wave 1 immediately
        if (waves.Length > 0)
        {
            StartCoroutine(AnnounceAndSpawn(waves[0]));
            currentMinute = 1;
        }
    }

    void Update()
    {
        if (!lastStandActive)
            RunTimer();
        else
            RunLastStand();
    }

    // Timer
    void RunTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            StartLastStand();
            return;
        }
        UpdateTimerUI();
        
        int minutePassed = Mathf.FloorToInt((startingTime - currentTime) / 60f);

        // Trigger next wave ONLY when minute increases
        if (minutePassed >= currentMinute && currentMinute < waves.Length)
        {
            StartCoroutine(AnnounceAndSpawn(waves[currentMinute]));
            currentMinute++;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        if (lastStandActive)
        {
            timerText.text = "FINAL STAND";
            return;
        }

        int min = Mathf.FloorToInt(currentTime / 60f);
        int sec = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{min:00}:{sec:00}";
    }

    IEnumerator AnnounceAndSpawn(WaveData wave)
    {
        if (waveText != null)
        {
            waveText.text = wave.waveName;
            waveText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (waveText != null)
            waveText.gameObject.SetActive(false);

        SpawnWave(wave);
    }

    // Waves
    void SpawnWave(WaveData wave)
    {
        StopAllCoroutines(); // prevents stacking systems

        activeWave = wave;

        foreach (var enemy in wave.enemies)
        {
            enemy.currentAlive = 0;

            StartCoroutine(ManageEnemyType(enemy));
        }

        if (wave.isBossWave)
        {
            Debug.Log("Boss Wave!");
        }
    }

    bool CanSpawnEnemy(bool isBoss)
    {
        // Bosses ignore cap
        if (isBoss)
            return true;

        // No active wave yet
        if (activeWave == null)
            return true;

        return currentNormalEnemies < activeWave.maxEnemiesOnScreen;
    }

    IEnumerator ManageEnemyType(EnemySpawnData enemy)
    {
        // Initial spawn over time
        if (enemy.amount > 0)
        {
            float interval = enemy.spawnDuration / enemy.amount;

            for (int i = 0; i < enemy.amount; i++)
            {
                if (CanSpawnEnemy(enemy.isBoss))
                {
                    SpawnEnemy(enemy.enemyPrefab, enemy.statsPreset, enemy);
                }

                yield return new WaitForSeconds(interval);
            }
        }

        // Maintain minimum count forever
        while (true)
        {
            if (enemy.currentAlive < enemy.minAlive)
            {
                if (CanSpawnEnemy(enemy.isBoss))
                {
                    SpawnEnemy(enemy.enemyPrefab, enemy.statsPreset, enemy);
                }
            }

            yield return new WaitForSeconds(enemy.spawnDelay);
        }
    }

    IEnumerator SpawnOverTime(EnemySpawnData enemy)
    {
        if (enemy.amount <= 0) yield break;

        float interval = enemy.spawnDuration / enemy.amount;

        for (int i = 0; i < enemy.amount; i++)
        {
            SpawnEnemy(enemy.enemyPrefab, enemy.statsPreset, enemy);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator ShowLastStandNotification()
    {
        if (lastStandNotification == null)
            yield break;

        lastStandNotification.gameObject.SetActive(true);

        Color color = lastStandNotification.color;

        // Fully visible first
        color.a = 1f;
        lastStandNotification.color = color;

        yield return new WaitForSeconds(1f);

        float timer = 0f;

        while (timer < notificationDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / notificationDuration);

            color.a = alpha;
            lastStandNotification.color = color;

            yield return null;
        }

        lastStandNotification.gameObject.SetActive(false);
    }

    // Last Stand
    void StartLastStand()
    {
        if (lastStandActive) return;

        lastStandActive = true;
        currentTime = 0;

        UpdateTimerUI();

        StartCoroutine(ShowLastStandNotification());
    }

    void RunLastStand()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            spawnInterval = Mathf.Max(0.3f, spawnInterval - 0.01f); // Gradually increases spawn rate during Last Stand

            difficultyMultiplier += difficultyRamp;

            int spawnCount = Mathf.CeilToInt(difficultyMultiplier);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnLastStandEnemy();
            }
        }
    }

    void SpawnLastStandEnemy()
    {
        if (finalStandEnemies.Length == 0)
            return;

        EnemySpawnData enemyData =
            finalStandEnemies[Random.Range(0, finalStandEnemies.Length)];

        // Prevents bosses in Final Stand (instead spawns a regular enemy)
        while (enemyData.isBoss)
        {
            enemyData = finalStandEnemies[
                Random.Range(0, finalStandEnemies.Length)
            ];
        }

        SpawnEnemy(enemyData.enemyPrefab, enemyData.statsPreset, enemyData);
    }

    // Spawning
    void SpawnEnemy(GameObject prefab, StatsPreset preset, EnemySpawnData data)
    {
        GameObject obj = ObjectPool.instance.GetObject(prefab);
        if (obj == null) return;

        Entity entity = obj.GetComponent<Entity>();

        if (entity != null)
        {
            entity.statPreset = preset;
        }

        obj.transform.position = GetSpawnLocation();
        obj.transform.rotation = Quaternion.identity;

        obj.SetActive(true);

        // Track this type
        data.currentAlive++;

        // Register callback on death
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.isBoss = data.isBoss;
            enemy.OnDeathCallback = () => data.currentAlive--;
        }
    }

    Vector3 GetSpawnLocation()
    {
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        for (int i = 0; i < 10; i++)
        {
            Ray rayBL = cam.ViewportPointToRay(new Vector3(0, 0));
            Ray rayTR = cam.ViewportPointToRay(new Vector3(1, 1));

            ground.Raycast(rayBL, out float enterBL);
            ground.Raycast(rayTR, out float enterTR);

            Vector3 bottomLeft = rayBL.GetPoint(enterBL);
            Vector3 topRight = rayTR.GetPoint(enterTR);

            float x = Random.Range(bottomLeft.x - padding, topRight.x + padding);
            float z = Random.Range(bottomLeft.z - padding, topRight.z + padding);

            Vector3 pos = new Vector3(x, 0, z);

            if (Vector3.Distance(pos, player.position) >= minSpawnDistance)
                return pos;
        }

        return player.position + Random.insideUnitSphere * minSpawnDistance;
    }

    // Enemy Tracking
    public void RegisterEnemy(bool isBoss)
    {
        if (isBoss)
            currentBossEnemies++;
        else
            currentNormalEnemies++;
    }

    public void UnregisterEnemy(bool isBoss)
    {
        if (isBoss)
        {
            if (currentBossEnemies > 0)
                currentBossEnemies--;
        }
        else
        {
            if (currentNormalEnemies > 0)
                currentNormalEnemies--;
        }
    }

    // Exp
    public void SpawnExperienceGem(Vector3 location, float amount)
    {
        GameObject obj = ObjectPool.instance.GetObject(experienceGem);

        var exp = obj.GetComponent<ExperienceGem>();
        exp.transform.position = location;
        exp.transform.rotation = Quaternion.identity;
        exp.experienceValue = amount;

        obj.SetActive(true);
    }

    public void SpawnChest(Vector3 position)
    {
        GameObject obj = ObjectPool.instance.GetObject(chest);
    }
}