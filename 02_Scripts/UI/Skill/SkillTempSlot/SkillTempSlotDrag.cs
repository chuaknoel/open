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
        base.Init();
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
        
        if(SlotIsOut()) // 스킬 해제
        {
            currentSlot.ClearSlot();
            currentSlot.UpdateSkill();

            int count = currentSlot.GetComponent<SkillTempSlot>().slotIndex;
            skillQuickSlotManager.skillSlots[count].ClearSlot();
            skillQuickSlotManager.skillSlots[count].UpdateSkill();
        }
    }

    /// <summary>
    /// 마우스 포인터의 슬롯이 무엇인지 반환합니다.
    /// </summary>
    protected override bool SlotIsOut() 
    {
        // 마우스 위치에서 Raycast
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            // 마우스 위치의 오브젝트가 스킬 임시 슬롯이 아니라면 스킬 장착 해제.
            // Inventory 스크립트가 붙은 오브젝트 감지
            if (result.gameObject.GetComponentInParent<SkillTempSlot>() != null)
                return false;
        }
        return true;
    }
}
