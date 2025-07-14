using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;

using UnityEngine.UI;
using Unity.Services.Analytics;
public class Inventory : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private QuickSlotManager quickSlotManager;

    protected UIInventory uiInventory;
    private ItemColorHelper itemColorHelper;
    [SerializeField] private GameObject quickSlots;

     public  List<Slot> slots = new List<Slot>();
    public List<Item> items = new List<Item>();

    public int slotCount = 20;
    public GameObject uiSlotPrefab;
    public Transform slotsParent;
    public Image draggedImage;
    public Tooltip toolTip;

    private void OnEnable()
    {
        uiInventory = GetComponent<UIInventory>();
        itemColorHelper = GetComponent<ItemColorHelper>();
    }
    // 슬롯 동적 생성
    public virtual void CreateSlots()
    {
        // 초기화
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(uiSlotPrefab, slotsParent);

            Slot _slot = slot.GetComponent<Slot>();
            slots.Add(_slot);
            slots[i].slotIndex = i;
            slots[i].Init( this, equipmentManager, quickSlotManager);

            ItemDrag itemDrag = slot.GetComponent<ItemDrag>();
            itemDrag.Init(canvas, uiManager, this.gameObject, quickSlots, draggedImage);

            SlotEvent slotEvent = slot.GetComponent<SlotEvent>();
            slotEvent.Init(canvas, uiManager, this.gameObject, draggedImage);
        }
    }
    // 인벤토리 초기 설정
    public void SetInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            AddItem(items[i],true); // 아이템 넣기
        }
        for (int i = items.Count; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
            uiInventory.UpdateSlot(slots[i]);// 나머지 슬롯은 비우기
        }
    } 
    // 아이템 넣기
    public void AddItem(Item item, bool useInventoryIndex = false)
    {
         // 인덱스가 유효하면 해당 슬롯에 넣기
        if (useInventoryIndex  && item.InventoryIndex >= 0 && item.InventoryIndex < slots.Count)
        {
            Slot slot = slots[item.InventoryIndex];
            // items.Add(item);
            slot.SetItem(item);
           // slot.AddCount(1);
            uiInventory.UpdateSlot(slot);
            return;
        }
        Slot exitsSlot = null;

        foreach (var slot in slots)
        {
            // 동일한 아이템이 있다면
            if (item != null && slot.HasItem(item))
            {
                exitsSlot = slot;
                break;
            }
        }
        if (exitsSlot != null)
        {
            exitsSlot.AddCount(1);
        }
        // 동일한 아이템이 없다면 
        else
        {
            // 슬롯의 빈곳을 찾아서
            Slot emptySlot = FindEmptySlot();
            if (emptySlot == null) { return; }

            // 아이템 넣어주기
            items.Add(item);
            emptySlot.SetItem(item);
            emptySlot.AddCount(1);
            uiInventory.UpdateSlot(emptySlot);
        }
    }

    // 슬롯 비우기
    public void RemoveItem(Item item)
    {
        Slot slot = slots[FindItemPos(item)];
        slot.ClearSlot();
    }
    
    // 인벤토리 비우기
    public void ClearInventory()
    {
        for (int i =0 ; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }
    // 빈 슬롯 찾기
    private Slot FindEmptySlot()
    {
        Slot emptySlot = null;
        foreach (var slot in slots)
        {
            if (slot.Item == null)
            {
                emptySlot = slot;
                return emptySlot;
            }
        }
        return emptySlot;
    }
    // 해당 아이템의 위치 찾기
    public int FindItemPos(Item item)
    {
        int index = -1;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item == item)
            {
                index = i;
                return index;
            }
        }
        return index;
    }
    // 장착 중인 아이템 해제
    //public void SetEquipItemSlot(EquipItem equipItem)
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        if (items[i] == null)
    //        {
    //            continue;
    //        }
    //        if (items[i] is EquipItem equippedItem)
    //        {
    //            // 이미 같은 타입의 아이템을 착용중이라면 교채
    //            if (equipItem.EquipType == equippedItem.EquipType
    //              && equipItem != equippedItem && equippedItem.Equipped)
    //            {
    //                Slot slot = slots[FindItemPos(equippedItem)];
    //                UISlot uiSlot = slot.GetComponent<UISlot>();

    //                uiInventory.UpdateSlot(slot);
    //                equippedItem.UnEquip(); // 장착 해제
    //                uiSlot.SlotOutline(equippedItem.Equipped);
    //                return;
    //            }
    //        }
    //    }
    //}
}
