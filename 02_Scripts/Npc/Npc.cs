using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class NPC : MonoBehaviour
{
    [Header("기본 정보")]
    [Tooltip("NPCs.csv에 정의된 고유 ID")]
    [SerializeField] private string npcID;
    [SerializeField] private bool isMerchant;

    [Header("퀘스트 정보")]
    [Tooltip("이 NPC가 제공할 퀘스트가 있는지 여부")]
    [SerializeField] private bool hasQuest;
    [Tooltip("제공할 퀘스트의 ID (QuestData에 정의된 ID)")]
    [SerializeField] private string questIDToGive;

    [Header("대화 설정 (인스펙터)")]
    [Tooltip("선택지가 나타나기 전에 출력될 대화")]
    [SerializeField] private List<DialogueLine> preChoiceDialogue;

    [Tooltip("상점 선택 후 출력될 대화")]
    [SerializeField] private List<DialogueLine> postShopDialogue;

    [Tooltip("퀘스트 수락 후 출력될 대화")]
    [SerializeField] private List<DialogueLine> postQuestAcceptDialogue;

    [Tooltip("일반 대화 선택 후 출력될 대화")]
    [SerializeField] private List<DialogueLine> generalDialogue;

    [Header("상호작용 설정")]
    [SerializeField] private float interactionRange = 5f;

    public NPCData Data { get; private set; }
    public bool IsMerchant => isMerchant;
    public bool HasQuest => hasQuest;
    public string QuestIDToGive => questIDToGive;

    // 대화 리스트들을 외부에서 접근할 수 있도록 프로퍼티 추가
    public List<DialogueLine> PostShopDialogue => postShopDialogue;
    public List<DialogueLine> PostQuestAcceptDialogue => postQuestAcceptDialogue;
    public List<DialogueLine> GeneralDialogue => generalDialogue;

    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        if (DataManager.Instance != null && DataManager.Instance.NPCDB.TryGetValue(npcID, out NPCData a_data))
        {
            Data = a_data;
            isMerchant = Data.IsMerchant;
        }

        NPCRegistry.Instance?.Register(npcID, this);
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (Vector3.Distance(transform.position, playerTransform.position) <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }
    }

    // 상호작용 시작
    private void Interact()
    {
        // 1. 선택지 이전 대화 시작
        DialogueManager.Instance.StartDialogue(preChoiceDialogue, () =>
        {
            // 2. 대화가 끝나면 선택지 UI 표시
            InteractionChoiceUI.Instance.ShowChoices(this);
        });
    }

    private void OnDestroy()
    {
        NPCRegistry.Instance?.Unregister(npcID);
    }
}