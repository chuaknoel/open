using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        //if(Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public SkillManager skillManager;
 

    public PlayManager playManager;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        skillManager ??= SkillManager.instance;
        
        playManager ??= PlayManager.instance;

        skillManager.Init();

        playManager.Init();
    }


    public void UnLoad()
    {
        playManager.UnLoad();
        playManager = null;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
