using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public Vector2 currentPlayerPosition;

    public List<Enemy> enemyList;

    public List<ExternalEnemy> externalEnemyList;

    public ExternalEnemy externalEnemy;

    public int activatedEnemyCount;

    public GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AddExternalEnemyList();
    }

    public void StartBattleScene()
    {
        StartCoroutine(LoadBattleSceneRoutine());
    }

    private IEnumerator LoadBattleSceneRoutine()
    {
        SceneManager.LoadScene("Tutorial_BattleScene");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Tutorial_BattleScene");

        AddEnemyList();
        DrawEnemy(externalEnemy.enemyEnum);

        GetPlayer();
        player.transform.position = currentPlayerPosition;
    }

    private void GetPlayer()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    public void AddEnemyList()
    {
        Enemy[] allEnemies = Resources.FindObjectsOfTypeAll<Enemy>();

        foreach(var enemy in allEnemies)
        {
            enemyList.Add(enemy);
        }
    }

    public void AddExternalEnemyList()
    {
        externalEnemy = FindObjectOfType<ExternalEnemy>();

        externalEnemyList.Add(externalEnemy);
    }

    public void DrawEnemy(EnemyEnum enemyEnum)
    {
        foreach (var enemy in enemyList)
        {
            if (enemy.enemyEnum == enemyEnum)
            {
                enemy.gameObject.SetActive(true);
                activatedEnemyCount++;
            }
        }
    }

    public void ExitBattleScene()
    {
        //플레이어가 죽으면 return

        activatedEnemyCount--;
        currentPlayerPosition = player.transform.position;

        if ( activatedEnemyCount == 0)
        {
            StartCoroutine(LoadMainSceneRoutine());  
        }
    }

    private IEnumerator LoadMainSceneRoutine()
    {
        SceneManager.LoadScene("TutorialScene");

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "TutorialScene");

        GetPlayer();
        AddExternalEnemyList();

        externalEnemy.transform.gameObject.SetActive(false);
        player.transform.position = currentPlayerPosition;
    }
}
