using Enums;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class CompanionSlot : Slot
{
    public CompanionType companionType;

    public override void Init()
    {
        base.Init();
        itemImage = GetComponentsInChildren<Image>()
              .FirstOrDefault(img => img.name == "ItemImage");
        itemText = GetComponentsInChildren<TextMeshProUGUI>()
              .FirstOrDefault(img => img.name == "ItemText");
    }

    public override void SetItem(Item _item)
    {     
        if (_item is CompanionItem companionItem)
        {
            if (companionType != companionItem.CompanionType)
            {
                return;
            }
            if (_item != null)
            {
                this.item = _item;
                itemImage.enabled = true;
                itemImage.sprite = item.Image;
            }
            else
            {
                ClearSlot();
            }
        }
       
    }
    public override void ClearSlot()
    {
        this.item = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
    public override void UseItem()
    {
        if (item == null) { return; }
        companionEquipManager.UnEquip(item);

        ClearSlot();
    }
    // 슬롯 정보 업데이트
    public override void UpdateSlot()
    {
        if (item == null) { ClearSlot(); return; }
        itemImage.enabled = true;
        itemImage.sprite = item.Image;
    }
}
