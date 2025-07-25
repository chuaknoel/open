using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class CompanionUIInventory : UIInventory
{
    public override void Init()
    {    
        inventory = GetComponent<Inventory>();
    }
}
