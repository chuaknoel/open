using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private List<Npc> npcList = new List<Npc>();

    public void Init()
    {
        foreach (Npc npc in npcList)
        {
            npc.Init();
        }
    }

    public void UnLoad()
    {
        foreach (Npc npc in npcList)
        {
            npc.Unload();
        }
    }
}
