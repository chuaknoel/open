using Enums;
using Palmmedia.ReportGenerator.Core.Logging;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///  아이템을 임시로 생성하는 스크립트입니다.
/// </summary>
public class ItemFactory : MonoBehaviour
{
    Inventory inventory;
    public List<Item> itemDatas = new List<Item>();
    public List<Sprite> itemImageDatas = new List<Sprite>();
    DataManager dataManager;
    /// <summary>
    /// 데이터를 넣습니다.
    /// </summary>
    public void AddItemData()
    {     
        inventory = GetComponent<Inventory>();
        dataManager = DataManager.Instance;

        // 초기 무기 데이터 넣기
        inventory.items.Add(CreateItemFromTemplate());

     
        // 임시 동료 아이템 넣기
        //TempCompannionItem tempCompanionItem = new TempCompannionItem();
        //tempCompanionItem.LoadData();
        //inventory.items.Add(CreateItemFromTemplate(tempCompanionItem));
       

    }

    /// <summary>
    /// 초기 무기 데이터입니다.
    /// </summary>
    /// <param name="template">낡은 단검</param>
    /// <param name="count">아이템 개수</param>
    /// <returns></returns>
    public Item CreateItemFromTemplate()
    {
        EquipItem oldDaggerItem = new EquipItem(
         dataManager.WeaponDB["WEAPON_001"].ID,
         dataManager.WeaponDB["WEAPON_001"].Name,
        ItemType.Equip,
        dataManager.WeaponDB["WEAPON_001"].Rank,
        dataManager.WeaponDB["WEAPON_001"].Description,
        itemImageDatas[0],
        0,
        1,
        99,
        EquipType.Weapon,
        dataManager.WeaponDB["WEAPON_001"].Atk,
        dataManager.WeaponDB["WEAPON_001"].Area,
         5, 5, 0, 0, 0
             );
       
            return oldDaggerItem;
       // }

        // if (template is TempCompannionItem item)
        //{
        //    CompanionItem oldDaggerItem = new CompanionItem(
        //    "1",
        //    item.weaponName,
        //    item.weaponType,
        //    item.rank,
        //    item.description,
        //    item.weaponImage,
        //    1,
        //    1,
        //    1,
        //   CompanionItemType.Weapon,
        //   CompanionType.COMPANION_001,
        //    item.attackPowerBouns,
        //    item.defensePowerBouns
        //);
        //    return oldDaggerItem;
        //}

        //return null;
    }
}
