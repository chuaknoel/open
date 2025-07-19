using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 퀘스트의 진행 상태를 관리하고 UI에 필요한 정보를 제공하는 매니저입니다.
/// 메인퀘스트와 서브퀘스트의 진행/완료 상태를 추적합니다.
/// </summary>
public class QuestDataManager : MonoBehaviour
{
    public static QuestDataManager Instance { get; private set; }

    // 현재 진행 중인 퀘스트 ID들
    private List<string> activeQuestIDs = new List<string>();

    // 완료된 퀘스트 ID들
    private List<string> completedQuestIDs = new List<string>();

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

    /// <summary>
    /// 새로운 퀘스트를 활성화합니다.
    /// </summary>
    public void ActivateQuest(string questID)
    {
        if (!activeQuestIDs.Contains(questID) && !completedQuestIDs.Contains(questID))
        {
            activeQuestIDs.Add(questID);
        }
    }

    /// <summary>
    /// (임시) 퀘스트를 수락하는 기능입니다.
    /// </summary>
    public void AcceptQuest(string questId)
    {
        if (string.IsNullOrEmpty(questId)) return;

        // 이미 진행 중이거나 완료한 퀘스트인지 확인하는 로직 (생략)

        Debug.Log($"퀘스트 '{questId}'를 수락했습니다!");
        // InProgressQuests.Add(questId); // 실제로는 이런 코드가 들어가게 됩니다.
    }

    /// <summary>
    /// 퀘스트를 완료 처리합니다.
    /// </summary>
    public void CompleteQuest(string questID)
    {
        if (activeQuestIDs.Contains(questID))
        {
            activeQuestIDs.Remove(questID);
            completedQuestIDs.Add(questID);

            // 메인퀘스트 완료 시 다음 메인퀘스트 자동 활성화 로직 추가 가능
            var quest = GetQuestData(questID);
            //if (quest != null && quest.Type == "Main")
            //{
            //    // 여기에 다음 메인퀘스트 활성화 로직 추가
            //}
        }
    }

    /// <summary>
    /// UI에서 사용할 진행중 퀘스트 정보를 반환합니다.
    /// </summary>
    //public List<ActiveQuestInfo> GetActiveQuests()
    //{
    //    var result = new List<ActiveQuestInfo>();
    //    foreach (var id in activeQuestIDs)
    //    {
    //        var quest = GetQuestData(id);
    //        if (quest != null)
    //        {
    //            result.Add(new ActiveQuestInfo
    //            {
    //                Title = quest.Title,
    //                GiverNPC = quest.Type == "Main" ? "" : quest.GiverNPC,
    //                StoryDesc = quest.StoryDesc,
    //                QuestContent = quest.QuestContent,
    //                RewardItemID = quest.Type == "Main" ? "" : quest.RewardItemID
    //            });
    //        }
    //    }
    //    return result;
    //}

    /// <summary>
    /// UI에서 사용할 완료된 퀘스트 정보를 반환합니다.
    /// </summary>
    public List<CompletedQuestInfo> GetCompletedQuests()
    {
        var result = new List<CompletedQuestInfo>();
        foreach (var id in completedQuestIDs)
        {
            var quest = GetQuestData(id);
            if (quest != null)
            {
                //result.Add(new CompletedQuestInfo
                //{
                //    Title = quest.Title,
                //    GiverNPC = quest.Type == "Main" ? "" : quest.GiverNPC,
                //    CompletionStory = quest.CompletionStory
                //});
            }
        }
        return result;
    }

    private QuestData GetQuestData(string questID)
    {
        if (DataManager.Instance != null && DataManager.Instance.QuestDB.ContainsKey(questID))
        {
            return DataManager.Instance.QuestDB[questID];
        }
        return null;
    }
}

/// <summary>
/// UI에 전달할 진행중 퀘스트 정보 구조체
/// </summary>
[System.Serializable]
public struct ActiveQuestInfo
{
    public string Title;
    public string GiverNPC;      // 메인퀘스트는 빈 문자열
    public string StoryDesc;
    public string QuestContent;
    public string RewardItemID;  // 메인퀘스트는 빈 문자열
}

/// <summary>
/// UI에 전달할 완료된 퀘스트 정보 구조체
/// </summary>
[System.Serializable]
public struct CompletedQuestInfo
{
    public string Title;
    public string GiverNPC;      // 메인퀘스트는 빈 문자열
    public string CompletionStory;
}