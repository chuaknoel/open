using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionItem : EquipItem
{
    [SerializeField] private CompanionType companionType;
    [SerializeField] private CompanionItemType companionItemType;
   
    public CompanionType CompanionType => companionType;
    public CompanionItemType CompanionItemType => companionItemType;

    // 기본 생성자
    public CompanionItem() { }

    public CompanionItem(CompanionItem item) : base(item)
    {
        // 원래 CompanionItem이었던 Item을 캐스팅
        if (item is CompanionItem companion)
        {
            this.companionItemType = companion.companionItemType;
            this.companionType = companion.companionType;
            this.attackBonus = companion.attackBonus;
            this.defenseBonus = companion.defenseBonus;
        }
        else
        {
            Debug.LogWarning("Item을 CompanionItem으로 캐스팅할 수 없습니다.");
        }
    }

    public CompanionItem(
     string itemId,
     string itemName,
     ItemType type,
     ItemGrade itemGrade,
     string itemDescription,
     Sprite image,
     int inventoryIndex,
     int count,
     int maxCount,
     CompanionItemType companionItemType,
     CompanionType companionType,
     int attackBonus,
     int defenseBonus

) : base(itemId, itemName, type, itemDescription, image, inventoryIndex, count, maxCount)
    {
        this.companionItemType = companionItemType;
        this.companionType = companionType;
        this.itemGrade = itemGrade;
        this.attackBonus = attackBonus;
        this.defenseBonus = defenseBonus;
    }
    public override void Use()
    {
        base.Use();
    }
}
