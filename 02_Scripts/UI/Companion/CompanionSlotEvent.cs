using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompanionSlotEvent : SlotEvent
{
    public override void Init()
    {
      base.Init();
    }

    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public override void SlotClickEvent()
    {
        base.SlotClickEvent();
    }

    /// <summary>
    /// 아이템이 Drop 됬을 경우
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public override void OnDrop(PointerEventData eventData)
    {
       base.OnDrop(eventData);
    }

    /// <summary>
    /// 동료 슬롯 변경 함수입니다.
    /// </summary>
    protected override void ChangeSlot()
    {
        if (dragitemImage != null && DragData<Slot, Item>.DraggedData != null)
        {
            if(DragData<Slot, Item>.DraggedData is CompanionItem item)
            {
                Slot originSlot = DragData<Slot, Item>.OriginSlot;
                Item originItem = DragData<Slot, Item>.DraggedData;
               
                // 동료 슬롯의 동툐 타입과 아이템의 동료 타입이 같다면
                if(slot is CompanionSlot _slot)
                {
                    if (originItem is CompanionItem _originItem
                    && _originItem.CompanionType == _slot.companionType
                    && _originItem.CompanionItemType == _slot.companionItemType)
                    {
                        // 동료 슬롯의 정보를 변경합니다.
                        slot.SetItem(originSlot.Item);
                        slot.UpdateSlot();

                        // 인벤토리 슬롯은 비워줍니다.
                        originSlot.ClearSlot();
                    }
                }     
            }
        }
    }
}
