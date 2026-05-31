using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Menus : MonoBehaviour
{
    public static Menus instance;

    [Header("MAIN MENUS")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject helpMenu;
    public GameObject optionsMenu;
    public GameObject deathScreen;

    [Header("HELP MENU TABS")]
    public GameObject gameConceptsTab;
    public GameObject itemsTab;

    [Header("GAME CONCEPTS SUBTABS")]
    public GameObject attackingSubTab;
    public GameObject evolutionsSubTab;
    public GameObject expSubTab;
    public GameObject upgradesSubTab;
    public GameObject timerSubTab;
    public GameObject finalStandSubTab;
    public GameObject bossesSubTab;
    public GameObject transformationSubTab;

    [Header("ITEMS SUBTABS")]
    public GameObject weaponsSubTab;

    public GameObject blazebootsSubTab;
    public GameObject blunthammerSubTab;
    public GameObject boltshotSubTab;
    public GameObject eternalswordSubTab;
    public GameObject holywaterSubTab;
    public GameObject huntingrifleSubTab;
    public GameObject phantombladeSubTab;

    public GameObject passivesSubTab;

    public GameObject adrenalineSubTab;
    public GameObject beserkersringSubTab;
    public GameObject damageringSubTab;
    public GameObject enchantedquiverSubTab;
    public GameObject magnetismSubTab;
    public GameObject monstrousstaminaSubTab;
    public GameObject rechargerSubTab;
    public GameObject swiftwalkersSubTab;
    public GameObject vitalitypotionSubTab;


    [Header("TUTORIAL")]
    public GameObject tutorialParent;
    public GameObject[] tutorialPages;

    [Header("SCENES")]
    public string gameScene = "Game";
    public string mainMenuScene = "MainMenu";

    [Header("TRANSITIONS")]
    public Image fadeOverlay;
    public float fadeDuration = 1f;

    [Header("MAIN MENU INTRO")]
    public CanvasGroup logoGroup;
    public CanvasGroup mainButtonsGroup;

    [Header("Death Screen")]
    public TextMeshProUGUI finalKillsText;
    public TextMeshProUGUI highScoreText;
    public GameObject newHighScoreText;

    private int tutorialIndex;
    private bool isPaused;
    public static bool IsPaused;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
    
        HideAllMenus();
        HideAllTabs();
        HideAllSubTabs();
        HideTutorialPages();

        if (mainMenu != null)
            mainMenu.SetActive(true);
        
        if (mainMenu != null)
        {
            StartCoroutine(MainMenuIntro());
        }

        StartCoroutine(FadeScreen(1f, 0f));
    }

    void Update()
    {
        HandleEscapeKey();
    }

    // =========================================================
    // ESCAPE KEY
    // =========================================================

    void HandleEscapeKey()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        // Tutorial closes first
        if (tutorialParent.activeSelf)
        {
            EndTutorial();
            return;
        }

        // Help closes first
        if (helpMenu.activeSelf)
        {
            CloseHelpMenu();
            return;
        }

        // Options closes second
        if (optionsMenu.activeSelf)
        {
            CloseOptionsMenu();
            return;
        }

        // Pause toggle
        if (pauseMenu != null)
        {
            if (isPaused)
                ResumeGame();
            else if (mainMenu == null || !mainMenu.activeSelf)
                PauseGame();
        }
    }

    // =========================================================
    // MAIN MENU
    // =========================================================

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f));

        Time.timeScale = 1f;

        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =========================================================
    // PAUSE
    // =========================================================

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        helpMenu.SetActive(false);
        optionsMenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;
        IsPaused = false;
    }

    // =========================================================
    // HELP MENU
    // =========================================================

    public void OpenHelpMenu()
    {
        helpMenu.SetActive(true);

        OpenGameConceptsTab();
    }

    public void CloseHelpMenu()
    {
        helpMenu.SetActive(false);
    }

    // =========================================================
    // OPTIONS
    // =========================================================

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }

    // =========================================================
    // HELP TABS
    // =========================================================

    void HideAllTabs()
    {
        gameConceptsTab.SetActive(false);
        itemsTab.SetActive(false);
    }

    public void OpenGameConceptsTab()
    {
        HideAllTabs();
        HideAllSubTabs();

        gameConceptsTab.SetActive(true);

    }

    public void OpenItemsTab()
    {
        HideAllTabs();
        HideAllSubTabs();

        itemsTab.SetActive(true);
    }

    // =========================================================
    // SUBTABS
    // =========================================================

    void HideAllSubTabs()
    {
        attackingSubTab.SetActive(false);
        evolutionsSubTab.SetActive(false);
        expSubTab.SetActive(false);
        upgradesSubTab.SetActive(false);
        timerSubTab.SetActive(false);
        finalStandSubTab.SetActive(false);
        bossesSubTab.SetActive(false);
        transformationSubTab.SetActive(false);

        weaponsSubTab.SetActive(false);

        blazebootsSubTab.SetActive(false);
        blunthammerSubTab.SetActive(false);
        boltshotSubTab.SetActive(false);
        eternalswordSubTab.SetActive(false);
        holywaterSubTab.SetActive(false);
        huntingrifleSubTab.SetActive(false);
        phantombladeSubTab.SetActive(false);

        passivesSubTab.SetActive(false);

        adrenalineSubTab.SetActive(false);
        beserkersringSubTab.SetActive(false);
        damageringSubTab.SetActive(false);
        enchantedquiverSubTab.SetActive(false);
        magnetismSubTab.SetActive(false);
        monstrousstaminaSubTab.SetActive(false);
        rechargerSubTab.SetActive(false);
        swiftwalkersSubTab.SetActive(false);
        vitalitypotionSubTab.SetActive(false);
    }

    public void OpenAttackingSubTab()
    {
        HideAllSubTabs();
        attackingSubTab.SetActive(true);
    }

    public void OpenEvolutionsSubTab()
    {
        HideAllSubTabs();
        evolutionsSubTab.SetActive(true);
    }

    public void OpenExpSubTab()
    {
        HideAllSubTabs();
        expSubTab.SetActive(true);
    }

    public void OpenUpgradesSubTab()
    {
        HideAllSubTabs();
        upgradesSubTab.SetActive(true);
    }

    public void OpenTimerSubTab()
    {
        HideAllSubTabs();
        timerSubTab.SetActive(true);
    }

    public void OpenFinalStandSubTab()
    {
        HideAllSubTabs();
        finalStandSubTab.SetActive(true);
    }

    public void OpenBossesSubTab()
    {
        HideAllSubTabs();
        bossesSubTab.SetActive(true);
    }

    public void OpenTransformationSubTab()
    {
        HideAllSubTabs();
        transformationSubTab.SetActive(true);
    }

    public void OpenWeaponsSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
    }

    public void OpenBlazeBootsSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        blazebootsSubTab.SetActive(true);
    }

    public void OpenBluntHammerSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        blunthammerSubTab.SetActive(true);
    }

    public void OpenBoltShotSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        boltshotSubTab.SetActive(true);
    }

    public void OpenEternalSwordSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        eternalswordSubTab.SetActive(true);
    }

    public void OpenHolyWaterSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        holywaterSubTab.SetActive(true);
    }

    public void OpenHuntingRifleSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        huntingrifleSubTab.SetActive(true);
    }

    public void OpenPhantomBladeSubTab()
    {
        HideAllSubTabs();
        weaponsSubTab.SetActive(true);
        phantombladeSubTab.SetActive(true);
    }

    public void OpenPassivesSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
    }

    public void OpenAdrenalineSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        adrenalineSubTab.SetActive(true);
    }

    public void OpenBeserkersRingSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        beserkersringSubTab.SetActive(true);
    }

    public void OpenDamageRingSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        damageringSubTab.SetActive(true);
    }

    public void OpenEnchantedQuiverSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        enchantedquiverSubTab.SetActive(true);
    }

    public void OpenMagnetismSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        magnetismSubTab.SetActive(true);
    }

    public void OpenMonstrousStaminaSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        monstrousstaminaSubTab.SetActive(true);
    }

    public void OpenRechargerSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        rechargerSubTab.SetActive(true);
    }

    public void OpenSwiftWalkersSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        swiftwalkersSubTab.SetActive(true);
    }

    public void OpenVitalityPotionSubTab()
    {
        HideAllSubTabs();
        passivesSubTab.SetActive(true);
        vitalitypotionSubTab.SetActive(true);
    }

    // =========================================================
    // TUTORIAL
    // =========================================================

    public void StartTutorial()
    {
        tutorialIndex = 0;

        tutorialParent.SetActive(true);

        ShowTutorialPage(tutorialIndex);

    }

    public void NextTutorialPage()
    {
        tutorialIndex++;

        if (tutorialIndex >= tutorialPages.Length)
        {
            EndTutorial();
            return;
        }

        ShowTutorialPage(tutorialIndex);
    }

    public void PreviousTutorialPage()
    {
        tutorialIndex--;

        tutorialIndex = Mathf.Max(0, tutorialIndex);

        ShowTutorialPage(tutorialIndex);
    }

    void ShowTutorialPage(int index)
    {
        HideTutorialPages();

        tutorialPages[index].SetActive(true);
    }

    void HideTutorialPages()
    {
        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
    }

    public void EndTutorial()
    {
        HideTutorialPages();

        tutorialParent.SetActive(false);

    }

    IEnumerator MainMenuIntro()
    {
        logoGroup.alpha = 0f;
        mainButtonsGroup.alpha = 0f;

        yield return StartCoroutine(FadeCanvasGroup(logoGroup, 0f, 1f, 1.5f));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeCanvasGroup(mainButtonsGroup, 0f, 1f, 1f));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group,
    float start,
    float end,
    float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            group.alpha = Mathf.Lerp(start, end, timer / duration);

            yield return null;
        }

        group.alpha = end;
    }

    IEnumerator FadeScreen(float start, float end)
    {
        float timer = 0f;

        Color color = fadeOverlay.color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(start, end, timer / fadeDuration);

            color.a = alpha;

            fadeOverlay.color = color;

            yield return null;
        }

        color.a = end;
        fadeOverlay.color = color;
    }

    // =========================================================
    // DEATH SCREEN
    // =========================================================

    public void OpenDeathScreen(int killCount)
    {
        deathScreen.SetActive(true);

        Time.timeScale = 0f;

        finalKillsText.text =
            $"Kills This Run: {killCount}";

        int highScore =
            PlayerPrefs.GetInt("HighScore", 0);

        bool newRecord = false;

        if (killCount > highScore)
        {
            highScore = killCount;

            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );

            PlayerPrefs.Save();

            newRecord = true;
        }

        highScoreText.text =
            $"High Score: {highScore}";

        if (newHighScoreText != null)
            newHighScoreText.SetActive(newRecord);
    }

    public void RestartGame()
    {
        if (newHighScoreText != null)
            newHighScoreText.SetActive(false);
        
        StartCoroutine(RestartRoutine());
    }

    IEnumerator RestartRoutine()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f));

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        if (newHighScoreText != null)
            newHighScoreText.SetActive(false);

        StartCoroutine(ReturnToMenuRoutine());
    }

    IEnumerator ReturnToMenuRoutine()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f));

        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }

    // =========================================================
    // UTIL
    // =========================================================

    void HideAllMenus()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (helpMenu != null) helpMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (deathScreen != null) deathScreen.SetActive(false);
        if (tutorialParent != null) tutorialParent.SetActive(false);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}