using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private UIInventory uiInventory;

    public QuickSlot[] slots;

    private void Start()
    {
        foreach (var slot in slots)
        {
            slot.Init(inventory, uiInventory);
        }
    }
    public void AssignItemToSlot(int index, Item item)
    {
        if(item.Type == ItemType.Consume)
        {
            slots[index].SetItem(item);
        }
    }
    public void OnQuickSlotPressed(InputAction.CallbackContext context)
    {   
        if(context.performed)
        {
            string keyName = context.control.name;
            UseSlot(int.Parse(keyName) - 1);
        }
    }
    private void UseSlot(int index)
    {
        slots[index].UseItem();
    }
}
