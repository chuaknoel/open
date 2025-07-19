using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.Actions.MenuPriority;


public class EquipmentSlot : Slot
{
    public EquipType equipSlotType;

    public override void SetItem(Item _item)
    {
        if (_item != null)
        {
            this.item = _item;
            itemImage.enabled = true;
            itemImage.sprite = item.Image;
        }
        else
        {
            ClearSlot();
        }
    }
    public override void ClearSlot()
    {
        this.item = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
    public override void UseItem()
    {
        if (item == null) { return; }


        equipmentManager.UnEquip(item, equipSlotType);

       
        ClearSlot();
    }
    // 슬롯 정보 업데이트
    public override void UpdateSlot()
    {
        if (item == null) { ClearSlot(); return; }
        itemImage.enabled = true;
        itemImage.sprite = item.Image;     
    }
}
