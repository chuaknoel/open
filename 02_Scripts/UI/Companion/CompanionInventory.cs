using Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CompanionInventory : Inventory
{
    public Dictionary<CompanionItemType, Item> companionItems = new();

   public void Init()
    {
        slots.AddRange(GetComponentsInChildren<CompanionSlot>());
    }
}
