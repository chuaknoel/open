using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTempSlotDrag : BaseDrag<Slot, SkillData>
{
    [SerializeField] private SkillQuickSlotManager skillQuickSlotManager;

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

    public  void Init(Canvas _canvas, Image _dragImage, SkillQuickSlotManager _skillQuickSlotManager)
    {
        canvas = _canvas;
        dragItemImage = _dragImage;
        skillQuickSlotManager = _skillQuickSlotManager;
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
        SkillTempSlot currentSlot = GetComponent<SkillTempSlot>();
        
        if(!CheckMousePointerSlot<SkillTempSlot>()) // 스킬 해제
        {
            currentSlot.ClearSlot();
            currentSlot.UpdateSkill();

            int count = currentSlot.GetComponent<SkillTempSlot>().slotIndex;
            skillQuickSlotManager.skillSlots[count].ClearSlot();
            skillQuickSlotManager.skillSlots[count].UpdateSkill();
        }
    }   
}
