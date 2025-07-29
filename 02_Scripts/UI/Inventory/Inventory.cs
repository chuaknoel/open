using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리ㅣ 스크립트입니다.
/// </summary>
public class Inventory : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private QuickSlotManager quickSlotManager;
    [SerializeField] private GameObject quickSlots;

    protected UIInventory uiInventory;

    public List<Slot> slots = new List<Slot>();
    public List<Item> items = new List<Item>();

    public int slotCount = 20;
    public GameObject uiSlotPrefab;
    public Transform slotsParent;
    public Image draggedImage;
    public Tooltip toolTip;

    public void Init()
    {
        uiManager = UIManager.Instance; 
        uiInventory = GetComponent<UIInventory>();
        uiInventory.Init();
    }
    /// <summary>
    /// 슬롯을 동적으로 생성합니다.
    /// </summary>
    public async Task CreateSlots()
    {
        // 초기화
        for (int i = 0; i < slotCount; i++)
        {           
           // GameObject slot = Instantiate(uiSlotPrefab, slotsParent);
            GameObject slot = Instantiate(await AddressableManager.Instance.LoadAsset<GameObject>("Slot"), slotsParent);
            if (slot.TryGetComponent<Slot>(out Slot _slot))
            {
                slots.Add(_slot);
                slots[i].slotIndex = i;
                slots[i].Init();

                if (slot.TryGetComponent<ItemDrag>(out ItemDrag itemDrag))
                {
                    itemDrag.Init(canvas, quickSlots);
                }
                if (slot.TryGetComponent<SlotEvent>(out SlotEvent slotEvent))
                {
                    slotEvent.Init();
                }             
            }        
        }
    }
  
    /// <summary>
    /// 인벤토리 초기 설정입니다.
    /// </summary>
    public void SetInventory()
    {
        Logger.Log("인벤토리 초기 설정 : "+ items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            AddItemAtIndex(items[i]); // 아이템 넣기
        }
        for (int i = items.Count; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
            uiInventory.UpdateSlot(slots[i]);// 나머지 슬롯은 비우기
        }
    } 
    
    /// <summary>
    /// 정해지지 않은 위치에 아이템을 넣습니다.
    /// </summary>
    /// <param name="item">아이템</param>
    public void AddItem(Item item)
    {
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
            QuestEvents.ItemCollected(item.ItemId, 1);
            exitsSlot.AddCount(1);
            uiInventory.UpdateSlot(exitsSlot);
        }
        // 동일한 아이템이 없다면 
        else
        {
            // 슬롯의 빈곳을 찾아서
            Slot emptySlot = FindEmptySlot();
            if (emptySlot == null) { return; }

            // 아이템 넣어주기
            QuestEvents.ItemCollected(item.ItemId, 1);

            items.Add(item);
            emptySlot.SetItem(item);
            item.Add(1);
            uiInventory.UpdateSlot(emptySlot);
        }
    }

    /// <summary>
    /// 정해진 위치에 아이템을 넣습니다.
    /// </summary>
    /// <param name="item">아이템</param>
    public void AddItemAtIndex(Item item)
    {
        // 아이템 위치 인덱스가 유효하고, 슬롯 리스트 안에 들어갈 수 있는 범위라면
        if (item.InventoryIndex >= 0 && item.InventoryIndex < slots.Count)
        {
            Slot slot = slots[item.InventoryIndex];
            slot.SetItem(item);
            uiInventory.UpdateSlot(slot);         
            return;
        }
    }

   /// <summary>
   /// 슬롯 비우기
   /// </summary>
   /// <param name="item">아이템</param>
    public void RemoveItem(Item item)
    {
        Slot slot = slots[FindItemPos(item)];
        slot.ClearSlot();
        slot.UpdateSlot();
    }
    
   /// <summary>
   /// 인벤토리 전체 비우기
   /// </summary>
    public void ClearInventory()
    {
        for (int i =0 ; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }
    
    /// <summary>
    /// 빈 슬롯 찾기
    /// </summary>
    /// <returns></returns>
    public Slot FindEmptySlot()
    {
        Slot emptySlot = null;

        foreach (var slot in slots)
        {   
            if (slot.Item == null || string.IsNullOrEmpty(slot.Item.ItemName))
            {
                emptySlot = slot;
                return emptySlot;
            }
        }
        return emptySlot;
    }
    
    /// <summary>
    /// 아이템 위치 찾기
    /// </summary>
    /// <param name="item">아이템</param>
    /// <returns></returns>
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
}
