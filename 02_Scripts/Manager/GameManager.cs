using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public SceneLoader sceneLoader { get; private set; }
    public InputManager inputManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public LanguageManager languageManager { get; private set; }
    public SoundManager soundManager { get; private set; }
    public MainQuestSystem mainQuestSystem { get; private set; }
    public SubQuestSystem subQuestSystem { get; private set; }
    public QuestConditionChecker questConditionChecker { get; private set; }
    public CompanionManager companionManager { get; private set; }
    public AddressableManager addressableManager { get; private set; }

    public PlayManager playManager;

    private void Start()
    {
        Init();
        StartScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        SceneManager.sceneLoaded += StartScene;
        SceneManager.sceneUnloaded += UnLoad;
    }

    public void Init()
    {
        SetComponents();
        InitComponents();
    }

    private void SetComponents()
    {
        sceneLoader ??= SceneLoader.Instance;
        inputManager ??= InputManager.Instance;
        dataManager ??= DataManager.Instance;
        languageManager ??= LanguageManager.Instance;
        soundManager ??= SoundManager.Instance;

        mainQuestSystem ??= MainQuestSystem.Instance;
        subQuestSystem ??= SubQuestSystem.Instance;
        questConditionChecker ??= GetComponentInChildren<QuestConditionChecker>();

        addressableManager ??= AddressableManager.Instance;
    }

    private void InitComponents()
    {
        inputManager?.Init();
        dataManager.Init();
        languageManager.Init();
        //soundManager.Init();
        //mainQuestSystem.Init();
        //subQuestSystem.Init();
        //questConditionChecker
        //addressableManager.Init();
    }

    public void StartScene(Scene scene, LoadSceneMode mode)
    {
        Logger.Log("씬 로드");
        playManager = FindObjectOfType<PlayManager>();

        if (playManager == null)
        {
            Logger.Log("플레이씬이 아닙니다.");
        }
        else
        {
            playManager?.Init();
        }
    }

    public void UnLoad(Scene scene)
    {
        Logger.Log("언 로드");
        playManager.UnLoad();
        playManager = null;
        addressableManager.ReleaseAll();
    }
}
