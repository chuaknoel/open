using UnityEngine;

public abstract class QuestData : ScriptableObject, IQuestData
{
    public abstract QuestType QuestType { get; }
    public abstract int QuestId { get; }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string TargetId { get; }
    public abstract int Amount { get; }

    public abstract string[] DialogueList { get; }
    public abstract string[] ClearDialogue { get; }
    public abstract string InprogressDialogue { get; }
    public abstract string RefusalDialogue { get; }

    public abstract string ClearNpcID { get; }
    public abstract string RewardItemId { get; }
    public abstract int RewardAmount { get; }
}