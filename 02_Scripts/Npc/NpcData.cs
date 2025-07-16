using UnityEngine;

public enum NpcType
{
    Normal,
    Shop,
    Enhance
}

[CreateAssetMenu(menuName = "NPC/NpcData")]
public class NpcData : ScriptableObject
{
    public string npcId; // NPC ID (고유 식별자)
    public string npcName; // NPC 이름
    public NpcType npcType; // NPC 타입 (일반, 상점, 강화 등)
    public string greeting; // NPC 인사말
    public string[] dialogueList; // NPC 대화 목록
}