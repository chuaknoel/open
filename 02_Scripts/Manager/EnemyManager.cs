using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private Dictionary<string, IObjectPool<Enemy>> enemyPoolDictionary = new();
    public List<EnemyData> stageEnemiesData;
    private Dictionary<string, GameObject> enemyDictionary = new();

    private int minCount;
    private int maxCount;

    public Transform externalEnemyPos;
    public Transform internalEnemyPos;

    private ExternalEnemy currentExternalEnemy;

    public UnityAction OnEndBattle;

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

    public async void Init()
    {
        minCount = 2;
        maxCount = 8;
        for (int i = 0; i < stageEnemiesData.Count; i++)
        {
            string address = stageEnemiesData[i].enemyPrefabAddress;
            enemyDictionary[address] = await AddressableManager.Instance.LoadAsset<GameObject>(address);
            enemyPoolDictionary[address] = PoolingManager.Instance.CreatePool<Enemy>(address, ()=> CreateEnemy(address), 3);

            for (int j = 0; j < 3; j++)      
            {
                Enemy enemy = CreateEnemy(address);
                enemyPoolDictionary[address].Release(enemy);       
            }
        }
    }

    public Enemy CreateEnemy(string address)
    {
        GameObject enemyPrefab = Instantiate(enemyDictionary[address], internalEnemyPos);
        if (enemyPrefab.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Init();
            OnEndBattle += enemy.EndBattle;
            enemy.connectPool = enemyPoolDictionary[address];
            return enemy;
        }

        return null;
    }

    public int SetBattleScene(ExternalEnemy externalEnemy)
    {
        if (externalEnemy == null) return 0;

        Logger.Log("Battle");

        List<Enemy> enemies = new();

        currentExternalEnemy = externalEnemy;
        externalEnemy.gameObject.SetActive(false);

        Enemy enemy = enemyPoolDictionary[externalEnemy.enemy.enemyPrefabAddress].Get();
        Vector2 lookDir = DetermineDir(enemy.transform.position);
        enemy.SetBattle(lookDir);


        int randomSpawnCount = Random.Range(minCount, maxCount);
        for (int i = 0; i < randomSpawnCount; i++)
        {
            int spawnEnemy = Random.Range(0, externalEnemy.enemyGroup.Count);
            Enemy randomEnemy = enemyPoolDictionary[externalEnemy.enemyGroup[spawnEnemy].enemyPrefabAddress].Get();
            randomEnemy.SetBattle(lookDir);
            enemies.Add(randomEnemy);
        }

        SetEnemiesFormation(enemies, enemy.transform.position, lookDir, 3);

        return randomSpawnCount + 1; //랜덤으로 소환되는 enemy와 무조건 소환되는 enemy의 수를 합쳐야 베틀씬에 소환된 enemy 수가 된다.
    }

    public void SetEnemiesFormation(List<Enemy> enemies, Vector3 center, Vector2 lookDir, float radius)
    {
        int count = enemies.Count;
        Logger.Log(count);
        if (count < 2) return;

        float angleStep = 180f / (count - 1);  // 반원 기준으로 분포
        float startAngle = -90f;               // enemy 세팅 시작 점 

        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, lookDir); //플레이어를 바라보는 방향 기준으로 포지션을 잡기 위한 rotation offset

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i; //enemy 배치 간격
            float rad = angle * Mathf.Deg2Rad;        //배치된 Enemy의 각도

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius; //원을 구해야되기 떄문에 cos으로 x축을 sin으로 y축을 구해준다.
            offset = rotation * offset;                                               //바라 보는 방향으로 위치값을 회전시켜준다.
            enemies[i].transform.position = center + offset;                          //센터를 중심으로 enemy를 배치한다.
        }
    }

    public Vector2 DetermineDir(Vector3 center)
    {
        Vector2 lookDir = Vector2.zero;

        Vector3 moveDir = (PlayManager.Instance.player.transform.position - center).normalized;

        float absX = Mathf.Abs(moveDir.x);
        float absY = Mathf.Abs(moveDir.y);

        float diff = Mathf.Abs(absX - absY);
        
        if (absX > absY)
        {
            if (moveDir.x > 0)
            {
                lookDir = Vector2.right;
            }
            else
            {
                lookDir = Vector2.left;
            }
        }
        else
        {
            if (moveDir.y > 0)
            {
                lookDir = Vector2.up;
            }
            else
            {
                lookDir = Vector2.down;
            }
        }

        return lookDir;
    }

    public void EndBattle()
    {
        if (currentExternalEnemy == null) return;

        currentExternalEnemy.gameObject.SetActive(true);
        currentExternalEnemy.isDeath = true;
        currentExternalEnemy.ChangeState(StateEnum.Death);
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
            OnEndBattle = null;
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
/*
    public static EnemyManager Instance { get; private set; }

    public Vector2 currentPlayerPosition;

    public List<Enemy> enemyList;

    public List<ExternalEnemy> externalEnemyList;

    public ExternalEnemy externalEnemy;

    public int activatedEnemyCount;

    public GameObject player;

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

    private void Start()
    {
        //AddExternalEnemyList();
    }

    public void Init()
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
*/