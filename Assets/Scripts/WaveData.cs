using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int amount;

    [Header("Stats")]
    public StatsPreset statsPreset;

    [Header("Spawn Timing")]
    public float spawnDuration = 60f; // spread over the minute
}

[System.Serializable]
public class WaveData
{
    public string waveName;
    public EnemySpawnData[] enemies;

    public bool isBossWave;
}