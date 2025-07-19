using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDrag : BaseDrag<Slot, SkillData>
{
    [SerializeField] private SkillQuickSlotManager skillQuickSlotManager;
    [SerializeField] private SkillTempSlotManager skillTempSlotManager;
  
    private SkillSlot currentSlot;


    protected override SkillData GetData()
    {
        ISkillSlot skillSlot = slot as ISkillSlot;
        if (skillSlot != null)
        {
            return skillSlot.GetSkill();
        }
        return null;
    }

    protected override Sprite GetSprite(SkillData data) => data.skillImage;

    public virtual void Init(Canvas _canvas, Image _dragImage)
    {
        canvas  = _canvas;
        dragItemImage= _dragImage;
    }
    // 드래그 시작시
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }
    // 드래그 중
    public override void OnDrag(PointerEventData eventData)
    { 
        base.OnDrag(eventData);
    }
    // 드래그 끝
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);     
    }
    protected override void OnDropCompleted(SkillData droppedSkill, Slot originSlot)
    {      
        // 마우스가 플레이어 스킬 슬롯이라면
        if (CheckMousePointerSlot<PlayerSkillQuickSlot>())
        {
            // 임시 스킬 슬롯에 넣어줌
            int count = 0;
            foreach (Transform child in skillTempSlotManager._tempSkillSlot.transform)
            {
                SkillTempSlot slot = child.gameObject.GetComponent<SkillTempSlot>();

                if (slot.slotIndex == count)
                {
                    slot.SetSkill(slot.GetSkill());
                    slot.UpdateSlot();
                }
                count++;
            }
        }
        else if(CheckMousePointerSlot<SkillTempSlot>())
        {
            // 진짜 스킬 퀵슬롯에도 스킬을 넣어준다.
            currentSlot = GetComponent<SkillSlot>();
           
            int count = draggedSlot.GetComponent<SkillTempSlot>().slotIndex;

            skillQuickSlotManager.skillSlots[count].SetSkill(currentSlot.GetSkill());
            skillQuickSlotManager.skillSlots[count].UpdateSkill();
        }    
    }
}
