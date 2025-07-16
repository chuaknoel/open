public enum QuestProgressState
{
    NotStarted,
    InProgress,
    CanComplete,
    Completed
}

[System.Serializable]
public class Quest
{
    public QuestData questData;
    public QuestProgressState progressState;
    public int currentAmount;

    /// <summary>
    /// 퀘스트 상태를 초기화합니다.
    /// </summary>
    /// <param name="data"></param>
    public Quest(QuestData data)
    {
        questData = data;
        progressState = QuestProgressState.NotStarted;
        currentAmount = 0;
    }
    /// <summary>
    /// 퀘스트를 시작합니다.
    /// </summary>
    public void StartQuest()
    {
        if (progressState == QuestProgressState.NotStarted)
            progressState = QuestProgressState.InProgress;
    }
    /// <summary>
    /// 퀘스트 진행 상태를 업데이트합니다.
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateProgress(int amount)
    {
        if (progressState != QuestProgressState.InProgress) return;

        currentAmount += amount;

        if (currentAmount >= questData.Amount)
        {
            currentAmount = questData.Amount;
            progressState = QuestProgressState.CanComplete;
        }
    }
    /// <summary>
    /// 퀘스트를 완료합니다.
    /// </summary>
    public void CompleteQuest()
    {
        if (progressState == QuestProgressState.CanComplete)
            progressState = QuestProgressState.Completed;
    }

    public bool IsCompleted => progressState == QuestProgressState.Completed;
}