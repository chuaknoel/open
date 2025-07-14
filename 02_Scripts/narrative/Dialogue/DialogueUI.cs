using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI 컴포넌트")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button exitButton; // 선택 사항

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dialoguePanel.SetActive(false);
    }

    void Start()
    {
        // '다음' 버튼을 누르면 DialogueManager에게 다음 대사를 요청합니다.
        nextButton.onClick.AddListener(() => DialogueManager.Instance.AdvanceDialogue());

        // '나가기' 버튼이 있다면, 대화를 즉시 종료하도록 연결합니다.
        exitButton?.onClick.AddListener(() => DialogueManager.Instance.EndDialogue());
    }

    /// <summary>
    /// DialogueManager가 이 함수를 호출하여 UI를 업데이트합니다.
    /// </summary>
    public void ShowDialogue(string speakerName, string dialogue)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = speakerName;

        // 여기에 타이핑 효과를 추가할 수 있습니다. (지금은 즉시 표시)
        dialogueText.text = dialogue;
    }

    /// <summary>
    /// 대화 패널을 닫습니다.
    /// </summary>
    public void CloseUI()
    {
        dialoguePanel.SetActive(false);
    }
}