using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotEvent : SlotEvent
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        uiManager = UIManager.Instance;
        dragitemImage = uiManager.dragitemImage;
        inventory = uiManager.inventorys[0];
        toolTip = inventory.toolTip;

        slot = GetComponent<Slot>();
    }

    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public override void SlotClickEvent()
    {
        //  사용
        slot.UseItem();
    }
}
