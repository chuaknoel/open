public class HuntingCondition : IQuestCondition
{
    private Quest questState;
    private string requiredMonsterId;

    public void Initialize(Quest state)
    {
        questState = state;
        requiredMonsterId = state.questData.TargetId;
    }

    public void OnEventTriggered(object data)
    {
        if (data is string killedMonsterId && killedMonsterId == requiredMonsterId)
        {
            questState.UpdateProgress(1);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }
}