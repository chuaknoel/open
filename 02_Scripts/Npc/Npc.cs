using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInputs playerInput;
    [SerializeField] private DialogueInputs dialogueInput;
    [SerializeField] private NpcData npcData;
    public NpcData NpcData => npcData;
    [SerializeField] private Quest[] subQuest;
    public Quest[] SubQuests => subQuest;
    [SerializeField] private Material outlineMaterial;
    private Renderer npcRenderer;
    private Material normalMaterial;
    public static event Action<Npc> OnNpcInteracted;

    public bool IsInteractable => true;

    public void Init()
    {
        npcRenderer = GetComponent<Renderer>();
        normalMaterial = npcRenderer.material;
        playerInput = InputManager.Instance.inputActions; // PlayerInput 초기화
        dialogueInput = InputManager.Instance.dialogueInputs; // DialogueInput 초기화
    }

    public void Unload()
    {
        OnNpcInteracted = null; // 이벤트 구독 해제
        npcRenderer.material = normalMaterial; // NPC 재질 초기화
    }

    public void OnInteraction()
    {
        playerInput.Disable(); // PlayerInput 비활성화
        dialogueInput.Enable(); // DialogueInput 활성화
        OnNpcInteracted?.Invoke(this);
    }

    public void SetInterface(bool active) // NPC가 Player의 범위안에 들어갔을 때 효과처리
    {
        if (active)
        {
            npcRenderer.material = outlineMaterial;
        }
        else
        {
            npcRenderer.material = normalMaterial;
        }
    }

    public NpcData Data
    {
        get => npcData;
        set => npcData = value;
    }

    public event Action<IInteractable> OnInteracted;
}