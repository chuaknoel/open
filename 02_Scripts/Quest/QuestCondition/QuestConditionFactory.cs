using UnityEngine.Pool;

public static class QuestConditionFactory
{
    private static IObjectPool<LocationTrigger> locationTriggerPool;

    public static void InitializePool(IObjectPool<LocationTrigger> pool)
    {
        locationTriggerPool = pool;
    }

    public static IQuestCondition CreateCondition(QuestType type)
    {
        return type switch
        {
            QuestType.Hunting => new HuntingCondition(),
            QuestType.Item => new ItemCondition(),
            QuestType.Talk => new TalkCondition(),
            QuestType.Location => new LocationCondition(locationTriggerPool),
            _ => null
        };
    }
}