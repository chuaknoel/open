using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System; // Action을 사용하기 위해 추가

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // --- 차후 CSV 확장을 위한 코드 (현재는 사용 안 함) ---
    // private Dictionary<string, List<DialogueLine>> dialogueDB = new Dictionary<string, List<DialogueLine>>();

    private List<DialogueLine> currentDialogue;
    private int currentIndex;
    private Action onDialogueEndCallback; // 대화 종료 후 실행될 액션(콜백)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /*
    // --- 차후 CSV 확장을 위한 메서드 ---
    public void SetDialogueData(Dictionary<string, List<DialogueLine>> db)
    {
        // dialogueDB = db;
    }

    public void StartDialogueFromCSV(string dialogueID, Action onEndCallback = null)
    {
        // if (!dialogueDB.ContainsKey(dialogueID)) return;
        // StartDialogue(dialogueDB[dialogueID], onEndCallback);
    }
    */

    /// <summary>
    /// 인스펙터에서 만든 대화 리스트로 대화를 시작합니다.
    /// </summary>
    public void StartDialogue(List<DialogueLine> dialogue, Action onEndCallback = null)
    {
        // 대화 내용이 없으면 아무것도 하지 않고 즉시 콜백 실행
        if (dialogue == null || dialogue.Count == 0)
        {
            onEndCallback?.Invoke();
            return;
        }

        currentDialogue = dialogue;
        onDialogueEndCallback = onEndCallback;
        currentIndex = 0;
        ShowCurrentLine();
    }

    public void AdvanceDialogue()
    {
        currentIndex++;
        if (currentIndex < currentDialogue.Count)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue[currentIndex];
        DialogueUI.Instance.ShowDialogue(line.SpeakerName, line.Text);
    }

    public void EndDialogue()
    {
        DialogueUI.Instance.CloseUI();
        currentDialogue = null;

        // 저장해둔 콜백 액션을 실행
        onDialogueEndCallback?.Invoke();
        onDialogueEndCallback = null; // 콜백 초기화
    }
}