using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : BaseDrag<Slot, Item>
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quickSlots;

    public void Init(Canvas _canvas,UIManager _uIManager, GameObject _inventory, GameObject _quickSlots, Image _dragitemImage)
    {
        canvas = _canvas;
        uiManager = _uIManager;
        inventory = _inventory;
        quickSlots = _quickSlots;
        dragItemImage = _dragitemImage;
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
    private bool SlotIsOut()
    {
        return (!RectTransformUtility.RectangleContainsScreenPoint(inventory.gameObject.GetComponent<RectTransform>(), Input.mousePosition, null)) &&
            (!RectTransformUtility.RectangleContainsScreenPoint(quickSlots.gameObject.GetComponent<RectTransform>(), Input.mousePosition, null));
    }
}
