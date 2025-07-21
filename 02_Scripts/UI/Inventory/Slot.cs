using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
   public SlotType slotType = SlotType.item;

    [SerializeField] protected Inventory inventory;
    [SerializeField] protected UIInventory uiInventory;
    [SerializeField] protected EquipmentManager equipmentManager;
    private QuickSlotManager quickSlotManager;

    [SerializeField] protected Item item;

    public int slotIndex;

    private ItemColorHelper itemColorHelper;

    [SerializeField] private Image backGround;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public Color basicBackgroundColor;

    public Item Item => item;
    public Image ItemImage => itemImage;
    public TextMeshProUGUI ItemText => itemText;

    public virtual void Init(Inventory _inventory, EquipmentManager _equipmentManager, QuickSlotManager _quickSlotManager)
    {
        inventory = _inventory;
        equipmentManager = _equipmentManager;
     //   itemImage = transform.GetChild(0).GetComponent<Image>();
        uiInventory = inventory.gameObject.GetComponent<UIInventory>();
        itemColorHelper = inventory.gameObject.GetComponent<ItemColorHelper>();
        quickSlotManager = _quickSlotManager;
    }
    public virtual void SetItem(Item _item)
    {
        if (_item != null)
        {
            this.item = _item;
            itemImage.enabled = true;
            itemImage.sprite = item.Image;
            itemText.text = item.Count.ToString();
        }
        else
        {
            ClearSlot();
        }
    }
    public void AddCount(int amount)
    {
        item.Add(amount);
    }
    // 
    public virtual void UseItem()
    {
        if (item == null) { return; }
        item.Use();

        if (item is EquipItem equip)
        {
            equipmentManager.Equip(equip);
        }
        else if (item.Type == ItemType.Consume)
        {
            // 퀵슬롯도 같이 사용
            foreach (QuickSlot _slot in quickSlotManager.slots)
            {
                if(_slot.Item != null && _slot.Item.ItemName == item.ItemName)
                {
                    _slot.Item.Use();
                    _slot.UpdateSlot();
                }
            }         
        }
        if (Item.Count == 0)
        {
            inventory.items.Remove(item);
            ClearSlot();
            UpdateSlot();
        }
        UpdateSlot();   
    }
    // 
    public bool HasItem(Item _item)
    {
        return item == _item;
    }
    // 슬롯 정보 업데이트
    public virtual void UpdateSlot()
    {
        if (item == null) { ClearSlot(); return; }
        itemImage.enabled = true;
        itemImage.sprite = item.Image;
        itemText.text = item.Count.ToString();

        if (item is EquipItem equipItem)
        {
            backGround.color = itemColorHelper.GetColor(equipItem.ItemGrade);
        }
        else
        {
            backGround.color = basicBackgroundColor;
        }
    }
    // 슬롯 비우기
    public virtual void ClearSlot()
    {
        this.item = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemText.text = "";

        basicBackgroundColor.a = 1;
        backGround.color = basicBackgroundColor;
    }
}
