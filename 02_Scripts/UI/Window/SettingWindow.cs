using UnityEngine.UI;
using UnityEngine;
using Enums;

/// <summary>
/// 설정창을 관리하는 스크립트입니다.
/// </summary>
public class SettingWindow : BaseWindow
{
    [SerializeField] private Button[] settingButtons; // 세팅 버튼들
    [SerializeField] private GameObject soundPanel; // 사운드 패널
    [SerializeField] private GameObject keyPanel; // 키 패널
    [SerializeField] private GameObject interfacePanel; // 인터페이스 패널
   
    public override UIType UIType => UIType.SelfWindow;
    private GameObject currentActivePanel;

    /// <summary>
    /// 설정창이 활성화될 때 실행됩니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();

        settingButtons[(int)SettingTab.Sound].onClick.AddListener(() => ShowPanel(soundPanel));
        settingButtons[(int)SettingTab.Key].onClick.AddListener(() => ShowPanel(keyPanel));
        settingButtons[(int)SettingTab.Interface].onClick.AddListener(() => ShowPanel(interfacePanel));

        ShowPanel(soundPanel); // 시작시 사운드 패널 열기
    }

    /// <summary>
    /// 설정창이 비활성화될 때 실행됩니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 패널이 활성화될 때 실행됩니다.
    /// </summary>
    /// <param name="panelToShow">열려 있는 패널</param>
    private void ShowPanel(GameObject panelToShow)
    {
        // 열려 있는 패널 닫고
        if (currentActivePanel != null) 
        { 
            currentActivePanel.SetActive(false);
        }
        // 새 패널 열기
        currentActivePanel = panelToShow;
        panelToShow.SetActive(true);
    }
}
