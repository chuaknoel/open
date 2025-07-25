using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterQuest(Quest quest)
    {
        quest.OnQuestCompleted += HandleQuestCompleted;
    }

    private void HandleQuestCompleted(Quest quest)
    {
        GiveReward(quest.questData);
        Logger.Log($"[QuestManager] Quest {quest.questData.QuestId} Completed.");
    }

    public void GiveReward(QuestData data)
    {
        switch (data.RewardType)
        {
            case RewardType.Exp:
                // Player.Instance.AddExp(data.RewardAmount);
                Logger.Log($"[QuestManager] Exp {data.RewardAmount} 지급");
                break;

            case RewardType.Item:
                // InventoryManager.Instance.AddItem(data.RewardItemId, data.RewardAmount);
                Logger.Log($"[QuestManager] Item {data.RewardItemId} ×{data.RewardAmount} 지급");
                break;

            case RewardType.Gold:
                // Player.Instance.AddGold(data.RewardAmount);
                Logger.Log($"[QuestManager] Gold {data.RewardAmount} 지급");
                break;

            case RewardType.Companion:
                CompanionManager.Instance.JoinParty(data.RewardItemId);
                Logger.Log($"[QuestManager] Companion {data.RewardItemId} 합류");
                break;
        }
    }
}