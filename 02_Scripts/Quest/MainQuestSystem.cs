using System.Collections.Generic;
using UnityEngine;

public class MainQuestSystem : MonoBehaviour
{
    [SerializeField] private QuestConditionChecker conditionChecker;
    [SerializeField] private List<MainQuestData> mainQuestList;
    private Dictionary<int, Quest> mainQuestStates = new();
    private List<Quest> completedMainQuests = new();
    private bool canStart; // 퀘스트 시작 가능 여부

    public static MainQuestSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    /// <summary>
    /// 주어진 NPC ID와 일치하는 시작 조건의 메인 퀘스트가 존재하고, 해당 퀘스트가 시작 가능한 상태일 경우 시작
    /// </summary>
    /// <param name="npcId"></param>
    public void TryStartMainQuestByNpcId(string npcId)
    {
        canStart = false;
        foreach (var data in mainQuestList)
        {
            canStart = data.PrerequisiteQuestId == 0 || IsQuestCompleted(data.PrerequisiteQuestId);

            if (data.StartNpcID == npcId &&
                !mainQuestStates.ContainsKey(data.QuestId) &&
                canStart)
            {
                Quest state = new Quest(data);
                state.StartQuest();
                mainQuestStates.Add(data.QuestId, state);
                conditionChecker.Register(state);

                Logger.Log($"[MainQuest] 시작: {data.Title}");
                return;
            }
        }
    }
    /// <summary>
    /// 주어진 NPC ID와 일치하는 완료 조건의 메인 퀘스트가 존재하고, 해당 퀘스트가 완료 가능한 상태일 경우 완료
    /// </summary>
    /// <param name="npcId"></param>
    public void TryCompleteMainQuestByNpcId(string npcId)
    {
        foreach (var pair in mainQuestStates)
        {
            var state = pair.Value;
            if (state.progressState == QuestProgressState.CanComplete &&
                state.questData.ClearNpcID == npcId)
            {
                state.CompleteQuest();
                completedMainQuests.Add(state);

                Logger.Log($"[MainQuest] 완료: {state.questData.Title}");
                return;
            }
        }
    }
    /// <summary>
    /// 해당 퀘스트가 진행중인지 확인, 퀘스트의 상태가 완료됐는지 확인
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public bool IsQuestCompleted(int questId) => mainQuestStates.TryGetValue(questId, out var state) && state.IsCompleted;
    /// <summary>
    /// 퀘스트 ID로 퀘스트 상태를 찾기
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public Quest GetQuestState(int questId) => mainQuestStates.TryGetValue(questId, out var state) ? state : null;

    public List<MainQuestData> GetMainQuestList()
    {
        return mainQuestList;
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

    private void OnDestroy()
    {
        UnLoad();
    }
}