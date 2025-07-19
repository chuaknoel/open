using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

/// <summary>
/// U를 관리하는 매니저 스크립트입니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private QuickSlotManager quickSlotManager;
    [SerializeField] private SkillQuickSlotManager skillQuickSlotManager;
    [SerializeField] private ShowBook showBook;

    public static UIManager Instance;
    public Canvas canvas;
    private UIStack uiStack;

    public BaseWindow destroyItemWindow;

    private void Awake()
    {
        Instance = this;

        Resources.Load<TMP_FontAsset>("Fonts/NEXON_Warhaven_Regular SDF");
        uiStack = GetComponent<UIStack>();
    }
    public void Init()
    {
        quickSlotManager.Init(inventory, uiInventory);
        skillQuickSlotManager.Init();
        showBook.Init();
        inventory.Init();
    }
    /// <summary>
    /// Window가 활성화 될 때 사용되는 함수입니다.
    /// </summary>
    /// <param name="window">??뽮쉐?酉釉?筌?/param>
    public void OpenWindow(BaseWindow window)
    {
        if (window.UIType == UIType.ParentWindow)
        {
            window.GetComponent<BaseWindow>().OpenParentWindow(); 
        }
        else
        {
            window.GetComponent<BaseWindow>().OpenUI();
        }
        uiStack.StackUI(window);
    }
    /// <summary>
    ///  Window가 비활성화 될 때 사용되는 함수입니다.
    /// </summary>
    /// <param name="window">??쑵??源딆넅??筌?/param>
    public void CloseWindow(BaseWindow window)
    {
        if (window.UIType == UIType.ParentWindow)
        {
            window.GetComponent<BaseWindow>().CloseParentWindow();
        }
        else
        {
            window.GetComponent<BaseWindow>().CloseUI();
        }       
        uiStack.GetUI();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}