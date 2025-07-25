using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCompannionItem : MonoBehaviour
{
    [Header("기본 정보")]
    public string weaponID;
    public string weaponName;
    public Sprite weaponImage;

    [TextArea(3, 5)]
    public string description;
    public ItemType weaponType;
    public CompanionItemType equipType;
    public ItemGrade rank;

    [Header("스탯 정보")]
    public int attackPowerBouns;
    public int defensePowerBouns;

    // 이 스크립트가 씬에 로드될 때 자동으로 호출됩니다.
    void Awake()
    {
        // '낡은 단검'의 데이터를 변수에 직접 할당합니다.
        LoadData();
    }

    public void LoadData()
    {
        weaponID = "WEAPON_001";
        weaponName = "임시 동료 데이터";
        weaponImage = Resources.Load<Sprite>("Weapons/02");

        description = "임시 동료 데이터입니다.";
        weaponType = ItemType.CompanionItem;
        equipType = CompanionItemType.Weapon;
        rank = ItemGrade.Common;

        attackPowerBouns = 10;
        defensePowerBouns = 10;
    }
}
