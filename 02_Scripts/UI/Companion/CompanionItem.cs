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
