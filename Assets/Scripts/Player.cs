using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{

    
    public int level = 1;
    public float currentExperience = 0;
    public List<Weapon> startingWeapons;
    SphereCollider pickupCollider;
    [HideInInspector]
    public List<Weapon> weapons;
    [HideInInspector]
    public List<Passive> passives;
    [HideInInspector]
    public List<StatModifier> modifiers;
    public Dictionary<ItemList, Weapon> itemDictionary = new Dictionary<ItemList, Weapon>();
    public Transformation transformation;
    public StatusEffectData vampire;
    public float currentTransformationAmount;
    bool isTransformed = false;
    Coroutine transformationCoroutine;
    StatusEffectManager statusManager;

    Queue<int> levelUps;
    [HideInInspector]
    public bool upgradeChosen;
    private bool levelUpRoutineRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statusManager = GetComponent<StatusEffectManager>();
        statusManager.ApplyEffect(vampire, gameObject);
        foreach (Weapon weapon in startingWeapons) 
        {
            AddWeapon(weapon);
        }

        pickupCollider = GetComponent<SphereCollider>();
        levelUps = new Queue<int>();
}

    private void AddWeapon(Weapon weaponData)
    {
        if (itemDictionary.ContainsKey(weaponData.itemType))
            return;

        Weapon weapon = Instantiate(weaponData);
        weapons.Add(weapon);
        itemDictionary.Add(weaponData.itemType, weapon);
        weapon.Initialize();
        transformationCoroutine = StartCoroutine(TransformationCoroutine());
    }


    public Item TryGetItem(ItemList itemType)
    {
        itemDictionary.TryGetValue(itemType, out var item);
        return item;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        UpdateWeapons();
        UpdatePickupRange();
        //UpdateTransformationAmount();
    }

    private void UpdateTransformationAmount()
    {
        if (currentTransformationAmount < stats.GetStat(StatType.MaxTransformation).currentValue && transformationCoroutine == null)
        {
            transformationCoroutine = StartCoroutine(TransformationCoroutine());
        }
    }

    private IEnumerator TransformationCoroutine()
    {
        float regenAccumulator = 0f;

        while (true)
        {
            if (!isTransformed)
            {
                regenAccumulator += stats.GetStat(StatType.TransformationGainRate).currentValue * Time.deltaTime;
            }


            else if (isTransformed)
            {
                regenAccumulator -= stats.GetStat(StatType.TransformationDecayRate).currentValue * Time.deltaTime;
            }

            //int regenAmount = Mathf.FloorToInt(regenAccumulator);

            //if (regenAmount > 1)
            //{
                AddTransformationPoints(regenAccumulator);
                regenAccumulator -= regenAccumulator;
            //}
            yield return null;
        }
        

        
        
    }

    private void AddTransformationPoints(float regenAmount)
    {
        currentTransformationAmount += regenAmount;

        if (currentTransformationAmount > stats.GetStat(StatType.MaxTransformation).currentValue)
        {
            currentTransformationAmount = stats.GetStat(StatType.MaxTransformation).currentValue;
        }

        if (currentTransformationAmount < 0)
        {
            currentTransformationAmount = 0;
            StopTransformation();
        }
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
        pickupCollider.radius = stats.GetStat(StatType.Collection).currentValue;
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
            Transform();
        }

        transform.position += movement * stats.GetStat(StatType.MoveSpeed).currentValue * Time.deltaTime;
    }

    private IEnumerator LevelUp()
    {


        Time.timeScale = 0f;

        List<Upgrade> chosenUpgrades = GameManager.instance.database.GetAvaliableUpgrades();

        if (chosenUpgrades.Count > 0)
        {
            GameManager.instance.gameUI.ShowUpgradeOptions(chosenUpgrades);

            yield return new WaitUntil(() => upgradeChosen == true);
            upgradeChosen = false;
        }
        

        Time.timeScale = 1f;

    }

    public void AddUpgrade(Upgrade upgrade)
    {    
        Item item = TryGetItem(upgrade.itemType);

        if (item == null)
        {         
            AddItem(upgrade.itemType);          
        }


        else if (upgrade.modifiers != null) 
        {
            if (item is Weapon weapon)
            {
                AddWeaponModifiers(upgrade.modifiers, weapon);
                

            }

            else if (item is Passive passive)
            {
                foreach (StatModifier modifier in upgrade.modifiers)
                {
                    modifiers.Add(modifier);
                }

                stats.ApplyModifiers(modifiers);
            }

            item.currentLevel++;
        }     
    }

    private void AddWeaponModifiers(List<StatModifier> modifiers, Weapon weapon)
    {
        foreach (StatModifier modifier in modifiers)
        {
            weapon.AddModifier(modifier);
        }
    }


    private void AddItem(ItemList type)
    {
        Item item = GameManager.instance.database.GetItem(type);

        if (item is Weapon weapon)
        {
            AddWeapon(weapon);
        }

        else if (item is Passive passive)
        {
            passives.Add(passive);
        }
    }

    public void AddExperience(float amount)
    {
        currentExperience += amount;

        while (currentExperience >= GameManager.instance.GetExperienceAtLevel(level))
        {
            QueueLevelUp();

        }

        if (!levelUpRoutineRunning)
            StartCoroutine(ProcessLevelUps());
    }

    private void QueueLevelUp()
    {
        levelUps.Enqueue(1);
        currentExperience -= GameManager.instance.GetExperienceAtLevel(level);
        level++;
    }

    internal override void Die()
    {
        throw new NotImplementedException();
    }

    internal void Transform()
    {
        statusManager.ApplyEffect(transformation.effect, gameObject);
        isTransformed = true;
    }

    private void StopTransformation()
    {
        isTransformed = false;
    }

}
