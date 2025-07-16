using System.Collections.Generic;
using UnityEngine;

public class SubQuestSystem : MonoBehaviour
{
    [Header("SubQuest List")]
    [SerializeField] private List<Quest> activeSubQuests = new();
    [SerializeField] private List<Quest> completedSubQuests = new();
    [Header("Prefabs")]
    [SerializeField] private GameObject questLocationZonePrefab;

    private Dictionary<int, Quest> subQuestStates = new();

    [SerializeField] private QuestConditionChecker conditionChecker;

    public static SubQuestSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
            GameObject zone = Instantiate(questLocationZonePrefab);
            var trigger = zone.GetComponent<LocationTrigger>();
            if (trigger != null)
            {
                trigger.questId = quest.questData.QuestId;
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
                Logger.Log($"[SubQuest] 완료 조건 달성: {quest.questData.Title}");
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
}