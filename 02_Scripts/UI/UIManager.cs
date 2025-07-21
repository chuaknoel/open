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
    public ShowStatus showStatus { get; private set; }
    public Inventory inventory { get; private set; }
    public UIInventory uiInventory { get; private set; }
    public EquipmentManager equipmentManager { get; private set; }
    public QuickSlotManager quickSlotManager { get; private set; }
    public SkillQuickSlotManager skillQuickSlotManager { get; private set; }
    public SkillTempSlotManager skillTempSlotManager { get; private set; }
    public MenuWindow menuWindow { get; private set; }
    public ShowBook showBook { get; private set; }
    public BookWindow bookWindow { get; private set; }

    public static UIManager Instance;
    public DestroyWindow destroyItemWindow;
    [HideInInspector] public RectTransform rect;
    private UIStack uiStack;

    private void Awake()
    {
        //인스턴스가 null이 아니라면 중복된 인스턴스가 있거나, 메모리가 제대로 해제되지 않았음을 뜻함
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Resources.Load<TMP_FontAsset>("Fonts/NEXON_Warhaven_Regular SDF");
    }

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        uiStack = GetComponent<UIStack>();

        showStatus = GetComponentInChildren<ShowStatus>(true);
        inventory = GetComponentInChildren<Inventory>(true);
        uiInventory = GetComponentInChildren<UIInventory>(true);
        equipmentManager = GetComponentInChildren<EquipmentManager>(true);
        quickSlotManager = GetComponentInChildren<QuickSlotManager>(true);
        skillQuickSlotManager = GetComponentInChildren<SkillQuickSlotManager>(true);
        skillTempSlotManager = GetComponentInChildren<SkillTempSlotManager>(true);
        menuWindow = GetComponentInChildren<MenuWindow>(true);
        showBook = GetComponentInChildren<ShowBook>(true);
        bookWindow = GetComponentInChildren<BookWindow>(true);
        inventory = GetComponentInChildren<Inventory>(true);
        destroyItemWindow = GetComponentInChildren<DestroyWindow>(true);

        quickSlotManager.Init(inventory, uiInventory);
        skillTempSlotManager.Init(skillQuickSlotManager);
        skillQuickSlotManager.Init(skillTempSlotManager);
        menuWindow.Init();
        showBook.Init();
        bookWindow.Init(skillTempSlotManager);
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

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}