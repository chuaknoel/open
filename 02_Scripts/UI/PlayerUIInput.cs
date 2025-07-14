using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// UI의 InputSystem을 관리하는 스크립트입니다.
/// </summary>
public class PlayerUIInput : MonoBehaviour
{
    private UIManager uiManager;
    private UIStack uiStack;

    [SerializeField] private BaseWindow menuWindow;
    [SerializeField] private BaseWindow bookWindow;
    [SerializeField] private BaseWindow settingWindow;
    [SerializeField] private BaseWindow inventoryWindow;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        uiStack = GetComponent<UIStack>();
    }
    /// <summary>
    /// 인벤토리창 활성화
    /// </summary>
    /// <param name="context">키보드 입력값</param>
    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        uiManager.OpenWindow(inventoryWindow);
    }
    /// <summary>
    /// 메인 메뉴창 활성화
    /// </summary>
    /// <param name="context">키보드 입력값</param>
    public void OnOpenMenuWindow(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        // 스택에 열려 있는 다른 창이 있으면 그걸 먼저 닫음
        if (uiStack.uiStack.Count > 0)
        {
            BaseWindow window = uiStack.GetUI();

            if(window.UIType == UIType.Window)
            {
                window.CloseUI();
            }
            else if (window.UIType == UIType.Inventroy)
            {
                window.CloseInventroy();
            }
            return;
        }
        // 메뉴창 열려 있으면 닫고, 아니면 엶
        if (menuWindow.gameObject.activeSelf)
        {
            uiManager.CloseWindow(menuWindow);
        }
        else
        {
            uiManager.OpenWindow(menuWindow);
        }
    }
    /// <summary>
    /// Book Window 활성화
    /// </summary>
    public void OnOpenBookWindow()
    {
        uiManager.OpenWindow(bookWindow);
    }
}
