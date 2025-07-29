using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class CompanionUIInventory : UIInventory
{
    public override async Task Init()
    {    
       inventory = GetComponent<Inventory>();
    }
}
