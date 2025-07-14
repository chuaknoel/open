using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

/// <summary>
/// LoadGame Window를 활성화하는 스크립트입니다.
/// </summary>
public class LoadGameWindow : BaseWindow
{
    public override UIType UIType => UIType.Window;

    [SerializeField] private UnityEngine.UI.Button removeButton;
    [SerializeField] private UnityEngine.UI.Button loadButton;

    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();
        removeButton.onClick.AddListener(RemoveGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    /// <summary>
    /// 화면을 비활성화하는 스크립트입니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 게임을 로드합니다.
    /// </summary>
    private void LoadGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    /// <summary>
    /// 저장한 게임 내역을 제거합니다.
    /// </summary>
    private void RemoveGame()
    {
        Logger.Log("저장된 데이터 제거");
    }
}
