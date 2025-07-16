using System;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] private NpcData npcData;
    public NpcData NpcData => npcData;
    [SerializeField] private Quest[] subQuest;
    public Quest[] SubQuests => subQuest;
    [SerializeField] private Material outlineMaterial;
    private Renderer npcRenderer;
    private Material normalMaterial;
    public static event Action<Npc> OnNpcInteracted;

    public bool IsInteractable => throw new NotImplementedException();

    private void Start()
    {
        npcRenderer = GetComponent<Renderer>();
        normalMaterial = npcRenderer.material;
    }

    public void OnInteraction()
    {
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