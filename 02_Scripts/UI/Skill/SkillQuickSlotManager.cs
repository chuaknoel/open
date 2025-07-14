using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillQuickSlotManager : MonoBehaviour
{
    public string actionName = "SkillButton";
    private PlayerInputs inputActions;
    private InputAction action;

    private int inputIndex;
   
    public SkillQuickSlot[] skillSlots = new SkillQuickSlot[8];

    private void Start()
    {
        Init();    
    }

    public void Init()
    {
        inputActions = InputManager.Instance.inputActions;
        action = inputActions.asset.FindAction(actionName);

        action.started += OnSkillQuickSlotPressed;

        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].Init(i);
        }
    }

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

    //public Dictionary<string, int> keyToSlotIndex = new()
    //{   
    //    { "q", 0 },
    //    { "w", 1 },
    //    { "e", 2 },
    //    { "r", 3 },
    //    { "a", 4 },
    //    { "s", 5 },
    //    { "d", 6 },
    //    { "f", 7 },
    //};
    //public void OnSkillQuickSlotPressed(InputAction.CallbackContext context)
    //{
    //    string key = context.control.name.ToLower();

    //    if (keyToSlotIndex.TryGetValue(key, out int slotIndex))
    //    {
    //        if (context.performed && skillSlots[slotIndex] != null)
    //        {
    //            skillSlots[slotIndex].UseSkill();
    //        }
    //    }   
    //}
}
