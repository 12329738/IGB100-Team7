
using System;
using System.Collections;
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
    public int weaponLimit;
    public int upgradesPerLevel;
    public int weaponUpgradeLimit;

    public float commonChance;
    public float uncommonChance;
    public float epicChance;
    public float legendaryChange;

    public float commonModifier;
    public float uncommonModifier;
    public float epicModifier;
    public float legendaryModifier;
    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        database.Initialize();
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {

    }


    private void Start()
    {

    }

    private void Update()
    {

    }

    internal void SetSceneReferences(SceneReference sceneReference)
    {
        player = sceneReference.player;
        camera = sceneReference.camera;
        gameUI = sceneReference.UI;
    }
}
