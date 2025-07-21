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

    private void Start()
    {
        npcRenderer = GetComponent<Renderer>();
        normalMaterial = npcRenderer.material;
        playerInput = new PlayerInputs();
    }

    public void OnInteraction()
    {
        InputManager.Instance.inputActions.Disable(); // InputActions 비활성화
        if (dialogueInput != null)
        {
            dialogueInput.enabled = true; // DialogueInputs 활성화
        }

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