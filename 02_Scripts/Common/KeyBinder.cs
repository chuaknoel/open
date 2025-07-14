using Enums;
using TMPro;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeyBinder : MonoBehaviour
{
    public string actionName;     // 예: "Move"
    public int bindingIndex = 0;  // 복수 바인딩 시 사용

    public ActionKeyType actinoKeyType;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private PlayerInputs inputActions; //
    public InputAction action;         // PlayerInputs에서 actionName을 통해 찾은 Action
                                       //public InputBinding bind;        // Actino내부의 바인딩 정보 : 구조체라 자동 갱신이 안되서 주석처리
                                       // ex) PlayerInputs(inputActions) -> Player -> Move(action) -> Up(bind)
    //private Button keyBindButton;
    private Image keyBindImage;

    [SerializeField] private TextMeshProUGUI disPlayName;

    private bool isSelect;
    private bool bindStart;

    public UnityAction<KeyBinder> OnSelectHandler;
    public UnityAction<KeyBinder> DeSelectHandler;

    public void Init(PlayerInputs inputActions, string bindJson)
    {
        this.inputActions = inputActions;
        action = inputActions.asset.FindAction(actionName); //PlayerInputs 내부에 actionName으로 처리된 InputAction을 찾아서 할당

        LoadBind(bindJson);                                 //저장 정보 적용

        //keyBindButton ??= GetComponent<Button>();
        //keyBindButton.onClick.AddListener(StartRebind);
        //keyBindButton.onClick.AddListener(OnSelect);

        keyBindImage ??= GetComponent<Image>();
        EventTrigger trigger = keyBindImage.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) =>
        {
            PointerEventData pointerEventData = (PointerEventData)eventData;
            if (pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            StartRebind();
            Toggle();
        });
        //entry.callback.AddListener((eventData) => { StartRebind(); });
        //trigger.triggers.Add(entry);
        //entry.callback.AddListener((eventData) => { OnSelect(); });
        trigger.triggers.Add(entry);

        disPlayName.text = action.bindings[bindingIndex].ToDisplayString();

        isSelect = false;
    }

    public void LoadBind(string bindJson)
    {
        if (!string.IsNullOrEmpty(bindJson)) //저장 정보가 널이면 덮어씌울 데이터가 없기 때문에 PlayInputs의 기본 데이터가 저장됨으로 널이 아닐떄만 처리
        {
            inputActions.asset.LoadBindingOverridesFromJson(bindJson);
            Logger.Log("바인딩 블러오기");
        }
    }

    public void Toggle()
    {
        if (isSelect)
        {
            DeSelect();
        }
        else
        {
            OnSelect();
        }
    }

    public void OnSelect()
    {
        isSelect = true;
        OnSelectHandler?.Invoke(this);
    }

    public void DeSelect()
    {
        isSelect = false;
        rebindingOperation.Cancel();
    }

    public void StartRebind()
    {
        if (isSelect) return;

        bindStart = true;
        inputActions.Disable();                                                 //바인딩 진행시 항상 인풋을 Disable해준다.
        Logger.Log("바인딩 함수 진입");
        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)   //actionName으로 찾은 InputAction에서 bindingIndex를 통해 변경될 InputBinding을 찾는다.
            .WithControlsExcluding("<Mouse>")                                   //마우스 우클릭은 키 할당에서 제외한다.
            //.WithCancelingThrough("<Mouse>/leftButton")                        //마우스 좌클릭은 재선택한 것이기 때문에 슬롯 해제와 함께 바인딩 취소처리를 한다.
            .OnMatchWaitForAnother(0.1f)                                        //한번에 여러 데이터를 입력받지 못하게 대기 시간을 걸어준다.
            .OnPotentialMatch(operation =>                                      //예외처리 키(ESC등)를 설정하여 바인딩을 취소한다.
            {
                var control = operation.selectedControl;
                Logger.Log(control.path);
                if (control.path == "/Keyboard/escape")
                {
                    operation.Cancel(); // 수동 취소
                }
            })
            .OnCancel(operation => RebindCancel())
            .OnComplete(operation =>
            {
                bindStart = false;
                var selectedControl = operation.selectedControl;                //취소없이 바인딩 성공시 바인딩 한 키를 찾는다.
                InputManager.Instance.CheckBind(this);                          //자신이 새로 할당키에 중복 및 충돌처리를 위한 CheckBind를 실행한다.
                rebindingOperation.Dispose();                                   //바인딩이 끝나면 rebindingOperation을 해제하여 메모리 누수를 관리한다.
                UpdateDisPlayName(action.bindings[bindingIndex].ToDisplayString());                 //
                DeSelectHandler?.Invoke(this);
                inputActions.Enable();                                          //Disable된 인풋을 다시 활성화해준다.
            })
            .Start();
    }

    public void RebindCancel()
    {
        if (!bindStart) return;
        bindStart = false;
        Logger.Log("바인딩 취소");
        DeSelectHandler?.Invoke(this);
        rebindingOperation.Dispose();
        inputActions.Enable();
    }

    public InputBinding GetBinding()
    {
        return action.bindings[bindingIndex];
    }

    public void UpdateDisPlayName(string disPlayName)
    {
        this.disPlayName.text = disPlayName;
    }

    private void OnDestroy()
    {
        OnSelectHandler = null;
        DeSelectHandler = null;
    }
}
