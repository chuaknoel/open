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

    public EquipItem(
        string itemId,
       string itemName,
       ItemType type,
       ItemGrade itemGrade,
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
   ) : base(itemId,itemName, type, itemDescription, image, inventoryIndex, count, maxCount)
    {
        this.equipType = equipType;
        this.itemGrade = itemGrade;
        this.attackPower = attackPower;
        this.attackArea = attackArea;
        this.attackBonus = attackBonus;
        this.defenseBonus = defenseBonus;
        this.hpBonus = hpBonus;
        this.mpBonus = mpBonus;
        this.evasionRate = evasionRate;
    }

    public EquipItem(string itemId, string itemName, ItemType type, string itemDescription, Sprite image, int inventoryIndex, int count, int maxCount) : base(itemId,itemName, type, itemDescription, image, inventoryIndex, count, maxCount)
    {
    }

    public override void Use()
    {  
        base.Use();
    }
}
