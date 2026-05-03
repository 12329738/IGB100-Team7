using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class Player : Entity, IDamageable
{

    
    public int level = 1;
    [HideInInspector]
    public float currentExperience = 0;
    public List<Weapon> startingWeapons;
    public SphereCollider pickupCollider;
    [HideInInspector]
    public List<Weapon> weapons;
    [HideInInspector]
    public List<Passive> passives;
    [HideInInspector]
    public Dictionary<ItemList, Item> itemDictionary = new();
    public Transformation transformation;
    [HideInInspector]
    public float currentTransformationAmount;
    bool isTransformed = false;
    Coroutine transformationCoroutine;

    public Sprite regularSprite;
    public AnimatorController animatorController;
    public AnimatorOverrideController transformOverride;
    public SpriteRenderer sr;
    bool isFlipped;
    Animator animator;
    private StatusEffectDataInstance transformationStatusEffect;


    Queue<int> levelUps = new();
    [HideInInspector]
    public bool upgradeChosen;
    private bool levelUpRoutineRunning;

    [HideInInspector]
    public float timeTransformed;
    float transformationStartTime;
    List<Upgrade> avaliableTransformationUpgrades = new();

    public List<TransformationUpgrade> currentTransformationUpgrades = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //foreach (EffectEntryNode node in transformation.effect.entries)
        //{
        //    currentTransformationEffects.Add(node);
        //}
        //foreach (TransformationUpgrade upgrade in currentTransformationUpgrades)
        //{
        //    foreach (EffectEntryNode node in upgrade.effects)
        //    {
        //        currentTransformationEffects.Add(node);
        //    }
        //}

        transformationStatusEffect = new StatusEffectDataInstance(transformation.effect);

        foreach (Weapon weapon in startingWeapons) 
        {
            AddWeapon(weapon);
        }

        avaliableTransformationUpgrades.AddRange(transformation.upgrades);


        pickupCollider.radius = stats.GetStat(StatType.Collection);
        transformationCoroutine = StartCoroutine(TransformationCoroutine());
    }

    private void AddWeapon(Weapon weaponData)
    {
        if (itemDictionary.ContainsKey(weaponData.itemType))
            return;

        Weapon weapon = Instantiate(weaponData); 
        weapon.Initialize(this);

        weapons.Add(weapon);
        itemDictionary.Add(weapon.itemType, weapon);

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
        UpdateTransformationAmount();
    }

    private void UpdateTransformationAmount()
    {
        if (currentTransformationAmount < stats.GetStat(StatType.MaxTransformation) && transformationCoroutine == null)
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
                regenAccumulator += stats.GetStat(StatType.TransformationGainRate) * Time.deltaTime;
                timeTransformed = Time.time - transformationStartTime;
            }


            else if (isTransformed)
            {
                regenAccumulator -= stats.GetStat(StatType.TransformationDecayRate) * Time.deltaTime;
            }

            AddTransformationPoints(regenAccumulator);
            regenAccumulator -= regenAccumulator;
            
            yield return null;
        }
        

        
        
    }

    private void AddTransformationPoints(float regenAmount)
    {
        currentTransformationAmount += regenAmount;

        if (currentTransformationAmount > stats.GetStat(StatType.MaxTransformation))
        {
            currentTransformationAmount = stats.GetStat(StatType.MaxTransformation);
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
        if (Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("w") || Input.GetKey("a"))
        {
            animator.SetBool("IsMoving", true);
        }

        else
            animator.SetBool("IsMoving", false);

        Vector3 movement = new Vector3(Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0,  0,Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0
        ).normalized;


        if (Input.GetKeyDown("t"))
        {
            if (!isTransformed)
            Transform();
            else
            {
                StopTransformation();
            }
        }
        if (Input.GetKeyDown("d") && !isFlipped)
        {
            sr.flipX = true;
            isFlipped = true;
        }
            
        if (Input.GetKeyDown("a") && isFlipped)
        {
            sr.flipX = false;
            isFlipped = false;
        }
            
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

            transform.position += movement * stats.GetStat(StatType.MoveSpeed) * Time.deltaTime;
    }

    private IEnumerator LevelUp()
    {

        level++;
        Time.timeScale = 0f;

        List<Upgrade> chosenUpgrades = GameManager.instance.database.GetAvaliableUpgrades();

        if (chosenUpgrades.Count > 0)
        {
            GameManager.instance.gameUI.ShowUpgradeOptions(chosenUpgrades);

            yield return new WaitUntil(() => upgradeChosen == true);
            upgradeChosen = false;
        }

        if (level % GameManager.instance.transformationUpgradeInterval == 0 && avaliableTransformationUpgrades.Count > 0)
        {
            GameManager.instance.gameUI.ShowUpgradeOptions(avaliableTransformationUpgrades);
            yield return new WaitUntil(() => upgradeChosen == true);
            upgradeChosen = false;

        }

        Time.timeScale = 1f;

    }

    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is ItemUpgrade itemUpgrade)
        {
            Item item = TryGetItem(itemUpgrade.itemType);

            if (item == null)
            {
                AddItem(itemUpgrade.itemType);
                item = TryGetItem(itemUpgrade.itemType);
            }

            bool hasModifiers = upgrade.modifiers.Count > 0;
            bool hasEffects = upgrade.effects.Count > 0;

            if (hasModifiers)
            {
                foreach (StatModifier modifier in upgrade.modifiers)
                {
                    item.AddModifier(modifier);
                }
            }

            if (hasEffects)
            {
                foreach (var effect in upgrade.effects)
                {
                    EffectInstance instance = new EffectInstance(effect, this, this, this);
                    GameManager.instance.effectHandler.Register(instance);
                }
            }

            if (hasModifiers || hasEffects)
            {
                item.currentLevel++;
            }
        }

        else if (upgrade is TransformationUpgrade transformationUpgrade)
        {
            transformationStatusEffect.entries.AddRange(transformationUpgrade.effects);
            avaliableTransformationUpgrades.Remove(transformationUpgrade);

        }
        
    }


    private void AddItem(ItemList type)
    {
        Item item = GameManager.instance.database.GetItem(type);

        if (item is Weapon w)
        {
            AddWeapon(w);
        }

        else if (item is Passive passive)
        {

            AddPassive(passive);

        }



    }

    private void AddPassive(Passive passive)
    {
        Passive passiveInstance = Instantiate(passive);

        passives.Add(passiveInstance);
        itemDictionary.Add(passiveInstance.itemType, passiveInstance);

        provider.AddChild(passiveInstance.provider);
        
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
        
    }

    internal override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    internal void Transform()
    {
        isTransformed = true;
        timeTransformed = 0;
        transformationStartTime = Time.time;

        status.Apply(transformationStatusEffect, this.GetComponent<IDamageSource>());
        sr.sprite = transformation.transformationSprite;
        animator.runtimeAnimatorController = transformOverride;


    }

    private void StopTransformation()
    {
        status.RemoveStatus(transformation.effect);
        sr.sprite = regularSprite;
        isTransformed = false;
        timeTransformed = 0;
        animator.runtimeAnimatorController = animatorController;
    }


    
}
