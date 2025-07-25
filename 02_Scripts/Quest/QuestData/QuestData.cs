using UnityEngine;

public abstract class QuestData : ScriptableObject, IQuestData
{
    public abstract CompleteType CompleteType { get; } // 퀘스트 완료 타입 (자동, 기본 등)
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
    public abstract RewardType RewardType { get; } // 보상 타입 (경험치, 아이템 등)
    public abstract string RewardItemId { get; }
    public abstract int RewardAmount { get; }
}

public enum CompleteType
{
    Auto,
    Default,
}

public enum RewardType
{
    Exp, // 경험치 보상
    Item, // 아이템 보상
    Companion, // 동료 보상
    Gold // 골드 보상
}