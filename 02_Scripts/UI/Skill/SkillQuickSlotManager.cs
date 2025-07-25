using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

/// <summary>
/// 스킬 퀵 슬롯 들을 관리하는 매니저 스크립트입니다.
/// </summary>
public class SkillQuickSlotManager : MonoBehaviour
{
    private PlayerInputs inputActions;

    public string actionName = "SkillButton";
    [SerializeField] private InputAction action;
    [SerializeField] private KeyBinder[] keyBinder = new KeyBinder[4];
   
    public SkillQuickSlot[] skillSlots = new SkillQuickSlot[8];
    public TextMeshProUGUI[] playerSkillSlotText = new TextMeshProUGUI[4];

    private SkillTempSlotManager skillTempSlotManager;
    private int inputIndex;

    /// <summary>
    /// 초기화 함수입니다.
    /// </summary>
    public void Init()
    {
        inputActions = InputManager.Instance.inputActions;
        action = inputActions.asset.FindAction(actionName);
        action.started += OnSkillQuickSlotPressed;

        UIManager uiManager = UIManager.Instance;
        skillTempSlotManager = uiManager.skillTempSlotManager;

        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].Init(i);
        }

        for (int i = 0; i < keyBinder.Length; i++)
        {
            keyBinder[i].OnCompleteRebind += ChangeSkillText;
        }
    }
    private void ChangeSkillText(KeyBinder _keyBinder)
    {
        if (_keyBinder.actionName != "SkillButton") { return; }

        for (int i = 0; i < keyBinder.Length; i++)
        {
            playerSkillSlotText[i].text = keyBinder[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
        skillTempSlotManager.ChangeTempSkillSlotText();
    }
    /// <summary>
    /// 스킬 퀵 슬롯들을 키보드로 작동할 시 발생하는 함수입니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnSkillQuickSlotPressed(InputAction.CallbackContext context)
    {
        //현재 Skills 액션 바인딩:
        //0: <Keyboard>/a
        //1: <Keyboard>/s
        //2: <Keyboard>/d
        //3: <Keyboard>/f
        //4: <Keyboard>/q
        //5: <Keyboard>/w
        //6: <Keyboard>/e
        //7: <Keyboard>/r

        // context.control: 현재 눌린 입력 키 (예: 'a')

        // action.GetBindingIndexForControl(context.control):
        // -> Skills 액션의 바인딩 목록 중, 이 키가 몇 번째인지 인덱스를 반환
        // -> 예: 'a'를 누르면 0, 'r'을 누르면 7
        inputIndex = action.GetBindingIndexForControl(context.control);

        if (inputIndex >= 0 && inputIndex < skillSlots.Length) //인풋이 변경되었을 경우 인풋과 슬롯의 갯수가 상이할 수 있다. 그런 경우 슬롯 범위내에서만 동작하로독 대비하는 방어 코드
        {
            skillSlots[inputIndex]?.UseSkill();
        }
    }
}
