using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInputs inputActions;                               //PlayerInput

    public List<KeyBinder> keyBinderList = new List<KeyBinder>();   //PlayerInputs에 등록된 Action 정보를 가지고 있는 Binder 리스트

    private KeyBinder currentBinder;                                //현재 선택된 keybinder

    private void Awake()
    {
        Instance = this;

    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Logger.Log("바인딩 리셋");
            ResetBind();

        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Logger.Log("바인딩 저장");
            SaveBindData();
        }
    }

    public void Init()
    {
        inputActions = new PlayerInputs();

        inputActions.Enable();

        string bindJson = PlayerPrefs.GetString("bindData", "");       //플레이어 프리팹에 저장되어 있는 바인드 정보 불러오기
        for (int i = 0; i < keyBinderList.Count; i++)
        {
            keyBinderList[i].Init(inputActions, bindJson);
            keyBinderList[i].OnSelectHandler += BinderSelectEvent;
            keyBinderList[i].DeSelectHandler += BinderDeselectEvent;
        }
    }

    public void ResetBind()                                 //바인드 정보 리셋
    {
        inputActions.Disable();                             //바인드 작업중엔 항상 Disable을 먼저 실행해야 오류를 방지할 수 있음
        inputActions.asset.RemoveAllBindingOverrides();     //바인드 정보를 모두 삭제
        inputActions.Enable();

        if (PlayerPrefs.HasKey("bindData"))
        {
            PlayerPrefs.DeleteKey("bindData");
            PlayerPrefs.Save();
        }
    }

    public void SaveBindData()                                              //바인드 정보 저장
    {
        string bindData = inputActions.asset.SaveBindingOverridesAsJson();  //변경된 바인드 정보를 제이슨으로 저장
        PlayerPrefs.SetString("bindData", bindData);                        //저장된 데이터는 플레이어프리팹에 저장
        PlayerPrefs.Save();
        Logger.Log("바인드 정보저장");
    }

    public void CheckBind(KeyBinder newBinder)                                                                 //바인드 정보를 변경할 때 중복이나 충돌처리를 위한 메서드
    {
        string newPath = newBinder.GetBinding().overridePath ?? newBinder.GetBinding().effectivePath;          //새로 바인드된 키가 어떤 키인지 체크 ex) attack이 x-> z로 변경되었다면 z키에 해당하는 경로

        foreach (KeyBinder keyBinder in keyBinderList)                                                         //현재 바인드 가능한 모든 키를 탐색하여 z가 할당된 데이터를 찾는다.
        {
            if (keyBinder == newBinder) continue;                                                              // 자기 자신은 패스한다.

            string existingPath = keyBinder.GetBinding().overridePath ?? keyBinder.GetBinding().effectivePath; //반복문을 통해 경로가 z인지 찾기 위해 기존 바인드 데이터의 경로를 찾아온다.

            if (existingPath == newPath)                                                                       //
            {
                keyBinder.action.ApplyBindingOverride(keyBinder.bindingIndex, string.Empty);                   //중복된 키를 찾았다면 기존에 있던 키는 바인드 정보를 삭제한다.
                keyBinder.UpdateDisPlayName(string.Empty);                                                     //ex) attack이 x키를 할당 받고 있다가 z키로 변경헀는데, 
                Logger.Log($"{keyBinder.actinoKeyType.ToString()} 은 중복되어 KeyBind가 해제 됩니다.");        //z에 dash가 있었다면 dash는 바인드 정보가 없어지고 z자리에 attack 할당된다. 
            }                                                                                                  //dash는 할당된 키가 없음으로 다시 할당해 주어야한다.
        }                                                                                   
    }

    public void BinderSelectEvent(KeyBinder selectBinder)          //바인드할 슬롯을 선택했을때 처리되는 이벤트
    {
        if(currentBinder == selectBinder)
        {
            currentBinder?.DeSelect();
            currentBinder = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Debug.Log("슬롯 교체");
            currentBinder?.DeSelect();
            currentBinder = selectBinder;

            ///////////  조하늘 추가
            if (Keyboard.current == null)
            {
                Debug.LogWarning("❌ Keyboard.current is null — Input System 설정 안 됐을 수 있음");
                return;
            }

            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                Debug.Log(key.wasPressedThisFrame);
                if (key.wasPressedThisFrame)
                {
                    Debug.Log($"누른 키: {key.displayName} ({key.name})");
                    break;
                }
            }
        }
    }

    public void BinderDeselectEvent(KeyBinder deselectBinder)      //선택된 바인드 슬롯이 해제 될때 처리되는 이벤트
    {
        deselectBinder?.DeSelect();

        if(currentBinder == deselectBinder)
        {
            currentBinder = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnLoad()
    {
        Instance = null;
    }

    private void OnDestroy()
    {
        OnLoad();
    }
}


//public void CheckBind(KeyBinder newAction, InputControl bindingIndex)
//{
//    InputBinding newBind = newAction.bindings[bindingIndex];
//    string newPath = newBind.overridePath ?? newBind.effectivePath;

//    for (int i = 0; i < newAction.actionMap.bindings.Count; i++)
//    {
//        InputBinding oldBind = newAction.actionMap.bindings[i];

//        if (oldBind.action == newBind.action) continue;                     //자기 자신은 체크하지 않는다.

//        string oldBindPath = oldBind.overridePath ?? oldBind.effectivePath; //오버라이드 정보 유무에 따라 기존 패스를 줄지 오버라이드 패스를 줄지 정한다.

//        if (oldBindPath == newPath)
//        {
//            newAction.actionMap.FindAction(oldBind.action);
//        }
//    }

//    foreach (InputBinding oldBind in newAction.actionMap.bindings)
//    {
//        if (oldBind.action == newBind.action) continue;

//        string oldBindPath = oldBind.overridePath ?? oldBind.effectivePath;

//        if (oldBindPath == newPath)
//        {

//        }
//    }
//}