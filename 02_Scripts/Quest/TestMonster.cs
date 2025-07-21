using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{
    public string monsterId;

    public void Die()
    {
        QuestEvents.MonsterKilled(monsterId);
    }
}
