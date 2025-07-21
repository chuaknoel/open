using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }
    public PartySystem playerParty;

    public List<PartySystem> enemySquads = new List<PartySystem>();

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void Init(Player player)
    {
        CreatePlayerParty(player);
    }

    public void CreatePlayerParty(BaseCreature partyLeader)
    {
        playerParty = new PartySystem(partyLeader);
    }

    public void SetEnemySquad(BaseCreature bossEnemy, List<BaseCreature> squad)
    {
        PartySystem enemySquad = new PartySystem(bossEnemy);
        for (int i = 0; squad.Count > i; i++)
        {
            enemySquad.AddMember(squad[i]);
        }

        enemySquads.Add(enemySquad);
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            //playerParty는 게임 전체에서 유지되는 데이터이므로 해제하지 않음
            enemySquads.Clear();
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}
