using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public Scene defalutScene;
    public Scene battleScene;

    public string DefaultScene;
    public string BattleScene;

    public List<GameObject> defalutObject;
    public List<GameObject> battleObject;

    private InputManager inputManager;

    private int spawnEnemyCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnterBattle(null);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExitBattle();
        }
    }

    public void Init()
    {
        inputManager = InputManager.Instance;

        defalutScene = SceneManager.GetSceneByName(DefaultScene);
        battleScene = SceneManager.GetSceneByName(BattleScene);

        battleObject.Add(UIManager.Instance.showStatus.gameObject);


        DefaultSceneToggle(true);
        BattleSceneToggle(false);
    }

    public void EnterBattle(ExternalEnemy externalEnemy)
    {
        Logger.Log("EnterBattle");
        StartCoroutine(EnterBattleRoutine(externalEnemy));
    }

    IEnumerator EnterBattleRoutine(ExternalEnemy externalEnemy)
    {
        Time.timeScale = 0;
        inputManager.inputActions.Disable();
        yield return new WaitForSecondsRealtime(2f);

        DefaultSceneToggle(false);

        spawnEnemyCount = EnemyManager.Instance.SetBattleScene(externalEnemy);
        //Logger.Log(spawnEnemyCount);
        if(spawnEnemyCount == 0)
        {
            Logger.LogError("enemy spawn count : 0, check external enemy");
        }
        CompanionManager.Instance.SetBattleScene();
        BattleSceneToggle(true);
        yield return null;

        Time.timeScale = 1;
        inputManager.inputActions.Enable();
    }

    public void PlayerDeath()
    {
        EnemyManager.Instance.OnEndBattle?.Invoke();
    }

    public void EnemyDeath()
    {
        spawnEnemyCount--;
        CheckBattleState();
    }

    public void CheckBattleState()
    {
        //Logger.Log(spawnEnemyCount);
        if (spawnEnemyCount <= 0)
        {
            Logger.Log("Enemy All Clear");
            ExitBattle();
        }
    }

    /// <summary>
    /// 전투 종료 (베틀씬 Unload → 탐험씬 Additive Load)
    /// </summary>
    public void ExitBattle()
    {
        Logger.Log("ExitBattle");
        StartCoroutine(ExitBattleRoutine());
    }

    IEnumerator ExitBattleRoutine()
    {
        Time.timeScale = 0;
        inputManager.inputActions.Disable();

        CompanionManager.Instance.EndBattleScene();
        EnemyManager.Instance.EndBattle();

        DefaultSceneToggle(true);
        BattleSceneToggle(false);

        yield return null;

        Time.timeScale = 1;
        inputManager.inputActions.Enable();
        Logger.Log("Default Scene Loaded & Active!");
    }

    public void DefaultSceneToggle(bool isOpen)
    {
        for (int i = 0; i < defalutObject.Count; i++)
        {
            defalutObject[i].SetActive(isOpen);
        }
    }

    public void BattleSceneToggle(bool isOpen)
    {
        for (int i = 0; i < battleObject.Count; i++)
        {
            battleObject[i].SetActive(isOpen);
        }
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}
