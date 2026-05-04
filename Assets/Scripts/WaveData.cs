using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public StatsPreset statsPreset;

    [Header("Wave Spawn")]
    public int amount;
    public float spawnDuration = 60f;

    [Header("Runtime Control")]
    public int minAlive = 3;          // Minimum number to maintain
    public float spawnDelay = 2f;     // Time between spawns

    // Runtime tracking (not shown in inspector)
    [HideInInspector] public int currentAlive = 0;
    [HideInInspector] public float spawnTimer = 0f;
}

[System.Serializable]
public class WaveData
{
    public string waveName;
    public EnemySpawnData[] enemies;

    public bool isBossWave;
}