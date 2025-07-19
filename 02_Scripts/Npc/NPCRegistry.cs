// NPCRegistry.cs (새 스크립트 파일)

using System.Collections.Generic;
using UnityEngine;

public class NPCRegistry : MonoBehaviour
{
    public static NPCRegistry Instance { get; private set; }

    //private Dictionary<string, NPC> registeredNPCs = new Dictionary<string, NPC>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// NPC가 자신을 등록소에 등록합니다.
    /// </summary>
    //public void Register(string id, NPC npc)
    //{
    //    if (!registeredNPCs.ContainsKey(id))
    //    {
    //        registeredNPCs.Add(id, npc);
    //    }
    //}

    /// <summary>
    /// NPC가 파괴될 때 등록소에서 제거합니다.
    /// </summary>
  //  public void Unregister(string id)
    //{
    //    if (registeredNPCs.ContainsKey(id))
    //    {
    //        registeredNPCs.Remove(id);
    //    }
    //}

    /// <summary>
    /// ID를 통해 씬에 있는 NPC를 찾습니다.
    /// </summary>
    //public NPC GetNPC(string id)
    //{
    //    registeredNPCs.TryGetValue(id, out NPC npc);
    //    return npc;
    //}
}