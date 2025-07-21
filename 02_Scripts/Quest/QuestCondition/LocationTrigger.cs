using UnityEngine;
using UnityEngine.Pool;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] private QuestConditionChecker questConditionChecker;
    private IObjectPool<LocationTrigger> locationTriggerPool;

    public int questId;

    public void Init(IObjectPool<LocationTrigger> locationtrigger)
    {
        questConditionChecker = FindObjectOfType<QuestConditionChecker>();
        locationTriggerPool = locationtrigger;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Logger.Log($"[LocationTrigger] {gameObject.name}에 플레이어가 진입: 퀘스트 ID {questId} 완료");
            questConditionChecker.TriggerEvent(questId, null);
            locationTriggerPool.Release(this); // 트리거 사용 후 풀에 반환
        }
    }
}