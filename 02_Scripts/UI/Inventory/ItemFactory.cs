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

    }

    /// <summary>
    /// 임시 데이터입니다.
    /// </summary>
    /// <param name="template">낡은 단검</param>
    /// <param name="count">아이템 개수</param>
    /// <returns></returns>
    public EquipItem CreateItemFromTemplate(OldDaggerItem template, int count = 1)
    {
        EquipItem oldDaggerItem = new EquipItem(
            template.weaponName,
            template.weaponType,
            template.rank,
            template.description,
            template.weaponImage,
            0,
            1,
            99,
            EquipType.Weapon,       
            template.attackPower,
            template.attackArea,
             5,5,0,0,0
        );
        return oldDaggerItem;
    }
}
