using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

[CreateAssetMenu(fileName = "NewItem", menuName = "EquipItem")]
public class EquipItem : Item
{
    [Header("EquipInfo")]
    [SerializeField] private EquipType equipType;
    [SerializeField] protected ItemGrade itemGrade;
    [SerializeField] private int attackPower;
    [SerializeField] private Vector2 attackArea;
    
    [Header("EquipAbilityData")]
    [SerializeField] protected int attackBonus;
    [SerializeField] protected int defenseBonus;
    [SerializeField] private int hpBonus;
    [SerializeField] private int mpBonus;
    [SerializeField] private int evasionRate; // 회피율
    [SerializeField] private bool equipped; // ?λ퉬 ?μ갑 ?щ?


    public EquipType EquipType => equipType;
    public ItemGrade ItemGrade => itemGrade;
    public int AttackPower => attackPower;
    public Vector2 AttackArea => attackArea;
    public int AttackBonus => attackBonus;
    public int DefenseBonus => defenseBonus;
    public int HpBonus => hpBonus;
    public int MpBonus => mpBonus;
    public int EvasionRate => evasionRate;
    public bool Equipped => equipped;

    // 기본 생성자
    public EquipItem() { }

    public EquipItem(EquipItem item) : base(item)
    {
        if (item is EquipItem equip)
        {
            this.equipType = equip.equipType;
            this.itemGrade = equip.itemGrade;
            this.attackPower = equip.attackPower;
            this.attackArea = equip.attackArea;
            this.attackBonus = equip.attackBonus;
            this.defenseBonus = equip.defenseBonus;
            this.hpBonus = equip.hpBonus;
            this.mpBonus = equip.mpBonus;
            this.evasionRate = equip.evasionRate;
            this.equipped = equip.equipped;
        }
        else
        {
            Debug.LogWarning("Item을 EquipItem으로 캐스팅할 수 없습니다.");
        }
    }

    public EquipItem(
        string itemId,
        string itemName,
        ItemType type,
        string itemGrade,
        string itemDescription,
        Sprite image,
        int inventoryIndex,
        int count,
        int maxCount,
        EquipType equipType,
        int attackPower,
        Vector2 attackArea,
        int attackBonus,
        int defenseBonus,
        int hpBonus,
        int mpBonus,
        int evasionRate
    ) : base(itemId, itemName, type, itemDescription, image, inventoryIndex, count, maxCount)
    {
        this.equipType = equipType;
        this.itemGrade = ParseItemGrade(itemGrade);
        this.attackPower = attackPower;
        this.attackArea = attackArea;
        this.attackBonus = attackBonus;
        this.defenseBonus = defenseBonus;
        this.hpBonus = hpBonus;
        this.mpBonus = mpBonus;
        this.evasionRate = evasionRate;
    }

    public EquipItem(string itemId, string itemName, ItemType type, string itemDescription, Sprite image, int count, int maxCount, int inventoryIndex) : base(itemId, itemName, type, itemDescription, image, count, maxCount, inventoryIndex)
    {
    }
    public static ItemGrade ParseItemGrade(string gradeKorean)
    {
        Dictionary<string, ItemGrade> gradeMap = new Dictionary<string, ItemGrade>()
    {
        { "일반", ItemGrade.Common },
        { "고급", ItemGrade.Uncommon },
        { "희귀", ItemGrade.Rare },
        { "영웅", ItemGrade.Epic },
        { "전설", ItemGrade.Legendary }
    };

        if (gradeMap.TryGetValue(gradeKorean.Trim(), out ItemGrade result))
        {
            return result;
        }

        Debug.LogError($"ItemGrade 변환 실패: {gradeKorean}");
        return ItemGrade.Common; // 기본값으로 Common을 반환하거나 예외 처리 가능
    }
    public override void Use()
    {  
        base.Use();
    }
}
