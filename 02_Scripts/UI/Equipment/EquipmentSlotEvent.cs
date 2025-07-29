using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotEvent : SlotEvent
{ 
    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public override void SlotClickEvent()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // 더블 클릭시
        if (timeSinceLastClick <= doubleClickThreshold)
        {  
            // Tooltip 비활성화
            base.CloseTooltip();
            // 장착 해제
            slot.UseItem();
        }
        // 일반 클릭시
        else
        {
            if (slot.Item == null)
            {
                return;
            }
            base.ShowTooltip();
        }
    }
    protected override void ChangeSlot()
    {
        if (dragitemImage != null && DragData<Slot, Item>.DraggedData != null)
        {
           Slot originSlot = DragData<Slot, Item>.OriginSlot;

            // 두 슬롯의 아이템 참조
            Item draggedItem = DragData<Slot, Item>.DraggedData;
            EquipItem item = draggedItem as EquipItem;
           
            if(slot is EquipmentSlot _slot)
            {
                if (item.EquipType != _slot.equipSlotType)
                {
                    Logger.Log("장비 타입이 일치하지 않음");
                    return; // 타입 다르면 교체 안함
                }
            }
             if(draggedItem is CompanionItem companionItem)
            {
                return;
            }

            Item currentItem = slot.Item;

            // 아이템 정보 임시 저장
            Item temp = slot.Item;

            // 드래그 받은 슬롯의 정보를 변경
            slot.SetItem(DragData<Slot, Item>.DraggedData);
            slot.Item.Move(slot.slotIndex);

            // 드래그한 슬롯의 정보를 변경
            originSlot.SetItem(temp);

            if (originSlot.isInventorySlot)
            {
                originSlot.ClearSlot();
                return;
            }
        }
    }
}
