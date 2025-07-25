using Enums;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  아이템을 임시로 생성하는 스크립트입니다.
/// </summary>
public class ItemFactory : MonoBehaviour
{
    Inventory inventory;
    public List<Item> itemDatas = new List<Item>();

    /// <summary>
    /// 데이터를 넣습니다.
    /// </summary>
    public void AddItemData()
    {     
        inventory = GetComponent<Inventory>();

        // 임시 데이터 넣기
        OldDaggerItem oldDaggerItem = new OldDaggerItem();
        oldDaggerItem.LoadData();
        inventory.items.Add(CreateItemFromTemplate(oldDaggerItem, 1));
        //Logger.Log(oldDaggerItem.weaponName);
     
        // 임시 동료 아이템 넣기
        TempCompannionItem tempCompanionItem = new TempCompannionItem();
        tempCompanionItem.LoadData();

        inventory.items.Add(CreateItemFromTemplate(tempCompanionItem, 1));
        Logger.Log(tempCompanionItem.weaponName);

    }

    /// <summary>
    /// 임시 데이터입니다.
    /// </summary>
    /// <param name="template">낡은 단검</param>
    /// <param name="count">아이템 개수</param>
    /// <returns></returns>
    public Item CreateItemFromTemplate(object template, int count = 1)
    {
        if (template is OldDaggerItem dagger)
        {
            EquipItem oldDaggerItem = new EquipItem(
                "0",
            dagger.weaponName,
            dagger.weaponType,
            dagger.rank,
            dagger.description,
            dagger.weaponImage,
            0,
            1,
            99,
            EquipType.Weapon,
            dagger.attackPower,
            dagger.attackArea,
             5, 5, 0, 0, 0
        );
            return oldDaggerItem;
        }
        else if (template is TempCompannionItem item)
        {
            CompanionItem oldDaggerItem = new CompanionItem(
            "1",
            item.weaponName,
            item.weaponType,
            item.rank,
            item.description,
            item.weaponImage,
            1,
            1,
            1,
           CompanionItemType.Weapon,
           CompanionType.COMPANION_001,
            item.attackPowerBouns,
            item.defensePowerBouns
        );
            return oldDaggerItem;
        }

        return null;
    }
}
