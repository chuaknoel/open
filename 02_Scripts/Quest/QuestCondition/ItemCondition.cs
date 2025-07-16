public class ItemCondition : IQuestCondition
{
    private Quest questState;
    private string requiredItemId;

    public void Initialize(Quest state)
    {
        questState = state;
        requiredItemId = state.questData.TargetId;
    }

    public void OnEventTriggered(object data)
    {
        if (data is (string itemId, int count) && itemId == requiredItemId)
        {
            questState.UpdateProgress(count);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }
}