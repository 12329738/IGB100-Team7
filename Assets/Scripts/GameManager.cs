
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public EffectExecutor effectExecutor;
    public EffectHandler effectHandler;
    public Player player;
    public Camera camera;
    public GameUI gameUI;
    public ItemDatabase database;
    public ProjectileSpawner projectileSpawner;
    public StatusEffectRegistry statusEffectRegistry;
    public int weaponLimit;
    public int passiveLimit;
    public int upgradesPerLevel;
    public int weaponUpgradeLimit;
    public int passiveUpgradeLimit;
    public float minExp = 0;
    public float maxExp = 1000000;
    public float maxLevel = 100;
    public float knockBackSpeed;
    public int transformationUpgradeInterval = 5;

    [HideInInspector]
    public Rarity[] rarities;
    public Dictionary<RarityEnum, Color> rarityColors;


    public AnimationCurve experienceCurve;
    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        ResetRuntimeState();

        
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ResetRuntimeState()
    {
        database = new ItemDatabase();
        database.Initialize();
        rarities = Resources.LoadAll<Rarity>("Rarities");
        projectileSpawner = new ProjectileSpawner();
        effectExecutor = new EffectExecutor();
        effectHandler = GetComponent<EffectHandler>();
        statusEffectRegistry = new StatusEffectRegistry();

        CreateRarityDictionary();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneReference sceneRef = FindFirstObjectByType<SceneReference>();

        if (sceneRef != null)
        {
            SetSceneReferences(sceneRef);
        }

        ResetRuntimeState();
    }
    private void CreateRarityDictionary()
    {
        rarityColors = new Dictionary<RarityEnum, Color>();
        foreach (Rarity rarity in rarities)
        {
            rarityColors[rarity.rarity] = rarity.color;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if (instance != null) return;

        GameObject prefab = Resources.Load<GameObject>("GameManager");
        Instantiate(prefab);
    }



    internal void SetSceneReferences(SceneReference sceneReference)
    {
        player = sceneReference.player;
        camera = sceneReference.camera;
        gameUI = sceneReference.UI;
    }

    public float GetExperienceAtLevel(int level)
    {
        float t = Mathf.Clamp01((float)level / maxLevel);
        return Mathf.Lerp(minExp, maxExp, experienceCurve.Evaluate(t));
    }

    public float GetExperienceBetweenRange(float min, float max)
    {
        return experienceCurve.Evaluate(max) - experienceCurve.Evaluate(min);
    }
}
