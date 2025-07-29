using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 동료 슬롯을 드래그할 때 호출되어지는 클래스입니다.
/// </summary>
public class CompanionSlotDrag : BaseDrag<CompanionSlot, Item>
{
    [SerializeField] private UIManager uiManager;
    private Inventory inventory;

    /// <summary>
    /// 슬롯의 아이템을 가져옵니다.
    /// </summary>
    /// <returns></returns>
    protected override Item GetData()=> slot.Item;

    /// <summary>
    /// 슬롯의 이미지를 가져옵니다.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected override Sprite GetSprite(Item data) => data.Image;

    /// <summary>
    /// 초기화합니다.
    /// </summary>
    public override void Init()
    {
        base.Init();
        uiManager = UIManager.Instance;
    }
   
    /// <summary>
    /// 드랍 완료시 호출되어지는 메서드 입니다.
    /// </summary>
    /// <param name="droppedItem">드롭된 아이템</param>
    /// <param name="originSlot">드래그 시작한 슬롯</param>
    protected override void OnDropCompleted(Item droppedItem, CompanionSlot originSlot)
    {
        if (SlotIsOut() && droppedItem != null)
        {
            // 인벤토리에 다시 넣어주기
            inventory = UIManager.Instance.inventorys[0];
            inventory.AddItem(droppedItem);

            // 아이템 장착 해제
            originSlot.ClearSlot();
        }
    } 

    /// <summary>
    /// 슬롯의 드래그 이미지가 정해진 범위를 벗어났는지 확인합니다.
    /// </summary>
    /// <returns></returns>
    protected override bool SlotIsOut()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            // CompanionSlot 스크립트가 붙은 오브젝트 감지
            if (result.gameObject.GetComponentInParent<CompanionSlot>() != null)
                return false;
        }
        return true; 
    }
}
