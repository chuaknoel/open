using Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class CompanionEquipManager : EquipmentManager
{
    [SerializeField] private CompanionWindow companionWindow;
    public List<CompanionInventory> inventories = new List<CompanionInventory>();
    public CompanionInventory currentInventory;
    public CompanionItem currentItem;
    public override void Init()
    {
        UIManager uiManager = UIManager.Instance;

        this.inventory = uiManager.inventorys[0];
        this.uiInventory = inventory.GetComponent<UIInventory>();
        this.companionWindow = uiManager.companionWindow;
        this.toolTip = uiManager.tooltip;
        inventories.AddRange(uiManager.GetComponentsInChildren<CompanionInventory>(true));
        
        foreach (var inventory in inventories)
        {
            inventory.Init();

            foreach (var slot in inventory.slots)
            {
                slot.Init();
                slot.GetComponent<CompanionSlotDrag>().Init();
                slot.GetComponent<CompanionSlotEvent>().Init();
            }
        }
    }

 
    public override void Equip(Item _item)
    {
        if (_item == null)
        {
            Logger.LogError("아이템이 존재하지 않습니다.");
            return;
        }
        currentItem = _item as CompanionItem;
        CompanionType type = currentItem.CompanionType;
        CompanionItemType itemType = currentItem.CompanionItemType;
  
        FindCurrentInventory(type); // 현재 동료 인벤토리 찾기
      
        //Slot slot = currentInventory.slots[0];
        //CompanionSlot companionSlot = (CompanionSlot)slot;
   
        if (currentInventory.companionItems.ContainsKey(itemType))
        {
            // 교체
            CompanionItem oldItem = currentInventory.companionItems[itemType] as CompanionItem;
            inventory.AddItem(oldItem);
            uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(oldItem)]);
            ShowItems();
        }
        currentInventory.companionItems[itemType] = currentItem;
        equipEvent?.Invoke(currentItem);

        ShowItems();
        currentInventory.items.Add(currentItem);
        currentInventory.AddItem(currentItem);

        currentItem.Remove(1);
      // 툴팁 비활성화
        if (toolTip.gameObject.activeSelf)
        {
            toolTip.CloseUI();
        }
    }
    public override void UnEquip(Item item)
    {
        currentItem = item as CompanionItem;
        CompanionType type = currentItem.CompanionType;
        FindCurrentInventory(type); // 현재 동료 인벤토리 찾기

        currentInventory.companionItems.Remove(currentItem.CompanionItemType);
        unequipEvent?.Invoke(item as EquipItem);

        inventory.AddItem(item);
        ShowItems();
        uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(item)]);
        currentItem = null;
    }
    public override void ShowItems()
    {    
        if (companionWindow.gameObject.activeInHierarchy)
        {
            companionWindow.UpdateInfo(ReturnCompanionData(),currentInventory);
           // companionWindow.ShowEquipInfo();
        }
    }

    /// <summary>
    /// 현재 동료 아이템에 알맞은 동료 데이터를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public CompanionData ReturnCompanionData()
    {
        // CompanionManager 완성되면 추후에 수정하기!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        CompanionData data = null;
        DeliverCompanionsData deliverCompanionsData = UIManager.Instance.deliverCompanionsData;
        for (int i = 0; i < deliverCompanionsData.companionDatas.Count; i++)
        {
            if (currentItem.CompanionType.ToString() == deliverCompanionsData.companionDatas[i].ID)
            {
                data = deliverCompanionsData.companionDatas[i];
            }
        }

        return data;
    }
    /// <summary>
    /// 현재 동료 인벤토리 찾기
    /// </summary>
    /// <param name="_itemType"></param>
    public void FindCurrentInventory(CompanionType type)
    {
       for (int i = 0; i < inventories.Count;i++)
        {
            if (inventories[i].slots[0] is CompanionSlot slot)
            {
                if (slot.companionType == type)
                {
                    currentInventory = inventories[i];
                }
                break;
            }    
        }
    }
}
