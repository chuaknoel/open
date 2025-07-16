public interface IQuestCondition
{
    void Initialize(Quest questState);
    void OnEventTriggered(object data);
    bool IsConditionMet();
}