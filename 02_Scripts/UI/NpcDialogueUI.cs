using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class NpcDialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueContentText;
    [SerializeField] private Button continueButton;

    [Header("Choices")]
    [SerializeField] private GameObject choiceContainer;
    [SerializeField] private GameObject buttonPrefab;

    private UnityAction _onContinue;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
        choiceContainer.SetActive(false);
    }

    public void ShowGreeting(string speakerName, string text, UnityAction onContinue)
    {
        dialoguePanel.SetActive(true);
        choiceContainer.SetActive(false);
        speakerNameText.text = speakerName;
        dialogueContentText.text = text;
        SetupContinue(onContinue, "계속");
    }

    public void ShowChoices(string speakerName,UnityAction onTalk,UnityAction onShop,UnityAction onEnhance,UnityAction onMainQuest,UnityAction onSubQuest)
    {
        dialoguePanel.SetActive(false);
        choiceContainer.SetActive(true);
        speakerNameText.text = speakerName;

        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (onTalk != null)CreateButtonNoClose("대화하기", onTalk);
        if (onShop != null) CreateButton("상점", onShop);
        if (onEnhance != null) CreateButton("강화하기", onEnhance);
        if (onMainQuest != null) CreateButton("메인퀘스트", onMainQuest);
        if (onSubQuest != null) CreateButtonNoClose("서브퀘스트", onSubQuest);
    }

    public void UpdateDialogue(string text, UnityAction onContinue)
    {
        SetupDialoguePanel();
        dialogueContentText.text = text;
        SetupContinue(onContinue, "계속");
    }

    public void UpdateDialogue(string text, UnityAction onContinue, string continueLabel)
    {
        SetupDialoguePanel();
        dialogueContentText.text = text;
        SetupContinue(onContinue, continueLabel);
    }

    public void Close()
    {
        dialoguePanel.SetActive(false);
        choiceContainer.SetActive(false);
    }

    public void CreateButtonNoClose(string label, UnityAction callback)
    {
        var obj = Instantiate(buttonPrefab, choiceContainer.transform);
        var text = obj.GetComponentInChildren<TMP_Text>();
        var btn = obj.GetComponentInChildren<Button>();

        text.text = label;
        btn.onClick.AddListener(() =>
        {
            callback?.Invoke();
        });
    }

    /// <summary>
    /// 대화창을 닫고, choiceContainer에
    /// "수락하기" / "거절하기" 버튼을 띄웁니다.
    /// </summary>
    public void ShowAcceptRefuse(UnityAction onAccept, UnityAction onRefuse)
    {
        dialoguePanel.SetActive(false);
        ClearChoices();
        choiceContainer.SetActive(true);

        CreateButton("수락하기", onAccept);
        CreateButtonNoClose("거절하기", onRefuse);
    }

    private void SetupDialoguePanel()
    {
        dialoguePanel.SetActive(true);
        choiceContainer.SetActive(false);
    }

    private void SetupContinue(UnityAction onContinue, string label)
    {
        _onContinue = onContinue;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => _onContinue());
        var text = continueButton.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = label;
        continueButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// choiceContainer에 버튼을 생성합니다.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="callback"></param>
    private void CreateButton(string label, UnityAction callback)
    {
        var go = Instantiate(buttonPrefab, choiceContainer.transform);
        var txt = go.GetComponentInChildren<TMP_Text>();
        var btn = go.GetComponentInChildren<Button>();

        txt.text = label;
        btn.onClick.AddListener(() =>
        {
            callback?.Invoke();
            Close();
        });
    }

    /// <summary>
    /// choiceContainer의 모든 버튼을 제거합니다.
    /// </summary>
    private void ClearChoices()
    {
        foreach (Transform t in choiceContainer.transform)
            Destroy(t.gameObject);
    }
}