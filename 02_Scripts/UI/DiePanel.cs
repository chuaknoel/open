using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 죽으면 활성화되는 패널입니다.
/// </summary>
public class DiePanel : BaseUI
{
    public override UIType UIType => UIType.Panel;
    private SceneLoader loader;
    private PlayerStat stat;

    public void Init()
    {
        GameObject player = PlayManager.Instance.player.gameObject;  

        if (player != null && player.TryGetComponent<PlayerStat>(out stat))
        {
            EventRelease(); // 나중에 옮기기~~~~~~~~
            EventRegistration();
        }
        else
        {
            Debug.LogWarning("PlayerStat 컴포넌트를 찾을 수 없습니다.");
        }
    }
    void Update()
    {
        if (stat.isDeath && Input.anyKeyDown && gameObject.activeInHierarchy)
        {
            loader.Load("StartScene");
        }
    }

    /// <summary>
    /// UI 활성화시 호출
    /// </summary>
    public override void OpenUI()
    { 
        GameObject child = transform.GetChild(0).gameObject;

        child.SetActive(true);
        child.transform.SetAsLastSibling();
        loader = SceneLoader.Instance;
    }

    /// <summary>
    /// 이벤트 등록
    /// </summary>
    private void EventRegistration()
    {
        stat.OnDeath += OpenUI;
    }

    /// <summary>
    /// 이벤트 해제
    /// </summary>
    private void EventRelease()
    {
        stat.OnDeath -= OpenUI; 
    }
}
