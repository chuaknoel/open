using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDrag : BaseDrag<Slot, SkillData>
{
    [SerializeField] private SkillTempSlotManager tempSlotManager;
    [SerializeField] private PlayerSkillQuickSlot playerSkillQuickSlot;

    private SkillQuickSlotManager skillQuickSlotManager;
    private SkillQuickSlot skillQuickSlot;
   
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

    public void Init(Canvas _canvas, Image _dragImage)
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
        // 마우스가 스킬 큇슬롯밖인지 검사
        if (SlotIsOut())
        {
            SkillQuickSlotEvent skillQuickSlotEvent = GetComponent<SkillQuickSlotEvent>();
            skillQuickSlotManager = skillQuickSlotEvent.skillQuickSlotManager;

            // 진짜 퀵슬롯의 스킬도 같이 해제
            skillQuickSlot = GetComponent<SkillQuickSlot>();

            // 진짜 스킬 퀵슬롯에도 스킬을 넣어준다.
            // 진짜 스킬 퀵슬롯에 해당 문자의 슬롯이 있다면
            skillQuickSlotManager.skillSlots[skillQuickSlot.slotNum].SetSkill(skillQuickSlot.GetSkill());
            skillQuickSlotManager.skillSlots[skillQuickSlot.slotNum].UpdateSkill();

            // 스킬 슬롯이 플레이어 스킬 퀵 슬롯이라면
            if(tempSlotManager._tempSkillSlot != null && playerSkillQuickSlot != null)
            {
                int count = 0;
                foreach (Transform child in tempSlotManager._tempSkillSlot.transform)               
                {               
                    SkillQuickSlot slot = child.gameObject.GetComponent<SkillQuickSlot>();
                    
                    if (playerSkillQuickSlot.slotNum == count)
                    {
                        slot.SetSkill(playerSkillQuickSlot.GetSkill());
                        slot.UpdateSkill();
                    }
                    count++;
                }
            }
            return;
        }
        else
        {
            // 스킬 Swap
            SkillData temp = ((ISkillSlot)slot).GetSkill();
            ((ISkillSlot)slot).SetSkill(droppedSkill);

            if (originSlot is SkillQuickSlot quickSlot)
            {
                SkillData tempOriginSkill = quickSlot.GetSkill();
                quickSlot.SetSkill(temp);
                quickSlot.UpdateSkill();
            }
        }
    }

    /// <summary>
    /// 마우스가 스킬 큇슬롯밖인지 검사
    /// </summary>
    /// <returns></returns>
    private bool SlotIsOut()
    {
        bool _result = false;

        // 내가 스킬 퀵슬롯이고, 마우스 위치의 오브젝트가 스킬 큇슬롯이 아니라면 스킬 장착 해제.
        if (TryGetComponent<SkillQuickSlot>(out var mySlot))
        {
            // 마우스 위치에서 Raycast
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            bool isOverSkillQuickSlot = false;

            foreach (var result in raycastResults)
            {
                if (result.gameObject.TryGetComponent<SkillQuickSlot>(out _))
                {
                    isOverSkillQuickSlot = true;
                    break;
                }
            }

            // 마우스 위치가 SkillQuickSlot이 아니면 해제
            if (!isOverSkillQuickSlot)
            {
                mySlot.ClearSlot(); 
                Logger.Log("스킬 장착 해제됨");
                _result = true;
                return _result;
            }
        }
        return _result;
    }
}
