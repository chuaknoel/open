using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;
    public PartySystem playerParty;

    public List<PartySystem> enemySquads = new List<PartySystem>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
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

    private void OnDestroy()
    {
        Instance = null;
        playerParty = null;
    }
}
