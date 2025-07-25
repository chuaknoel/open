using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Pool;

public class LocationCondition : IQuestCondition
{
    private Quest questState;
    private IObjectPool<LocationTrigger> pool;
    private LocationTrigger trigger;

    public LocationCondition(IObjectPool<LocationTrigger> pool)
    {
        this.pool = pool;
    }

    public void Initialize(Quest state)
    {
        questState = state;
        trigger = pool.Get();
        trigger.questId = state.questData.QuestId;

        var parts = questState.questData.TargetId.Split('.');
        if (parts.Length == 3 && float.TryParse(parts[0], out var x)&& float.TryParse(parts[1], out var y) && float.TryParse(parts[2], out var z))
        {
            trigger.transform.position = new Vector3(x, y, z);
        }

        if (trigger.TryGetComponent<BoxCollider2D>(out var colider))
        {
            float size = Mathf.Max(1f, questState.questData.Amount);
            colider.size = new Vector2(size, size);
        }
    }

    public void OnEventTriggered(object data)
    {
        if (data is int locationID && locationID == questState.questData.QuestId)
        {
            questState.UpdateProgress(questState.questData.Amount);
        }
    }

    public bool IsConditionMet()
    {
        return questState.progressState == QuestProgressState.CanComplete;
    }
}