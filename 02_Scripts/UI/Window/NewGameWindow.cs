using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Analytics;

/// <summary>
/// 새로하기 창을 관리하는 스크립트입니다.
/// </summary>
public class NewGameWindow : BaseWindow
{
    public override UIType UIType => UIType.SelfWindow;

    [SerializeField] private UnityEngine.UI.Button startButton;
    
    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();

        startButton.onClick.AddListener(GameStart);
    }

    /// <summary>
    /// 화면을 비활성화하는 스크립트입니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 게임을 시작합니다.
    /// </summary>
    private void GameStart()
    {
        SceneLoader.Instance.Load("LoadingScene");
        Logger.Log("게임 시작");
    }
}
