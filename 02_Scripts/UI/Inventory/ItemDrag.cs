using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : BaseDrag<Slot, Item>
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quickSlots;

    public void Init(Canvas _canvas, GameObject _quickSlots)
    {
        base.Init();
        canvas = _canvas;
        uiManager = UIManager.Instance;
        inventory = uiManager.inventorys[0].gameObject;
        quickSlots = _quickSlots;
        dragItemImage = uiManager.dragitemImage;
    }

    protected override Item GetData() => slot.Item;
    protected override Sprite GetSprite(Item data) => data.Image;

    protected override void OnDropCompleted(Item droppedItem, Slot originSlot)
    {  
        if (SlotIsOut() && slot.Item != null)
        {
            if (slot is QuickSlot quickSlot)
            {
                quickSlot.ClearSlot();
            }
            else
            {
                uiManager.OpenWindow(uiManager.destroyItemWindow);
                uiManager.destroyItemWindow.GetComponent<DestroyWindow>().slot = slot;
            }
        }
    }

    /// <summary>
    /// 마우스가 인벤토리밖인지 검사
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
            // Inventory 스크립트가 붙은 오브젝트 감지
            if (result.gameObject.GetComponentInParent<Inventory>() != null)
                return false;

            // QuickSlot도 마찬가지로 검사
            if (result.gameObject.transform.IsChildOf(quickSlots.transform))
                return false;
        }

        return true; // 둘 다 해당 안 됨
    }
}
