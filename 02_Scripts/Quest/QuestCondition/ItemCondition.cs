public class ItemCondition : IQuestCondition
{
    private Quest questState;
    private string requiredItemId;

    public void Initialize(Quest state)
    {
        questState = state;
        requiredItemId = state.questData.TargetId;
        QuestEvents.OnItemCollected += HandleItemCollected;
    }

    public void OnEventTriggered(object data)
    {
        if (data is (string itemId, int count))
        {
            HandleItemCollected(itemId, count);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }

    public void Dispose()
    {
        QuestEvents.OnItemCollected -= HandleItemCollected;
    }

    private void HandleItemCollected(string itemId, int count)
    {
        if (questState.progressState != QuestProgressState.InProgress) return;
        if (itemId != requiredItemId) return;

        questState.UpdateProgress(count);
    }
}