using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject controlsTab;
    public GameObject gameConceptsTab;
    public GameObject itemsTab;

    [Header("GAME CONCEPT SUBTABS")]
    public GameObject attackingSubTab;
    public GameObject evolutionsSubTab;
    public GameObject expSubTab;
    public GameObject upgradesSubTab;
    public GameObject timerSubTab;
    public GameObject finalStandSubTab;
    public GameObject bossesSubTab;
    public GameObject transformationSubTab;

    [Header("TUTORIAL")]
    public GameObject tutorialParent;
    public GameObject[] tutorialPages;

    [Header("SCENES")]
    public string gameScene = "Game";
    public string mainMenuScene = "MainMenu";

    private int tutorialIndex;
    private bool isPaused;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        HideAllMenus();
        HideAllTabs();
        HideAllSubTabs();
        HideTutorialPages();

        if (mainMenu != null)
            mainMenu.SetActive(true);
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
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        helpMenu.SetActive(false);
        optionsMenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;
    }

    // =========================================================
    // HELP MENU
    // =========================================================

    public void OpenHelpMenu()
    {
        helpMenu.SetActive(true);

        OpenControlsTab();
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
        controlsTab.SetActive(false);
        gameConceptsTab.SetActive(false);
        itemsTab.SetActive(false);
    }

    public void OpenControlsTab()
    {
        HideAllTabs();

        controlsTab.SetActive(true);
    }

    public void OpenGameConceptsTab()
    {
        HideAllTabs();

        gameConceptsTab.SetActive(true);

        OpenAttackingSubTab();
    }

    public void OpenItemsTab()
    {
        HideAllTabs();

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

    // =========================================================
    // TUTORIAL
    // =========================================================

    public void StartTutorial()
    {
        tutorialIndex = 0;

        tutorialParent.SetActive(true);

        ShowTutorialPage(tutorialIndex);

        Time.timeScale = 0f;
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

        Time.timeScale = 1f;
    }

    // =========================================================
    // DEATH SCREEN
    // =========================================================

    public void OpenDeathScreen()
    {
        deathScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
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
}