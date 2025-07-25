using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInputs inputActions;  //PlayerInput
    public DialogueInputs dialogueInputs; //DialogueInput

    private void Awake()
    {
        //인스턴스가 null이 아니라면 중복된 인스턴스가 있거나, 메모리가 제대로 해제되지 않았음을 뜻함
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init()
    {
        inputActions ??= new PlayerInputs();
        dialogueInputs ??= new DialogueInputs();
        inputActions.Enable();
        dialogueInputs.Disable();
    }

    public void UnLoad()
    {
        //언로드 요청이 들어왔을때 UnLoad가 제대로 되었는지 확인하는 코드

        //Instance가 자기 자신 : 정상처리
        if (Instance == this) 
        {
            inputActions = null;
            dialogueInputs = null;
            Instance = null;
        }
        //Instance 없음 : 초기화되지 않았거나, 다른곳이미 이미 해제되어 있는 상태. 확인 필요
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        //Instance가 자기 자신이 아님 : 인스턴스 초기화 과정에서 중복처리가 제대로 이루어지지 않음.
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }

    private void OnDestroy()
    {
        UnLoad();
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