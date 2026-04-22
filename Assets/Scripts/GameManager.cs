
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;


public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public Player player;
    public Camera camera;
    public GameUI gameUI;
    public ItemDatabase database;
    public ProjectileSpawner projectileSpawner;
    public int weaponLimit;
    public int passiveLimit;
    public int upgradesPerLevel;
    public int weaponUpgradeLimit;
    public int passiveUpgradeLimit;
    public float minExp = 0;
    public float maxExp = 1000000;
    public float maxLevel = 100;
    public float knockBackSpeed;


    public List<Rarity> rarities;
    public Dictionary<RarityEnum, Color> rarityColors;


    public AnimationCurve experienceCurve;
    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        database = new ItemDatabase();
        database.Initialize();    
        projectileSpawner = new ProjectileSpawner();

        CreateRarityDictionary();
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
        if (instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("GameManager");
            Instantiate(prefab);
        }
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
