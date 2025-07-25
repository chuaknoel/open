public class TalkCondition : IQuestCondition
{
    private Quest questState;
    private string targetNpcId;

    public void Initialize(Quest state)
    {
        questState = state;
        targetNpcId = state.questData.TargetId;
        questState.UpdateProgress(state.questData.Amount);
        Logger.Log($"[TalkCondition] 즉시 완료 가능 체크: {state.questData.Title}");
    }

    public void OnEventTriggered(object data)
    {
        if (data is string talkedNpcId && talkedNpcId == targetNpcId)
        {
            questState.UpdateProgress(questState.questData.Amount);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }
}