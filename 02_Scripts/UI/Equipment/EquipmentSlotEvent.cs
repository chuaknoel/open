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
        base.ChangeSlot();
    }
}
