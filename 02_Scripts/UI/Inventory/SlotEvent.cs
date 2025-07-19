using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotEvent : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Canvas canvas;
    protected UIManager uiManager;
    protected Inventory inventory;
    protected UIInventory uiInventory;
    protected Slot slot;
    protected UISlot uiSlot;
    [SerializeField] protected Tooltip toolTip;

    [SerializeField] protected Image dragitemImage;
    public Outline outline;

    public float doubleClickThreshold = 0.3f; 
    protected float lastClickTime = -1f;

    void OnEnable()
    {
        slot = GetComponent<Slot>();
        uiSlot = GetComponent<UISlot>();
    }
    public virtual void Init(Canvas _canvas,UIManager _uiManager, GameObject _inventory, Image _dragitemImage)
    {
        this.canvas = _canvas;
        uiManager = _uiManager;

        dragitemImage = _dragitemImage;
        inventory = _inventory.GetComponent<Inventory>();
        toolTip = inventory.toolTip;
    }
    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    public virtual void SlotClickEvent()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // 더블 클릭시
        if (timeSinceLastClick <= doubleClickThreshold)
        {
            // 아이템 사용
            slot.UseItem();
        }
        // 일반 클릭시
        else
        {
            if(slot.Item == null)
            {
                return;
            }
            ShowTooltip();
        }               
    }
    /// <summary>
    /// 아이템이 Drop 됬을 경우
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (slot.slotType != SlotType.item) { return; }

        // 슬롯 변경
        ChangeSlot();
    } 
    /// <summary>
    /// 슬롯 변경
    /// </summary>
    protected virtual void ChangeSlot()
    {
        if (dragitemImage != null && DragData<Slot, Item>.DraggedData != null)
        {
            // 아이템 정보 임시 저장
            Item temp = slot.Item;

            // 드래그 받은 슬롯의 정보를 변경
            slot.SetItem(DragData<Slot, Item>.DraggedData);
            slot.Item.Move(slot.slotIndex);

            // 드래그한 슬롯의 정보를 변경
            Slot originSlot = DragData<Slot, Item>.OriginSlot;
            originSlot.SetItem(temp);

            if(originSlot is QuickSlot quickSlot)
            {
                quickSlot.ClearSlot();  
                return;
            }
            // 드래그 받은 슬롯에 아이템이 없지 않다면
            if (temp != null)
            {
                // 드래그한 슬롯의 아이템의 위치 변경
                originSlot.Item.Move(originSlot.slotIndex);       
            }
        }
    }
    /// <summary>
    /// Tooltip 보여주기
    /// </summary>
    protected void ShowTooltip()
    {
        // Tooltip 내용 변환
        TooltipParam tooltipParam = new TooltipParam(slot.Item.ItemName, ChangeItemType(slot.Item.Type),
                                    slot.Item.ItemDescription);
        // Tooltip 활성화
        toolTip.OpenUI(tooltipParam);
        // Tooltip 위치 변경
        ChangeTooltipPos();
    }
    /// <summary>
    /// Tooltip 비활성화
    /// </summary>
    protected void CloseTooltip()
    {
        // Tooltip 비활성화
        toolTip.CloseUI();
    }
    /// <summary>
    /// Tooltip ItemType 내용 변환
    /// </summary>
    /// <param name="itemtype">아이템 타입</param>
    /// <returns></returns>
    private string ChangeItemType(ItemType itemtype)
    {
        switch (itemtype.ToString())
        {
            case "Consume":
                return "소모품";
            case "Equip":
                return "장비 아이템";
            case "Important":
                return "중요 아이템";
            case "Collectibles":
                return "수집품";
            case "None":
                return "기타";
            default:
                return "기타";
        }
    }
    /// <summary>
    /// Tooltip 위치 변경
    /// </summary>
    private void ChangeTooltipPos()
    {
        Vector2 mouseScreenPos = slot.GetComponent<RectTransform>().position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mouseScreenPos,
            null,
            out Vector2 localPos
        );
        toolTip.rect.anchoredPosition = new Vector2(localPos.x + 198f, localPos.y - 53f);
    }

    protected void Swap<T>(T slot01, T slot02) where T : Component, ISkillSlot
    {
        // 드래그한 아이템 정보 임시 저장
        SkillData temp = slot01.GetSkill();

        // 서로 Swap
        slot01.SetSkill(slot02.GetSkill());
        slot01.UpdateSkill();

        slot02.SetSkill(temp);
        slot02.UpdateSkill();
    }
}
