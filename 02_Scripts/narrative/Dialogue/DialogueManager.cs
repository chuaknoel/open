using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private NpcDialogueUI npcDialogueUI;
    private NpcDialogue npcDialogue;
    private SubQuestDialogue subQuestDialogue;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        npcDialogueUI = GetComponentInChildren<NpcDialogueUI>();
        npcDialogue = GetComponentInChildren<NpcDialogue>();
        subQuestDialogue = GetComponentInChildren<SubQuestDialogue>();
        npcDialogueUI?.Init();
        npcDialogue?.Init();
        subQuestDialogue?.Init();
    }

    public void UnLoad()
    {
        npcDialogueUI = null;
        npcDialogue = null;
        subQuestDialogue = null;
        Instance = null;
    }
}