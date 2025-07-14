using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotEvent : SlotEvent, IDropHandler
{
    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public override void SlotClickEvent()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        // 더블 클릭시
        if (timeSinceLastClick <= doubleClickThreshold)
        {
            // 아이템 사용
            slot.UseItem();
        }
        lastClickTime = Time.time;
    }

    /// <summary>
    /// 슬롯 변경
    /// </summary>
    protected override void ChangeSlot()
    {
        if (dragitemImage != null && DragData<Slot, Item>.DraggedData != null 
            && DragData<Slot, Item>.DraggedData.Type == ItemType.Consume)
        {
            // 아이템 정보 임시 저장
            Item temp = slot.Item;

            // 드래그 받은 슬롯의 정보를 변경
            slot.SetItem(DragData<Slot, Item>.DraggedData);

            // 드래그한 슬롯의 정보를 변경
            Slot originSlot = DragData<Slot, Item>.OriginSlot;

            if (originSlot != null)
            {
                if (originSlot is QuickSlot)
                {
                    originSlot.SetItem(temp);
                }
            }
        }
    }
}
