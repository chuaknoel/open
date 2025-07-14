using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    public Image iconImage;

    public void Init(Inventory _inventory, UIInventory _uiInventory)
    {
        inventory = _inventory;
        uiInventory = _uiInventory;
    }

    public override void ClearSlot()
    {
        this.item = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        itemText.text = "";
    }
    public override void UseItem()
    {
        if (item != null)
        {
            item.Use();
            inventory.slots[inventory.FindItemPos(item)].UseItem();

            //UpdateSlot();
        }
    }
    public override void UpdateSlot()
    {
        if (item.Count == 0)
        {
            ClearSlot();
        }
        else
        {
            itemText.text = item.Count.ToString();
        }     
    }
}
