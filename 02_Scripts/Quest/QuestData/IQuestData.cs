public enum QuestType
{
    Hunting, // 사냥 퀘스트 (몬스터 처치 등)
    Item, // 아이템 수집 퀘스트 (아이템 획득 등)
    Talk, // 대화 퀘스트 (NPC와 대화 등)
    Location, // 위치 탐색 퀘스트 (특정 장소 방문 등)
}

public interface IQuestData
{
    CompleteType CompleteType { get; } // 퀘스트 완료 타입 (자동, 기본 등)
    QuestType QuestType { get; } // 퀘스트 타입 (메인, 서브 등)  
    int QuestId { get; } // 퀘스트 ID (고유 식별자)  
    string Title { get; } // 퀘스트 제목  
    string Description { get; } // 퀘스트 설명  
    string TargetId { get; } // 퀘스트 대상 ID (예: 몬스터, 아이템 등)
    int Amount { get; } // 퀘스트 진행 수량  
    string ClearNpcID { get; } // 퀘스트 완료 후 대화할 NPC ID  

    string[] DialogueList { get; } // 대화 내용  
    string[] ClearDialogue { get; } // 퀘스트 완료 후 대화 내용  
    string InprogressDialogue { get; } // 퀘스트 진행 중 대화 내용
    string RefusalDialogue { get; } // 퀘스트 거절 대화 내용(메인퀘스트는 거절 불가)  

    RewardType RewardType { get; } // 보상 타입 (경험치, 아이템 등)
    string RewardItemId { get; } // 보상 아이템 ID  
    int RewardAmount { get; } // 보상  
}