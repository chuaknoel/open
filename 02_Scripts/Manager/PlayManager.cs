using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    private void Awake()
    {
        instance = this;    
    }

    public InputManager inputmanager;
    public Player player;
    public EquipmentManager equipmentManager;

    public void Init()
    {
        inputmanager ??= InputManager.Instance;
        player = FindObjectOfType<Player>();
        
        inputmanager.Init();
        player.Init();
    }

    public void UnLoad()
    {
        instance = null;
    }
}
