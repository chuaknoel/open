using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private GameObject equipWindow;
    [SerializeField] private Tooltip toolTip;

    public Dictionary<EquipType, Item> equippedItems = new();

    public UnityAction<EquipItem> equipEvent;
    public UnityAction<EquipItem> unequipEvent;

    public void Equip(Item item)
    {
        EquipItem equipItem = item as EquipItem;
        EquipType type = equipItem.EquipType;

        if (equipItem == null) return;
        if (equippedItems.ContainsKey(type))
        {
            // 교체
            EquipItem oldItem = equippedItems[type] as EquipItem;
            inventory.AddItem(oldItem,false);
            uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(oldItem)]);
            ShowEquipItems();
        }

        equippedItems[type] = equipItem;
        equipEvent?.Invoke(equipItem);

        ShowEquipItems();

        // 툴팁 비활성화
        if(toolTip.gameObject.activeSelf)
        {
            toolTip.CloseUI();
        }
    }
    public void UnEquip(Item item, EquipType equipSlotType)
    {
        equippedItems.Remove(equipSlotType);
        unequipEvent?.Invoke(item as EquipItem);

        inventory.AddItem(item, false);
        ShowEquipItems();
        uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(item)]);
        Logger.Log("장착 해제");
    }
    public void ShowEquipItems()
    {
        if (equipWindow.activeSelf)
        {
            EquipmentWindow equipmentWindow = equipWindow.GetComponent<EquipmentWindow>();
            equipmentWindow.UpdateEquipSlot();
            equipmentWindow.ShowEquipInfo();
        }
    }
}
