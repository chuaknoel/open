using UnityEngine;

[CreateAssetMenu(fileName = "MainQuestData", menuName = "Quest/MainQuest")]
public class MainQuestData : QuestData
{
    [SerializeField] private CompleteType completeType;
    [SerializeField] private QuestType questType;
    [SerializeField] private int questId;
    [SerializeField] private int prerequisiteQuestId;
    [SerializeField] private string title;
    [SerializeField][TextArea] private string description;
    [SerializeField] private string targetId;
    [SerializeField] private int amount;
    [SerializeField] private string startNpcID;
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

    public override CompleteType CompleteType => completeType;
    public override QuestType QuestType => questType;
    public override int QuestId => questId;
    public int PrerequisiteQuestId => prerequisiteQuestId;
    public override string Title => title;
    public override string Description => description;
    public override string TargetId => targetId;
    public override int Amount => amount;
    public string StartNpcID => startNpcID;
    public override string ClearNpcID => clearNpcID;

    public override string[] DialogueList => dialogueList;
    public override string[] ClearDialogue => clearDialogue;
    public override string InprogressDialogue => inprogressDialogue;
    public override string RefusalDialogue => refusalDialogue;

    public override RewardType RewardType => rewardType;
    public override string RewardItemId => rewardItemId;
    public override int RewardAmount => rewardAmount;
}