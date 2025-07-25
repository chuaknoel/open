using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// U를 관리하는 매니저 스크립트입니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    public ShowStatus showStatus { get; private set; }
   // public Inventory inventory { get; private set; }
  //  public UIInventory uiInventory { get; private set; }
    public EquipmentManager equipmentManager { get; private set; }
    public EquipmentWindow equipmentWindow { get; private set; }
    public CompanionEquipManager companionEquipManager { get; private set; }
    public CompanionWindow companionWindow { get; private set; }
    public QuickSlotManager quickSlotManager { get; private set; }
    public SkillQuickSlotManager skillQuickSlotManager { get; private set; }
    public SkillTempSlotManager skillTempSlotManager { get; private set; }
    public MenuWindow menuWindow { get; private set; }
    public ShowBook showBook { get; private set; }
    public BookWindow bookWindow { get; private set; }

    public Tooltip tooltip { get; private set; }

    public DialogueManager dialogueManager { get; private set; }

    [HideInInspector] public RectTransform rect;
    [HideInInspector] public DeliverCompanionsData deliverCompanionsData;

    public static UIManager Instance;
    public DestroyWindow destroyItemWindow;  
    private UIStack uiStack;
    public Image dragitemImage;
    public List<Inventory> inventorys = new List<Inventory>();

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
        equipmentManager = GetComponentInChildren<EquipmentManager>(true);
        equipmentWindow = GetComponentInChildren<EquipmentWindow>(true);

        //  inventory = GetComponentInChildren<Inventory>(true);
        inventorys.AddRange(GetComponentsInChildren<Inventory>(true));
        var uiInventorys = GetComponentsInChildren<UIInventory>(true);
        companionEquipManager = GetComponentInChildren<CompanionEquipManager>(true);
        companionWindow = GetComponentInChildren<CompanionWindow>(true);
        // uiInventory = GetComponentInChildren<UIInventory>(true);
        quickSlotManager = GetComponentInChildren<QuickSlotManager>(true);
        skillQuickSlotManager = GetComponentInChildren<SkillQuickSlotManager>(true);
        skillTempSlotManager = GetComponentInChildren<SkillTempSlotManager>(true);
        menuWindow = GetComponentInChildren<MenuWindow>(true);
        showBook = GetComponentInChildren<ShowBook>(true);
        bookWindow = GetComponentInChildren<BookWindow>(true);
      //  inventory = GetComponentInChildren<Inventory>(true);
        destroyItemWindow = GetComponentInChildren<DestroyWindow>(true);

        tooltip = GetComponentInChildren<Tooltip>(true);
        dragitemImage = GameObject.Find("DragImage").GetComponent<Image>();

        dialogueManager = GetComponentInChildren<DialogueManager>(true);
        dragitemImage = GameObject.Find("DragImage").GetComponent<Image>();

        deliverCompanionsData = GetComponentInChildren<DeliverCompanionsData>(true);

        equipmentManager.Init();
        quickSlotManager.Init();
        companionEquipManager?.Init();
        companionWindow.Init();
        skillTempSlotManager.Init();
        skillQuickSlotManager.Init();
        menuWindow.Init();
        showBook.Init();

        bookWindow.Init();
        dialogueManager?.Init();
        for (int i = 0; i < inventorys.Count; i++)
        {
            inventorys[i].Init();
            uiInventorys[i].Init();
        }
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