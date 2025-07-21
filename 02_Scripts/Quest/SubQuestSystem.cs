using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Pool;
using static Constants;

public class SubQuestSystem : MonoBehaviour
{
    [SerializeField] private QuestConditionChecker conditionChecker;
    [Header("SubQuest List")]
    private List<Quest> activeSubQuests = new();
    private List<Quest> completedSubQuests = new();
    [Header("Addressable")]
    private Dictionary<string, GameObject> gameObjects = new();
    private List<string> addressList = new List<string>()
    {
        GameObjects.QuestLocationZonePrefab,
    };

    private Dictionary<int, Quest> subQuestStates = new();

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
            var obj = await AddressableManager.Instance.LoadAsset<GameObject>(address);
            if (obj != null && !gameObjects.ContainsKey(address))
            {
                gameObjects[address] = obj;
                var trigger = obj.GetComponent<LocationTrigger>();
                locationTriggerPool = PoolingManager.Instance.CreatePool<LocationTrigger>(GameObjects.QuestLocationZonePrefab, CreateLocationTrigger, 3);
                for (int i = 0; i < 3; i++)
                {
                    var locationTrigger = CreateLocationTrigger();
                    locationTriggerPool.Release(locationTrigger); // 미리 만들어 놓은 오브젝트이기 때문에 회수하여 저장
                }
            }
        }
    }

    public LocationTrigger CreateLocationTrigger()
    {
        GameObject zone = Instantiate(gameObjects[GameObjects.QuestLocationZonePrefab]);
        var trigger = zone.GetComponent<LocationTrigger>();
        trigger.Init(locationTriggerPool);
        return trigger;
    }

    public void AcceptSubQuest(Quest quest)
    {
        int id = quest.questData.QuestId;
        if (subQuestStates.ContainsKey(id)) return;

        quest.StartQuest();
        subQuestStates.Add(id, quest);
        activeSubQuests.Add(quest);
        conditionChecker.Register(quest);

        Logger.Log($"[SubQuest] 수락: {quest.questData.Title}");

        if (quest.questData.QuestType == QuestType.Talk)
        {
            quest.UpdateProgress(quest.questData.Amount);
            Logger.Log($"[SubQuest] Talk 퀘스트 즉시 완료 가능: {quest.questData.Title}");
        }

        if (quest.questData.QuestType == QuestType.Location)
        {
            var trigger = locationTriggerPool.Get();
            trigger.questId = quest.questData.QuestId;
            var parts = quest.questData.TargetId.Split('.');
            if (parts.Length == 3&& float.TryParse(parts[0], out var px) && float.TryParse(parts[1], out var py) && float.TryParse(parts[2], out var pz))
            {
                trigger.transform.position = new Vector3(px, py, pz);
            }

            var collider = trigger.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                float size = Mathf.Max(1f, quest.questData.Amount);
                collider.size = new Vector2(size, size);
            }
        }
    }
    /// <summary>
    /// 해당 NPC에게 완료 보고를 시도합니다.
    /// 완료 가능 상태인 퀘스트를 완료 처리하고 리스트에 담습니다.
    /// </summary>
    public void TryCompleteSubQuestByNpcId(string npcId)
    {
        foreach (var quest in subQuestStates.Values)
        {
            if (quest.progressState == QuestProgressState.CanComplete &&
                quest.questData.ClearNpcID == npcId)
            {
                quest.CompleteQuest();
                conditionChecker.Unregister(quest.questData.QuestId);
                activeSubQuests.Remove(quest);
                completedSubQuests.Add(quest);
                Logger.Log($"[SubQuest] 완료: {quest.questData.Title}");
                return;
            }
        }
    }
    /// <summary>
    /// 진행 중인 서브 퀘스트의 진행량을 갱신합니다.
    /// </summary>
    public void UpdateSubQuestProgress(int questId, int amount)
    {
        if (subQuestStates.TryGetValue(questId, out var quest))
        {
            quest.UpdateProgress(amount);
            if (quest.progressState == QuestProgressState.CanComplete)
            {
                Logger.Log($"[SubQuest] 완료 조건 달성: {quest.questData.Title}");
            }
        }
        else
        {
            Logger.Log($"서브 퀘스트 {questId} 가 진행 중이 아닙니다.");
        }
    }
    /// <summary>
    /// 특정 퀘스트의 현재 상태를 반환합니다. 없으면 null.
    /// </summary>
    public Quest GetQuestState(int questId) => subQuestStates.TryGetValue(questId, out var state) ? state : null;
    /// <summary>
    /// 특정 퀘스트가 완료되었는지 확인합니다.
    /// </summary>
    public bool IsQuestCompleted(int questId) => subQuestStates.TryGetValue(questId, out var state) && state.IsCompleted;
    /// <summary>
    /// 수락은 되었지만 아직 완료되지 않은 모든 서브 퀘스트 상태를 반환합니다.
    /// </summary>
    public IEnumerable<Quest> GetAllActiveSubQuestStates() =>subQuestStates.Values;
    /// <summary>
    /// 지금까지 완료된 서브 퀘스트 상태들의 리스트 (읽기 전용)
    /// </summary>
    public IReadOnlyList<Quest> CompletedSubQuests => completedSubQuests;

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