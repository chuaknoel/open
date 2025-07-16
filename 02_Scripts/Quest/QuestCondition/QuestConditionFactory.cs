public static class QuestConditionFactory
{
    public static IQuestCondition CreateCondition(QuestType type)
    {
        return type switch
        {
            QuestType.Hunting => new HuntingCondition(),
            QuestType.Item => new ItemCondition(),
            QuestType.Talk => new TalkCondition(),
            QuestType.Location => new LocationCondition(),
            _ => null
        };
    }
}