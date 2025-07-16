using System.Collections.Generic;
using UnityEngine;

public class QuestConditionChecker : MonoBehaviour
{
    private Dictionary<int, IQuestCondition> activeConditions = new();

    public void Register(Quest questState)
    {
        var condition = QuestConditionFactory.CreateCondition(questState.questData.QuestType);
        condition.Initialize(questState);
        activeConditions[questState.questData.QuestId] = condition;
    }

    public void TriggerEvent(int questId, object data)
    {
        if (activeConditions.TryGetValue(questId, out var condition))
        {
            condition.OnEventTriggered(data);
        }
    }

    public bool IsMet(int questId)
    {
        return activeConditions.TryGetValue(questId, out var condition) && condition.IsConditionMet();
    }
}