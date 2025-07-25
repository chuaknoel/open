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

    public override void Init()
    {
        base.Init();
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
        SlotIsOut();
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
            // 마우스가 플레이어 스킬 슬롯이라면
            if (result.gameObject.GetComponentInParent<PlayerSkillQuickSlot>() != null)
            {
                Logger.Log("임시 스킬 슬롯" + skillTempSlotManager._tempSkillSlot != null);
                // 임시 스킬 슬롯에 넣어줌
                if (skillTempSlotManager._tempSkillSlot != null)
                {
                    int count = 0;
                    foreach (Transform child in skillTempSlotManager._tempSkillSlot.transform)
                    {
                        Logger.Log("임시 스킬 슬롯 : " + child.gameObject.TryGetComponent<SkillTempSlot>(out SkillTempSlot _Dslot));
                        if(child.gameObject.TryGetComponent<SkillTempSlot>(out SkillTempSlot _slot))
                        {
                            SkillTempSlot slot = _slot;

                            if (slot.slotIndex == count)
                            {
                                slot.SetSkill(slot.GetSkill());
                                slot.UpdateSkill();
                            }
                            count++;
                        }                     
                    }
                }             
                return false;
            }
            else if (result.gameObject.GetComponentInParent<SkillTempSlot>() != null)
            {
                // 진짜 스킬 퀵슬롯에도 스킬을 넣어준다.       
                if(TryGetComponent<SkillSlot>(out SkillSlot skillSlot))
                {
                    currentSlot = skillSlot;
                    int count = result.gameObject.GetComponentInParent<SkillTempSlot>().slotIndex;

                    skillQuickSlotManager.skillSlots[count].SetSkill(currentSlot.GetSkill());
                    skillQuickSlotManager.skillSlots[count].UpdateSkill();
                    return false;
                }           
            }
        }
        return true;
    }

}
