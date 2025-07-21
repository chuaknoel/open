using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InteractionChoiceUI : MonoBehaviour
{
    //public static InteractionChoiceUI Instance { get; private set; }

    //[SerializeField] private GameObject choicePanel;
    //[SerializeField] private GameObject choiceButtonPrefab;
    //[SerializeField] private Transform buttonsParent;

    //private List<GameObject> currentButtons = new List<GameObject>();

    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);
    //    choicePanel?.SetActive(false);
    //}

    //public void ShowChoices(NPC npc)
    //{
    //    foreach (var button in currentButtons) Destroy(button);
    //    currentButtons.Clear();

    //    // 상점 버튼
    //    if (npc.IsMerchant)
    //    {
    //        CreateButton("상점", () => {
    //            CloseUI();
    //            DialogueManager.Instance.StartDialogue(npc.PostShopDialogue, () => {
    //                Debug.Log("실제 상점 UI를 여기서 엽니다.");
    //            });
    //        });
    //    }

    //    // 퀘스트 버튼
    //    if (npc.HasQuest)
    //    {
    //        CreateButton("퀘스트", () => {
    //            CloseUI();
    //            // 1. 퀘스트를 먼저 지급
    //            QuestDataManager.Instance.AcceptQuest(npc.QuestIDToGive);
    //            // 2. 퀘스트 수락 후 대화 시작
    //            DialogueManager.Instance.StartDialogue(npc.PostQuestAcceptDialogue);
    //        });
    //    }

    //    // 일반 대화 버튼
    //    CreateButton("대화하기", () => {
    //        CloseUI();
    //        DialogueManager.Instance.StartDialogue(npc.GeneralDialogue);
    //    });

    //    CreateButton("떠나기", CloseUI);

    //    choicePanel.SetActive(true);
    //}

    //private void CreateButton(string text, UnityEngine.Events.UnityAction action)
    //{
    //    GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsParent);
    //    buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = text;
    //    buttonObj.GetComponent<Button>().onClick.AddListener(action);
    //    currentButtons.Add(buttonObj);
    //}

    //public void CloseUI()
    //{
    //    choicePanel.SetActive(false);
    //}
}