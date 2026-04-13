using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{

    
    public float level = 1;
    Vector3 position;
    public float currentExperience = 0;
    public float experienceForNextLevel = 100;
    //[SerializeField] private Team _team;
    //public override Team team => _team;

    //[SerializeField] private Stats _stats;
    //public override Stats stats => _stats;

    //[SerializeField] private float _hitCooldown;
    //public override float hitCooldown => _hitCooldown;
    public List<WeaponData> weaponData;
    SphereCollider pickupCollider;
    public List<Weapon> weapons = new List<Weapon>();
    public Dictionary<ItemList, Weapon> weaponDictionary = new Dictionary<ItemList, Weapon>();
    List<List<Upgrade>> currentUpgrades = new List<List<Upgrade>>();
    Queue<int> levelUps;
    public bool upgradeChosen;
    private bool levelUpRoutineRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        foreach (var weaponData in weaponData) 
        {
            AddUpgrade(weaponData.upgradeList[0][0]);
        }

        pickupCollider = GetComponent<SphereCollider>();
        levelUps = new Queue<int>();
    }

    private void AddWeapon(WeaponData weaponData)
    {
        Weapon weapon = new Weapon();
        weapon.baseData = weaponData;
        weapons.Add(weapon);
        weaponDictionary.Add(weaponData.itemType, weapon);
        weapon.Initialize();
    }

    public Weapon TryGetWeapon(ItemList itemType)
    {
        weaponDictionary.TryGetValue(itemType, out var weapon);
        return weapon;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        UpdateWeapons();
        UpdatePickupRange();
    }

    private IEnumerator ProcessLevelUps()
    {
        levelUpRoutineRunning = true;
        while (levelUps.Count > 0)
        {
            levelUps.Dequeue();

            yield return StartCoroutine(LevelUp());
        }

        levelUpRoutineRunning = false;
    }

    private void UpdatePickupRange()
    {
        pickupCollider.radius = stats.GetStat(StatType.PickupRadius).currentValue;
    }


    private void UpdateWeapons()
    {
        float dt = Time.deltaTime;

        foreach (var weapon in weapons)
        {
            weapon.Tick(dt);
        }
    }

    private void CheckMovement()
    {
        Vector3 movement = new Vector3(
            Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0,
            0,
            Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0
        ).normalized;

        if (Input.GetKeyDown("t"))
        {
            LevelUp();
        }

        transform.position += movement * stats.GetStat(StatType.MoveSpeed).currentValue * Time.deltaTime;
    }

    private IEnumerator LevelUp()
    {
        level++;

        Time.timeScale = 0f;

        List<Upgrade> chosenUpgrades = GameManager.instance.database.GetAvaliableUpgrades();

        GameManager.instance.gameUI.ShowUpgradeOptions(chosenUpgrades);

        yield return new WaitUntil(() => upgradeChosen == true);
        upgradeChosen = false;

        Time.timeScale = 1f;

    }

    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is ItemUpgrade itemUpgrade)
        {
            Weapon weapon = TryGetWeapon(itemUpgrade.itemType);


            foreach (StatModifier modifier in itemUpgrade.modifiers)
            {
                weapon.AddModifier(modifier);
            }

            weapon.currentLevel++;

        }

        if (upgrade is Item item)
        {
            AddWeapon(GameManager.instance.database.GetItem(item.itemType));
        }
        

        foreach (KeyValuePair<StatType, Stat> stat in stats.statDictionary)
        {
            Debug.Log($"{stat.Key.ToString()} {stat.Value.currentValue}");
        }

    }

    public void AddExperience(float amount)
    {
        currentExperience += amount;

        while (currentExperience >= experienceForNextLevel)
        {
            experienceForNextLevel += 100;
            levelUps.Enqueue(1);
        }

        if (!levelUpRoutineRunning)
            StartCoroutine(ProcessLevelUps());
    }

    internal override void Die()
    {
        throw new NotImplementedException();
    }
}
