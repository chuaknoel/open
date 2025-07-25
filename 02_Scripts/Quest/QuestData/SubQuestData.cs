using UnityEngine;

[CreateAssetMenu(fileName = "SubQuestData", menuName = "Quest/SubQuest")]
public class SubQuestData : QuestData
{
    [SerializeField] private CompleteType completeType;
    [SerializeField] private QuestType questType;
    [SerializeField] private int questId;
    [SerializeField] private string title;
    [SerializeField][TextArea] private string description;
    [SerializeField] private string targetId;
    [SerializeField] private int amount;
    [SerializeField] private string clearNpcID;

    [Header("Dialogue")]
    [SerializeField] private string[] dialogueList;
    [SerializeField] private string[] clearDialogue;
    [SerializeField] private string inprogressDialogue;
    [SerializeField][TextArea] private string refusalDialogue;

    [Header("Reward")]
    [SerializeField] private RewardType rewardType;
    [SerializeField] private string rewardItemId;
    [SerializeField] private int rewardAmount;

    public override CompleteType CompleteType => completeType; // 퀘스트 완료 타입 (자동, 기본 등)
    public override QuestType QuestType => questType;
    public override int QuestId => questId;
    public override string Title => title;
    public override string Description => description;
    public override string TargetId => targetId;
    public override int Amount => amount;
    public override string ClearNpcID => clearNpcID;

    public override string[] DialogueList => dialogueList;
    public override string[] ClearDialogue => clearDialogue;
    public override string InprogressDialogue => inprogressDialogue;
    public override string RefusalDialogue => refusalDialogue;

    public override RewardType RewardType => rewardType; // 보상 타입 (경험치, 아이템 등)
    public override string RewardItemId => rewardItemId;
    public override int RewardAmount => rewardAmount;
}