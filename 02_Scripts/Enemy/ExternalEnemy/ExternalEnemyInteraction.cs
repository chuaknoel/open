using UnityEngine;

public class ExternalEnemyInteraction : MonoBehaviour
{
    private ExternalEnemy externalEnemy;
    private EnemyManager enemyManager;

    public void Start()
    {
        externalEnemy = GetComponent<ExternalEnemy>();
        enemyManager = EnemyManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & externalEnemy.targetMask) != 0)
        {
            //enemyManager.currentPlayerPosition = collision.transform.position;

            //enemyManager.StartBattleScene();
        }
    }
}
