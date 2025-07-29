using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotDrag : BaseDrag<Slot, Item>
{
    protected override Item GetData() => slot.Item;
    protected override Sprite GetSprite(Item data) => data.Image;
    private Inventory inventory;

    public override void Init()
    {
       base.Init();
    }
   
    protected override void OnDropCompleted(Item droppedItem, Slot originSlot)
    {
        if (SlotIsOut() && droppedItem != null)
        {
            // 인벤토리에 다시 넣어주기
            inventory = UIManager.Instance.inventorys[0];
            inventory.AddItem(droppedItem);

            // 장비 해제
            originSlot.ClearSlot();
        }
    }
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
            // EquipmentSlot 스크립트가 붙은 오브젝트 감지
            if (result.gameObject.GetComponentInParent<EquipmentSlot>() != null)
                return false;
        }

        return true; // 해당 안 됨
    }
}
