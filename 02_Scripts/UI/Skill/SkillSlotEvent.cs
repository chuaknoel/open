using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotEvent : SlotEvent
{
    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public override void SlotClickEvent()
    {
        //  사용
        slot.UseItem();
    }
}
