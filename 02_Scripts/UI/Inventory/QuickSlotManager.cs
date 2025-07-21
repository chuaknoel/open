using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 아이템 퀵슬롯을 관리하는 매니저 스크립트입니다.
/// </summary>
public class QuickSlotManager : MonoBehaviour
{
    public QuickSlot[] slots;

    /// <summary>
    /// 아이템 퀵슬롯들을 초기화합니다.
    /// </summary>
    /// <param name="inventory">인벤토리</param>
    /// <param name="uiInventory">UI 인벤토리</param>
    public void Init(Inventory inventory, UIInventory uiInventory)
    {
        foreach (var slot in slots)
        {
            slot.Init(inventory, uiInventory);
        }

        InputManager.Instance.inputActions.Player.QuickSlot.started += OnQuickSlotPressed;
    }

    /// <summary>
    /// 아이템 퀵슬롯에 아이템을 할당합니다
    /// </summary>
    /// <param name="index">아이템 위치</param>
    /// <param name="item">아이템</param>
    public void AssignItemToSlot(int index, Item item)
    {
        if(item.Type == ItemType.Consume)
        {
            slots[index].SetItem(item);
        }
    }

    /// <summary>
    /// 아이템 퀵슬롯을 키보드로 작동시 발생하는 함수입니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnQuickSlotPressed(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;
        UseSlot(int.Parse(keyName) - 1);
    }

    /// <summary>
    /// 아이템 퀵슬롯의 아이템들을 사용시 발생하는 함수입니다.
    /// </summary>
    /// <param name="index"></param>
    private void UseSlot(int index)
    {
        slots[index].UseItem();
    }
}
