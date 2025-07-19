using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTempSlotEvent: SlotEvent, IDropHandler
{
    private SkillQuickSlotManager skillQuickSlotManager;
    private SkillTempSlot skillTempSlot;
  
    public void Init(SkillQuickSlotManager _skillQuickSlotManager)
    {
        dragitemImage = GameObject.Find("DragImage").GetComponent<Image>();
        skillQuickSlotManager = _skillQuickSlotManager;
        skillTempSlot = GetComponent<SkillTempSlot>();
    }

    /// <summary>
    /// 아이템이 Drop 됬을 경우
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public new void OnDrop(PointerEventData eventData)
    {
        if (skillTempSlot.slotType != SlotType.skill) { return; }
        // 슬롯 변경
        ChangeSlot();
    }
    /// <summary>
    /// 슬롯 변경
    /// </summary>
    protected override void ChangeSlot()
    {
        if (DragData<Slot, SkillData>.DraggedData != null)
        {
            Slot originSlot = DragData<Slot, SkillData>.OriginSlot;
           
            // 드래그한 슬롯이 스킬 퀵슬롯이라면
            if (originSlot is SkillQuickSlot originQuickSlot)
            {
                return;
            }
            else if (originSlot is SkillTempSlot originTempSlot)
            {          
                SkillTempSlot currentSlot = GetComponent<SkillTempSlot>();   
                base.Swap(currentSlot, originTempSlot);
              
                // 퀵슬롯도 같이 변경
                SkillQuickSlot slot01 = skillQuickSlotManager.skillSlots[currentSlot.slotIndex];
                SkillQuickSlot slot02 = skillQuickSlotManager.skillSlots[originTempSlot.slotIndex];
                base.Swap(slot01, slot02);

                return;
            }
            // 스킬북에서 스킬 등록
            else if(originSlot is SkillSlot originSkillSlot)
            {
                ISkillSlot currentSlot = GetComponent<ISkillSlot>();

                // 드래그한 아이템 정보 임시 저장
                SkillData temp = currentSlot.GetSkill();

                // 드래그 받은 슬롯의 정보를 변경
                currentSlot.SetSkill(DragData<Slot, SkillData>.DraggedData);
                (currentSlot as SkillTempSlot)?.UpdateSkill();

                return;
            }
           
        }
    }

}
