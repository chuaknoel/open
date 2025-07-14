using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillQuickSlotEvent : SlotEvent, IDropHandler
{
    public SkillQuickSlotManager skillQuickSlotManager;
    private SkillQuickSlot skillQuickSlot;
    private void OnEnable()
    {
        dragitemImage = GameObject.Find("DragImage").GetComponent<Image>();
        skillQuickSlot = GetComponent<SkillQuickSlot>();
    }

    /// <summary>
    /// 아이템이 Drop 됬을 경우
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public new void OnDrop(PointerEventData eventData)
    {
        if(skillQuickSlot.slotType != SlotType.skill) { return; }
        // 슬롯 변경
        ChangeSlot();
    }
    /// <summary>
    /// 슬롯 변경
    /// </summary>
    protected override void ChangeSlot()
    {
        if (dragitemImage != null && DragData<Slot, SkillData>.DraggedData != null)
        {
            ISkillSlot currentSlot = GetComponent<ISkillSlot>();
       
            // 드래그한 아이템 정보 임시 저장
            SkillData temp = currentSlot.GetSkill();
         
            // 드래그 받은 슬롯의 정보를 변경
            currentSlot.SetSkill(DragData<Slot, SkillData>.DraggedData);
            (currentSlot as SkillQuickSlot)?.UpdateSkill();


            Slot originSlot = DragData<Slot, SkillData>.OriginSlot;

            // 드래그한 슬롯이 스킬 퀵 슬롯이라면
            if (originSlot is SkillQuickSlot originQuickSlot)
            {
                // 드래그한 슬롯의 정보를 변경
                originQuickSlot.SetSkill(temp);
                originQuickSlot.UpdateSkill();

                skillQuickSlotManager.skillSlots[originQuickSlot.slotNum].SetSkill(originQuickSlot.GetSkill());
                skillQuickSlotManager.skillSlots[originQuickSlot.slotNum].UpdateSkill();
            }

            skillQuickSlot = GetComponent<SkillQuickSlot>();

            // 진짜 스킬 퀵슬롯에도 스킬을 넣어준다.
            // 진짜 스킬 퀵슬롯에 해당 문자의 슬롯이 있다면
            skillQuickSlotManager.skillSlots[skillQuickSlot.slotNum].SetSkill(skillQuickSlot.GetSkill());
            skillQuickSlotManager.skillSlots[skillQuickSlot.slotNum].UpdateSkill();
        }
    }
}
