using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

using UnityEngine.UI;
public class ExitWindow : BaseWindow
{
    public override UIType UIType => UIType.Window;

    [SerializeField] private Button okButton;

    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();

        okButton.onClick.AddListener(EndStart);
    }

    /// <summary>
    /// 화면을 비활성화하는 스크립트입니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 게임을 종료합니다.
    /// </summary>
    private void EndStart()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
