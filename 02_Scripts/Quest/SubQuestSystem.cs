using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static Constants;

public class SubQuestSystem : MonoBehaviour
{
    [SerializeField] private QuestConditionChecker conditionChecker;

    [Header("SubQuest Lists")]
    private readonly List<Quest> activeSubQuests = new();
    private readonly List<Quest> completedSubQuests = new();

    [Header("Addressables")]
    private readonly Dictionary<string, GameObject> gameObjects = new();
    private readonly List<string> addressList = new()
    {
        GameObjects.QuestLocationZonePrefab
    };

    private readonly Dictionary<int, Quest> subQuestStates = new();
    private IObjectPool<LocationTrigger> locationTriggerPool;

    public static SubQuestSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private async void Start()
    {
        foreach (var address in addressList)
        {
            var prefab = await AddressableManager.Instance.LoadAsset<GameObject>(address);
            if (prefab != null && !gameObjects.ContainsKey(address))
            {
                gameObjects[address] = prefab;
                locationTriggerPool = PoolingManager.Instance.CreatePool<LocationTrigger>(GameObjects.QuestLocationZonePrefab, CreateLocationTrigger, 3);

                for (int i = 0; i < 3; i++)
                {
                    var trigger = CreateLocationTrigger();
                    locationTriggerPool.Release(trigger);
                }
                QuestConditionFactory.InitializePool(locationTriggerPool);
            }
        }
    }
    /// <summary>
    /// LocationCondition에서 사용될 트리거를 생성합니다.
    /// </summary>
    public LocationTrigger CreateLocationTrigger()
    {
        var zoneObj = Instantiate(gameObjects[GameObjects.QuestLocationZonePrefab]);
        var trigger = zoneObj.GetComponent<LocationTrigger>();
        trigger.Init(locationTriggerPool);
        return trigger;
    }
    /// <summary>
    /// 서브 퀘스트를 수락하고, 타입별 Condition을 생성·초기화·등록합니다.
    /// </summary>
    public void AcceptSubQuest(Quest quest)
    {
        int id = quest.questData.QuestId;
        if (subQuestStates.ContainsKey(id))
        {
            Logger.LogError($"[SubQuest] 이미 수락된 퀘스트입니다: {quest.questData.Title}");
            return;
        }

        quest.StartQuest();
        subQuestStates.Add(id, quest);
        activeSubQuests.Add(quest);
        conditionChecker.Register(quest);
        QuestManager.Instance.RegisterQuest(quest);

        Logger.Log($"[SubQuest] 수락: {quest.questData.Title}");
    }
    /// <summary>
    /// NPC에게 보고했을 때 호출됩니다.
    /// ClearNpcID와 progress 상태를 보고 완료 처리합니다.
    /// </summary>
    public void TryCompleteSubQuestByNpcId(string npcId)
    {
        foreach (var quest in subQuestStates.Values)
        {
            if (quest.progressState == QuestProgressState.CanComplete && quest.questData.ClearNpcID == npcId)
            {
                ForceCompleteQuest(quest.questData.QuestId);
                return;
            }
        }
    }
    /// <summary>
    /// ConditionChecker가 완료 조건을 감지했을 때 호출합니다.
    /// 실제 완료 처리(완료 메서드 호출, 보상, 리스트 이동 등)를 수행합니다.
    /// </summary>
    public void ForceCompleteQuest(int questId)
    {
        if (!subQuestStates.TryGetValue(questId, out var quest)) return;
        if (quest.progressState != QuestProgressState.CanComplete) return;

        quest.CompleteQuest();
        activeSubQuests.Remove(quest);
        completedSubQuests.Add(quest);

        Logger.Log($"[SubQuest] 완료: {quest.questData.Title}");
    }
    /// <summary>
    /// 외부에서 진행량을 갱신하고 싶을 때 사용합니다.
    /// </summary>
    public void UpdateSubQuestProgress(int questId, int amount)
    {
        if (subQuestStates.TryGetValue(questId, out var quest))
        {
            quest.UpdateProgress(amount);
            if (quest.progressState == QuestProgressState.CanComplete)
                Logger.Log($"[SubQuest] 완료 조건 달성: {quest.questData.Title}");
        }
        else
        {
            Logger.Log($"서브 퀘스트 {questId}가 진행 중이 아닙니다.");
        }
    }
    /// <summary>
    /// 현재 진행 중인 Quest 객체를 가져옵니다.
    /// </summary>
    public Quest GetQuestState(int questId) => subQuestStates.TryGetValue(questId, out var q) ? q : null;
    public bool IsQuestCompleted(int questId) => subQuestStates.TryGetValue(questId, out var s) && s.IsCompleted;
    public IEnumerable<Quest> GetAllActiveSubQuestStates() => subQuestStates.Values;
    public IReadOnlyList<Quest> CompletedSubQuests => completedSubQuests;

    /// <summary>
    /// 싱글턴 해제 처리
    /// </summary>
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

    private void OnDestroy()
    {
        UnLoad();
    }
}