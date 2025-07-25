using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] protected Inventory inventory;
    [SerializeField] protected UIInventory uiInventory;
    [SerializeField] private GameObject equipWindow;
    [SerializeField] protected Tooltip toolTip;

    public Dictionary<EquipType, Item> equippedItems = new();

    public UnityAction<Item> equipEvent;
    public UnityAction<Item> unequipEvent;

    public virtual void Init()
    {
        UIManager uIManager = UIManager.Instance;
        this.inventory = uIManager.inventorys[0];
        this.uiInventory = inventory.GetComponent<UIInventory>();
        this.equipWindow = uIManager.equipmentWindow.gameObject;
        this.toolTip = uIManager.tooltip;
    }
    public virtual void Equip(Item item)
    {
        EquipItem equipItem = item as EquipItem;
        EquipType type = equipItem.EquipType;

        if (equipItem == null) return;
        if (equippedItems.ContainsKey(type))
        {
            // 교체
            EquipItem oldItem = equippedItems[type] as EquipItem;
            inventory.AddItem(oldItem);
            uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(oldItem)]);
            ShowItems();
        }
        equippedItems[type] = equipItem;
        equipEvent?.Invoke(equipItem);
        // inventory.RemoveItem(equipItem);
        equipItem.Remove(1);
        ShowItems();

        // 툴팁 비활성화
        if(toolTip.gameObject.activeSelf)
        {
            toolTip.CloseUI();
        }
    }
    public virtual void UnEquip(Item item)
    {
        EquipItem equipItem = item as EquipItem;

        equippedItems.Remove(equipItem.EquipType);
        unequipEvent?.Invoke(item as EquipItem);

        inventory.AddItem(item);
        ShowItems();
        uiInventory.UpdateSlot(inventory.slots[inventory.FindItemPos(item)]);
    }
    public virtual void ShowItems()
    {
        if (equipWindow.activeInHierarchy)
        {
            EquipmentWindow equipmentWindow = equipWindow.GetComponent<EquipmentWindow>();
            equipmentWindow.UpdateEquipSlot();
            equipmentWindow.ShowEquipInfo();
        }
    }
}
