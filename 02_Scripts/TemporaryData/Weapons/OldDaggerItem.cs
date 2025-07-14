using UnityEngine;
/// <summary>
/// '낡은 단검(WEAPON_001)' 하나의 데이터만 담고 있는 테스트용 스크립트입니다.
/// 이 컴포넌트가 붙은 게임 오브젝트는 그 즉시 '낡은 단검' 아이템이 됩니다.
/// </summary>
public class OldDaggerItem 
{
    [Header("기본 정보")]
    public string weaponID;
    public string weaponName;
    public Sprite weaponImage;

    [TextArea(3, 5)]
    public string description;
    public ItemType weaponType;
    public EquipType equipType;
    public ItemGrade rank;

    [Header("스탯 정보")]
    public int attackPower;
    public Vector2 attackArea;

    // 이 스크립트가 씬에 로드될 때 자동으로 호출됩니다.
    void Awake()
    {
        // '낡은 단검'의 데이터를 변수에 직접 할당합니다.
        LoadData();
    }

    public void LoadData()
    {
        weaponID = "WEAPON_001";
        weaponName = "낡은 단검";
        weaponImage = Resources.Load<Sprite>("Weapons/01");

        description = "어디서나 볼 수 있는 평범한 단검.\n호신용으로나 쓸만하다.";
        weaponType = ItemType.Equip;
        equipType = EquipType.Weapon;
        rank = ItemGrade.Common;

        attackPower = 10;
        attackArea = new Vector2(0.8f, 1.5f);
    }
}