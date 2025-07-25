using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillQuickSlotDrag : BaseDrag<Slot, SkillData>, ITempSkillSlotUpdater
{
    private TempSkillQuickSlotManager tempSkillQuickSlotManager;
    [SerializeField] private PlayerSkillQuickSlot playerSkillQuickSlot;
    [SerializeField] private SkillTempSlotManager skillTempSlotManager;

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
        base.Init();
        canvas = _canvas;
        dragItemImage = _dragImage;
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
        PlayerSkillQuickSlot currentSlot = GetComponent<PlayerSkillQuickSlot>();

       //if (SlotIsOut())
       // {
            currentSlot.ClearSlot();
            currentSlot.UpdateSkill();

            // 임시 스킬 슬롯도 업데이트 
            UpdateTempSkillQuickSlot();
       // }
        
    }

    /// <summary>
    /// 마우스가 인벤토리밖인지 검사
    /// </summary>
    /// <returns></returns>
    protected override bool SlotIsOut()
    {
        //PointerEventData pointerData = new PointerEventData(EventSystem.current)
        //{
        //    position = Input.mousePosition
        //};

        //List<RaycastResult> results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(pointerData, results);
        //PlayerSkillQuickSlot currentSlot = GetComponent<PlayerSkillQuickSlot>();

        //foreach (var result in results)
        //{
        //    // 스킬 해제 
        //    if (result.gameObject.GetComponentInParent<PlayerSkillQuickSlot>() != null)
        //    {
        //       // int count = currentSlot.GetComponent<PlayerSkillQuickSlot>().slotIndex;
        //        currentSlot.ClearSlot();
        //        currentSlot.UpdateSkill();

        //        // 임시 스킬 슬롯도 업데이트 
        //        UpdateTempSkillQuickSlot();
        //        return false;
        //    }
        //}
        return true; // 둘 다 해당 안 됨
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
}
