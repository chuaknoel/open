using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillQuickSlotEvent : SlotEvent, IDropHandler, ITempSkillSlotUpdater
{
    [SerializeField] private SkillTempSlotManager skillTempSlotManager;
    private TempSkillQuickSlotManager tempSkillQuickSlotManager;
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
            Slot originSlot = DragData<Slot, SkillData>.OriginSlot;

            // 드래그한 슬롯이 스킬 퀵 슬롯이라면
            if (originSlot is PlayerSkillQuickSlot originQuickSlot)
            {
                PlayerSkillQuickSlot currentSlot = GetComponent<PlayerSkillQuickSlot>();

                // 스킬 쿨타임 멈춤
                var executor1 = currentSlot.GetComponent<SkillExecutor>();
                var executor2 = originQuickSlot.GetComponent<SkillExecutor>();

                currentSlot.StopSkillCoolTime();
                originQuickSlot.StopSkillCoolTime();

                SwapSkill(executor1, executor2);
                base.Swap(currentSlot, originQuickSlot);
                 
                currentSlot.ShowSkillCoolTime();
                originQuickSlot.ShowSkillCoolTime();
            }
            //드래그한 슬롯이 스킬 슬롯이라면
            if (originSlot is SkillSlot originSkillSlot)
            {
                PlayerSkillQuickSlot currentSlot = GetComponent<PlayerSkillQuickSlot>();
               
                // 드래그 받은 슬롯의 정보를 변경
                currentSlot.SetSkill(DragData<Slot, SkillData>.DraggedData);
                currentSlot.UpdateSkill();

                UpdateTempSkillQuickSlot();
            }
        }
    }

    /// <summary>
    /// 임시 스킬 슬롯을 업데이트 해주는 함수입니다.
    /// </summary>
    public void UpdateTempSkillQuickSlot()
    {
        // 임시 스킬 슬롯이 켜져있다면
        if (skillTempSlotManager.tempSkillQuickSlotManager != null)
        {
            // 임시 스킬 슬롯 업데이트
            tempSkillQuickSlotManager = skillTempSlotManager.tempSkillQuickSlotManager;
            tempSkillQuickSlotManager.SetTempSkillQuickSlot();
        }
    }

    private void SwapSkill(SkillExecutor executor01, SkillExecutor executor02)
    {
        // 드래그한 아이템 정보 임시 저장
        float temp = executor01.remainingTime;

        // 서로 Swap
        executor01.remainingTime = executor02.remainingTime;
        executor02.remainingTime = temp;    
    }
}
