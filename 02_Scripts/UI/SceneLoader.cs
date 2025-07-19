using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    private string sceneName;
  
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 이동할 씬을 변경합니다.
    /// </summary>
    /// <param name="_sceneName"></param>
    public void ChangeSceneName(string _sceneName)
    {
        sceneName = _sceneName; 
    }

    /// <summary>
    /// 지정한 씬 이름을 반환합니다.
    /// </summary>
    public string ReturnSceneName()
    {
        return sceneName;
    }

    /// <summary>
    /// 로딩씬을 거치지 않고 지정한 씬으로 바로 이동합니다
    /// </summary>
    /// <param name="targetSceneName">이동할 씬 이름</param>
    public void Load(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName); 
    }
}
