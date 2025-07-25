using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
public class BookWindow : BaseWindow
{
    public override UIType UIType => UIType.SelfWindow;
   
    [SerializeField] private BookAnimation bookAnimation;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private GameObject[] windows = new GameObject[5];
    [SerializeField] private Button[] buttons = new Button[5];
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private string[] contents = new string[5]; // 버튼별 텍스트 내용
    [SerializeField] private Tooltip toolTip;
    [SerializeField] private bool[] isTextVisible = new bool[5];                     // 글자 보임 여부 저장

    private SkillTempSlotManager skillTempSlotManager;
    private Coroutine currentTapCoroutine;
    private int currentTapIndex = 0;


    /// <summary>
    /// 초기화 함수입니다.
    /// </summary>
    public void Init()
    { 
        UIManager uiManager = UIManager.Instance;
         skillTempSlotManager = uiManager.skillTempSlotManager;

        // 버튼 연결
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ShowTap(index));
        }
        // 처음에는 타이틀 텍스트 모두 보이도록 초기화
        for (int i = 0; i < isTextVisible.Length; i++)
        {
            isTextVisible[i] = true;
        }

        isTextVisible[0] = true;

        windows[0].GetComponent<EquipmentWindow>().Init(); // 장비창 초기화
        windows[1].GetComponent<SkillWindow>().Init(); // Skill창 초기화            
        windows[2].GetComponent<DeliverQuestsData>().Init(); // 퀘스트창 초기화   
        windows[3].GetComponent<DeliverCompanionsData>().Init(); // 동료창 초기화
        windows[4].GetComponent<DeliverCollectionData>().Init(); // 도감창 초기화        
    }
    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();

          ShowTap(0);
    }

    /// <summary>
    /// 화면을 비활성화하는 함수입니다.
    /// </summary>
    public override void CloseUI()
    {
        StopClickTapButtonCoroutine();
        toolTip.CloseUI();
        skillTempSlotManager.DestroySkillTempSlot();
        base.CloseUI(); 
    }
    /// <summary>
    /// 닫기 버튼 이벤트 연결
    /// </summary>
    /// <param name="action"></param>
    protected override void AddCloseButtonListener(UnityEngine.Events.UnityAction action)
    {
        if (closeButton != null)
        {
            toolTip.CloseUI();
            closeButton.onClick.AddListener(action);
        }
    }
    /// <summary>
    /// 모든 Tap을 비활성화한 뒤, 보여주고자 하는 Tap만 보여줍니다.
    /// </summary>
    /// <param name="index">// 창의 인덱스</param>
    private void ShowTap(int index)
    {
        if (!gameObject.activeInHierarchy)
            return; // 비활성화 상태에서는 무시

        CloseAll();
        bookAnimation.BookOpen();

        currentTapIndex = index;

        StartClickTapButtonCoroutine(index);

        skillTempSlotManager.DestroySkillTempSlot();

    }

    /// <summary>
    /// Tap 버튼 클릭시 해당 Tap이 활성화 됩니다.
    /// </summary>
    /// <param name="index"></param>
    IEnumerator ClickTapButton(int index)
    {
        yield return new WaitForSeconds(0.5f);

        windows[index].SetActive(true);
        buttons[index].transform.GetChild(1).gameObject.SetActive(true);
        ShowTitleText(index);

        currentTapCoroutine = null;
    }

    /// <summary>
    /// ClickTapButton Coroutine을 실행시킵니다.
    /// </summary>
    /// <param name="index"></param>
    private void StartClickTapButtonCoroutine(int index)
    {
        StopClickTapButtonCoroutine();
        currentTapCoroutine = StartCoroutine(ClickTapButton(index));
    }

    /// <summary>
    ///  ClickTapButton Coroutine을 정지시킵니다.
    /// </summary>
    private void StopClickTapButtonCoroutine()
    {
        if (currentTapCoroutine != null)
        {
            StopCoroutine(currentTapCoroutine);
            currentTapCoroutine = null;
        }
    }

    /// <summary>
    /// 모든 Tap을 비활성화합니다.
    /// </summary>
    private void CloseAll()
    {
        titleText.gameObject.SetActive(false);
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
            buttons[i].transform.GetChild(0).gameObject.SetActive(true);
            buttons[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 타이틀 글자를 보이게 하는 함수입니다.
    /// </summary>
    /// <param name="index"></param>
    private void ShowTitleText(int index)
    {
        titleText.text = contents[index];
        titleText.gameObject.SetActive(isTextVisible[index]);

        if (index == 0 || index == 1)
        {
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -269f);
        }
        titleText.text = contents[index];
            
    }

    /// <summary>
    /// 타이틀 글자를 안 보이게 하는 함수입니다.
    /// </summary>
    public void HideTitleText()
    {
        isTextVisible[currentTapIndex] = false;
        titleText.gameObject.SetActive(false);
    }
}
