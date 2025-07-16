using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] private QuestConditionChecker questConditionChecker;

    public int questId;

    private void Awake()
    {
        questConditionChecker = FindObjectOfType<QuestConditionChecker>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Logger.Log($"[LocationTrigger] {gameObject.name}에 플레이어가 진입: 퀘스트 ID {questId} 완료");
            questConditionChecker.TriggerEvent(questId, null);
            gameObject.SetActive(false);
        }
    }
}
