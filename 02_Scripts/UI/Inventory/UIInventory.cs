using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : BaseWindow
{   
    // 검색
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private List<ItemGradeColorPair> colorList;

    // 카테고리 버튼과 버튼별 아이템 타입
    [SerializeField] private List<Button> categoryButtons;
    [SerializeField] private List<ItemType> categoryTypes;

    // 필터
    [SerializeField] private TMP_Dropdown sortDropdown;

    [SerializeField] private Tooltip toolTip;

    public GameObject destroyItemWindow;
    protected Inventory inventory;
    private ItemFactory itemFactory;
    public override UIType UIType => UIType.ParentWindow;
    public int slotCount = 20;
    private bool isInitialized = false;

    private ItemFilter itemFilter = new ItemFilter();
    private ItemSort itemSort = new ItemSort();

    public GameObject uiSlotPrefab;
    public Transform slotsParent;

    public virtual async Task Init()
    {
        if (!isInitialized)
        {
            itemFactory = GetComponent<ItemFactory>();
            inventory = GetComponent<Inventory>();

            // 레벨업 시 슬롯 동적으로 생성하는 코드는 추후에 레벨업 기능 완성되면 이벤트로 처리하기
            await inventory.CreateSlots();  // 슬롯 동적 생성
            itemFactory.AddItemData();
            inventory.SetInventory(); // 인벤토리 초기 설정

            searchInput.onValueChanged.AddListener(OnSearch);
            sortDropdown.onValueChanged.AddListener(SortItem);

            OnCategorySelected(); // 카테고리별 아이템 분류 버튼 연결
            isInitialized = true;

            InputManager.Instance.inputActions.Player.Inventory.started += OpenInventory;
        }
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        OpenParentWindow();
    }

    public override void OpenParentWindow()
    {
        base.OpenParentWindow();
        transform.SetAsLastSibling();
    }
    public override void CloseParentWindow()
    {
        toolTip.CloseUI();
        base.CloseParentWindow();
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

    // UI 업데이트
    public void UpdateSlot(Slot slot)
    {
        slot.UpdateSlot();
    }
   
    // 검색
    public void OnSearch(string keyword)
    {
        List<Item> searchItems = itemFilter.FilterByKeyword(inventory.items, keyword);
        inventory.ClearInventory();
        AddItemInInventory(searchItems, false);
    }
    // 카테고리 선택
    private void OnCategorySelected()
    {
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            int index = i; 
            categoryButtons[i].onClick.AddListener(() =>
            {   
                if(index == 0)
                {
                    // 전체 인벤토리 아이템들 보여주기
                    inventory.ClearInventory();
                    AddItemInInventory(inventory.items, true);
                }
                else
                {
                    List<Item> searchItems = itemFilter.FilterByCategory(inventory.items, categoryTypes[index]);
                    inventory.ClearInventory();
                    AddItemInInventory(searchItems, false);
                }              
            });
        }
    }
    // 아이템 정렬
    private void SortItem(int index)
    {
        inventory.ClearInventory();

        string selectedText = sortDropdown.options[index].text;
        switch (selectedText)
        {
            case "카테고리별 정렬":                
                AddItemInInventory(inventory.items, true); // 정렬 풀기
                break;
            case "등급별":

                // 장비 아이템만 뽑기
                List<Item> equipmentItems = inventory.items
                .Where(item => item.Type == ItemType.Equip)
                .ToList();

                // 장비 아이템이 아닌 아이템만 뽑기
                List<Item> nonEquipItems = inventory.items
                .Where(item => item.Type != ItemType.Equip)
                .ToList();

                // 장비 아이템만 등급별로 정렬하기
                List<Item> result = new List<Item>();
                List<EquipItem> equipmentList = equipmentItems.OfType<EquipItem>().ToList();
                result.AddRange(itemSort.SortByItemGrade(equipmentList));
                result.AddRange(nonEquipItems);

                AddItemInInventory(result, false);

                break;
            case "이름순":
                AddItemInInventory(itemSort.SortByItemName(inventory.items), false);
             
                break;
        }
    }
    // 인벤토리에 아이템 넣기
    private void AddItemInInventory(List<Item> searchItems, bool useInventoryIndex = false)
    {
        foreach (Item item in searchItems)
        {
            if(useInventoryIndex)
            {
                inventory.AddItemAtIndex(item);
            }
            else
            {
                // inventory.AddItem(item, useInventoryIndex);
                inventory.AddItem(item);
            }
        }
    }
}
