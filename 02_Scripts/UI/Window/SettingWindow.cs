using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class SettingWindow : BaseWindow
{
    private enum SettingTab
    {
        Sound,
        Key,
        Interface
    }
    [SerializeField] private Button[] settingButtons; // 세팅 버튼들

    [SerializeField] private GameObject soundPanel; // 사운드 패널
    [SerializeField] private GameObject keyPanel; // 키 패널
    [SerializeField] private GameObject interfacePanel; // 인터페이스 패널

    private GameObject currentActivePanel;

    public override UIType UIType => UIType.Window;

    public override void OpenUI()
    {
        base.OpenUI();

        settingButtons[(int)SettingTab.Sound].onClick.AddListener(() => ShowPanel(soundPanel));
        settingButtons[(int)SettingTab.Key].onClick.AddListener(() => ShowPanel(keyPanel));
        settingButtons[(int)SettingTab.Interface].onClick.AddListener(() => ShowPanel(interfacePanel));

        ShowPanel(soundPanel); // 시작시 사운드 패널 열기
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }
    // 패널 활성화
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
