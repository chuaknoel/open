using UnityEngine;

public class SubQuestDialogue : MonoBehaviour
{
    [SerializeField] private NpcDialogueUI dialogueUI;
    [SerializeField] private MainQuestSystem mainQuestSystem;
    [SerializeField] private SubQuestSystem subQuestSystem;

    private Quest _currentQuest;
    private string _npcId;
    private string[] _lines;
    private int _index;
    private bool _isAcceptFlow;

    public void Init()
    {
        subQuestSystem = SubQuestSystem.Instance;
    }

    /// <summary>
    /// 수락 전(NotStarted) 혹은 완료 가능(CanComplete) 상태의 Quest를
    /// 받아서 대화 흐름(수락/완료)을 수행합니다.
    /// </summary>
    public void StartSubQuestDialogue(Quest quest, string npcId)
    {
        _currentQuest = quest;
        _npcId = npcId;
        _index = 0;

        // 1) 수락 전
        if (quest.progressState == QuestProgressState.NotStarted)
        {
            _isAcceptFlow = true;
            _lines = quest.questData.DialogueList;
        }
        // 2) 완료 가능
        else if (quest.progressState == QuestProgressState.CanComplete)
        {
            _isAcceptFlow = false;
            _lines = ((SubQuestData)quest.questData).ClearDialogue;
        }
        // 3) 진행 중인 상태
        else
        {
            dialogueUI.ShowGreeting(
                speakerName: _currentQuest.questData.Title,
                text: _currentQuest.questData.Description,
                onContinue: dialogueUI.Close
            );
            return;
        }

        bool isLast = (_lines.Length == 1);
        string label = isLast ? "대화종료" : "다음";
        dialogueUI.UpdateDialogue(_lines[0], ContinueSubQuest, label);
    }

    private void ContinueSubQuest()
    {
        _index++;

        if (_index < _lines.Length)
        {
            bool isLast = (_index == _lines.Length - 1);
            string label = isLast ? "대화종료" : "다음";
            dialogueUI.UpdateDialogue(_lines[_index], ContinueSubQuest, label);
            return;
        }

        if (_isAcceptFlow)
        {
            dialogueUI.ShowAcceptRefuse(
                onAccept: () =>
                {
                    subQuestSystem.AcceptSubQuest(_currentQuest);
                    dialogueUI.Close();
                },
                onRefuse: () =>
                {
                    dialogueUI.ShowGreeting(speakerName: _currentQuest.questData.Title,text: _currentQuest.questData.RefusalDialogue,onContinue: dialogueUI.Close);
                }
            );
        }
        else
        {
            subQuestSystem.TryCompleteSubQuestByNpcId(_npcId);
            dialogueUI.Close();
        }
    }
}