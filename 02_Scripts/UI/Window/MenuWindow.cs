using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// 매인 메뉴를 구성하는 스크립트입니다.
/// </summary>
public class MenuWindow : BaseWindow
{
    public UIManager uIManager;

    [SerializeField] private TMP_Text recentItemText;
    [SerializeField] private TMP_Text recentQuestText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button settingButton;
    public BaseWindow settingWindow;

    public override UIType UIType => UIType.SelfWindow;

    public void Init() 
    {
        InputManager.Instance.inputActions.Player.Menu.started += MenuOpen;
    }

    public void MenuOpen(InputAction.CallbackContext context)
    {
        OpenUI();
    }

    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();
        SetText(TestData());
        settingButton.onClick.AddListener(SetButton);
    }
    /// <summary>
    /// 화면을 비활성화하는 스크립트입니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 테스트 DB입니다.
    /// </summary>
    /// <returns></returns>
    private RecentDataParam TestData()
    {
        string[] testa = { "아이템a", "아이템b" };
        string[] testb = { "퀘스트a", "퀘스트b" };
        string testLevel = "7";
        RecentDataParam recentDataParam = new RecentDataParam(testa, testb, testLevel);
        return recentDataParam;
    }

    /// <summary>
    /// DB를 화면에 표시해주는 기능입니다.
    /// </summary>
    /// <param name="param">Player DB</param>
    private void SetText(OpenParam param)
    {
        var p = param as RecentDataParam;

        if (p != null) 
        {
            recentItemText.text = $"   최근 발견\r\n- {p.recentItems[0]}\r\n- {p.recentItems[1]}";
            recentQuestText.text = $"   긴급 상황\r\n- {p.recentQuests[0]}\r\n- {p.recentQuests[1]}";
            levelText.text = $"현재 레벨 : {p.level}";
        }
    }

    /// <summary>
    /// 버튼에 Method를 연결해주는 기능입니다.
    /// </summary>
    private void SetButton()
    {
        uIManager.OpenWindow(settingWindow);
    }
}
