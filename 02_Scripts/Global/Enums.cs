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
}