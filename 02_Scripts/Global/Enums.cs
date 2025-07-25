namespace Enums
{
    public enum StateEnum //크리처들의 상태
    {
        Idle,
        Move,
        Dash,
        Attack,
        Damaged,
        Chase,
        Skill,
        Death
    }

    public enum LookDirection //크리처들의 바라보는 방향
    {
        East = 0,
        North = 1,
        West = 2,
        South = 3,
        None
    }

    public enum ActionKeyType //인풋액션 키 타입
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Dash,
        Attack,
        Interaction,
        Inventory,
        Status,
        Menu,
        SkillButton0,
        SkillButton1,
        SkillButton2,
        SkillButton3,
        SkillButton4,
        SkillButton5,
        SkillButton6,
        SkillButton7,
    }

    public enum SearchTargetMode
    {
        Nearest,
        Strongest,
        Weakest,
        Random,
    }

    public enum PartyRole
    {
        Tanker,
        Healer,
        Dealer,
        Support
    }
    public enum EnemyEnum
    {
        DeathKnight,
    }

    public enum AttackType
    {
        Melee,
        Range,
    }
    
    public enum SkillType
    {

    }

    public enum SkillEffectType
    {

    }

    public enum SkillTargetType
    {

    }

    public enum SKillDamageType
    {

    }

    public enum SoundType
    {
        BGM,
        SFX,
        Voice,
        Ambient,
        None
    }

    public enum AudioType
    {
        Static,
        Dynamic
    }

    public enum SettingTab
    {
        Sound,
        Key,
        Interface
    }

    public enum ItemType
    {
        None, // 기타
        Consume, // 소모품
        Equip, // 장비 아이템
        Important, // 중요 아이템(퀘스트,열쇠,문서 등등)
        Collectibles, // 수집품,
        CompanionItem // 동료 아이템
    }
    public enum ItemGrade
    {
        Common, // 일반
        Uncommon, //고급
        Rare, // 희귀
        Epic, // 영웅
        Legendary // 전설
    }
    public enum EquipType
    {
        Helmet,
        Armor,
        Weapon,
        Shoes,
    }
    public enum CompanionType // 동료 타입
    {
        COMPANION_001,
        COMPANION_002,
        COMPANION_003,
        COMPANION_004
    }
    public enum CompanionItemType // 동료 아이템
    {
        Weapon,
        Armor,
    }
}