public class LocationCondition : IQuestCondition
{
    private Quest questState;

    public void Initialize(Quest state)
    {
        questState = state;
    }

    public void OnEventTriggered(object data)
    {
        questState.UpdateProgress(questState.questData.Amount); // 위치 도달 시 완료
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }
}