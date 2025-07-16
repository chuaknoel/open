using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class NpcDialogue : MonoBehaviour
{
    [SerializeField] private NpcDialogueUI dialogueUI;
    [SerializeField] private MainQuestSystem mainQuestSystem;
    [SerializeField] private SubQuestSystem subQuestSystem;
    [SerializeField] private SubQuestDialogue subQuestDialogue;

    private Npc _currentNpc;
    private int _dialogueIndex;

    private void OnEnable()
    {
        Npc.OnNpcInteracted += StartDialogue;
    }

    private void OnDisable()
    {
        Npc.OnNpcInteracted -= StartDialogue;
    }

    private void StartDialogue(Npc npc)
    {
        _currentNpc = npc;
        _dialogueIndex = 0;

        dialogueUI.ShowGreeting(
            speakerName: npc.NpcData.npcName,
            text: npc.NpcData.greeting,
            onContinue: ShowChoices
        );
    }

    private void ShowChoices()
    {
        // 1) 기본 선택지(대화하기, 상점, 강화, 메인퀘스트) 표시
        dialogueUI.ShowChoices(
            speakerName: _currentNpc.NpcData.npcName,
            onTalk: ContinueConversation,
            onShop: _currentNpc.NpcData.npcType == NpcType.Shop ? (UnityAction)OpenShop : null,
            onEnhance: _currentNpc.NpcData.npcType == NpcType.Enhance ? (UnityAction)OpenEnhance : null,
            onMainQuest: GetMainQuestAction(),
            onSubQuest: null
        );

        // 2) 완료 가능한 서브퀘스트들 (NPC 배열 먼저, 없으면 전역 리스트)
        var completableList = _currentNpc.SubQuests.Where(quest => quest.progressState == QuestProgressState.CanComplete &&((SubQuestData)quest.questData).ClearNpcID == _currentNpc.NpcData.npcId).ToList();

        if (!completableList.Any())
        {
            completableList = subQuestSystem.GetAllActiveSubQuestStates().Where(quest =>quest.progressState == QuestProgressState.CanComplete &&((SubQuestData)quest.questData).ClearNpcID == _currentNpc.NpcData.npcId).ToList();
        }

        foreach (var quest in completableList)
        {
            dialogueUI.CreateButtonNoClose(label: quest.questData.Title,callback: () =>subQuestDialogue.StartSubQuestDialogue(quest, _currentNpc.NpcData.npcId));
        }

        // 3) 시작 가능한(NotStarted) 서브퀘스트들
        var startableList = _currentNpc.SubQuests.Where(quest => quest.progressState == QuestProgressState.NotStarted).ToList();

        foreach (var quest in startableList)
        {
            dialogueUI.CreateButtonNoClose(label: quest.questData.Title,callback: () =>
                {
                    subQuestDialogue.StartSubQuestDialogue(quest, _currentNpc.NpcData.npcId);
                }
            );
        }
    }

    /// <summary>
    /// “대화하기” 버튼을 눌렀을 때 호출됩니다.
    /// dialogueList 배열을 순차적으로 보여주고, 끝나면 UI를 닫습니다.
    /// </summary>
    private void ContinueConversation()
    {
        var lines = _currentNpc.NpcData.dialogueList;
        bool isLast = (_dialogueIndex >= lines.Length - 1);
        string next = lines[_dialogueIndex++];
        if (isLast)
        {
            dialogueUI.UpdateDialogue(next,dialogueUI.Close,"대화종료");
        }
        else
        {
            dialogueUI.UpdateDialogue(next,ContinueConversation,"다음");
        }
    }

    private UnityAction GetMainQuestAction()
    {
        if (mainQuestSystem == null) return null;

        var list = mainQuestSystem.GetMainQuestList() ?? new List<MainQuestData>();
        var available = list.Find(q =>
            q.StartNpcID == _currentNpc.NpcData.npcId &&
            !mainQuestSystem.IsQuestCompleted(q.QuestId) &&
            (q.PrerequisiteQuestId == 0 || mainQuestSystem.IsQuestCompleted(q.PrerequisiteQuestId))
        );
        return available == null
            ? null
            : new UnityAction(() =>
                mainQuestSystem.TryStartMainQuestByNpcId(_currentNpc.NpcData.npcId)
              );
    }

    private void OpenShop()
    {
        // TODO: 상점 UI 열기
    }

    private void OpenEnhance()
    {
        // TODO: 강화 UI 열기
    }
}