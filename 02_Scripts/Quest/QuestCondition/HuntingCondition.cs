public class HuntingCondition : IQuestCondition
{
    private Quest questState;
    private string requiredMonsterId;

    public void Initialize(Quest state)
    {
        questState = state;
        requiredMonsterId = state.questData.TargetId;

        QuestEvents.OnMonsterKilled += HandleMonsterKilled;
    }


    public void OnEventTriggered(object data)
    {
        if (data is string monsterId)
        {
            HandleMonsterKilled(monsterId);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }

    public void Dispose()
    {
        QuestEvents.OnMonsterKilled -= HandleMonsterKilled;
    }

    private void HandleMonsterKilled(string killedMonsterId)
    {
        if (questState.progressState != QuestProgressState.InProgress) return;
        if (killedMonsterId != requiredMonsterId) return;

        questState.UpdateProgress(1);
    }
}