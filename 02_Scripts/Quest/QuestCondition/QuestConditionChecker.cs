using System;
using System.Collections.Generic;
using UnityEngine;

public static class QuestEvents
{
    /// <summary>
    /// 몬스터가 죽었을 때 monsterId만 흘려줍니다.
    /// </summary>
    public static event Action<string> OnMonsterKilled;

    public static void MonsterKilled(string monsterId) => OnMonsterKilled?.Invoke(monsterId);

    /// <summary>
    /// 아이템을 획득했을 때 아이템 ID와 개수를 전달합니다.
    /// </summary>
    public static event Action<string, int> OnItemCollected;

    public static void ItemCollected(string itemId, int count) => OnItemCollected?.Invoke(itemId, count);
}

public class QuestConditionChecker : MonoBehaviour
{
    private readonly Dictionary<int, IQuestCondition> activeConditions = new();

    public void Register(Quest questState)
    {
        var condition = QuestConditionFactory.CreateCondition(questState.questData.QuestType);
        condition.Initialize(questState);
        activeConditions[questState.questData.QuestId] = condition;
    }

    public void Unregister(int questId)
    {
        if (activeConditions.TryGetValue(questId, out var cond))
        {
            activeConditions.Remove(questId);
        }
    }

    public void TriggerEvent(object data)
    {
        foreach (var condition in activeConditions.Values)
        {
            condition.OnEventTriggered(data);
        }
    }
}