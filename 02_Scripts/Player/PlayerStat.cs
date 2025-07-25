using Enums;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : BaseStat, IDamageable
{
    private Player player;
    //캐릭터 별 주스텟 추가

    //힘 민첩 마력  : 플레이어 전용
    [Header("STR Stat")]
    protected int baseStr;
    protected int addStr;
    protected int totalStr;

    [Header("DEX Stat")]
    protected int baseDex;
    protected int addDex;
    protected int totalDex;

    [Header("INT Stat")]
    protected int baseInt;
    protected int addInt;
    protected int totalInt;

    //항마력 : 악신의 힘에 대한 저항도 : 주인공 전용
    [Header("Sanctity Stat")]
    protected int baseSanctity;

    [Header("Level")]
    protected int level;
    protected int exp;
    protected int nextLevelExp;

    public event UnityAction OnDamaged;
    public event UnityAction OnDeath;

    public override void Init(BaseCreature player)
    {
        base.Init(player);
        this.player = player as Player;

        CharacterData data = this.player.GetData();

        baseAttack = data.baseAttack;
        baseDefence = data.baseDefence;

        baseEvasionRate = data.baseEvasionRate;

        baseHealth = data.baseHealth;
        currentHealth = data.currentHealth;

        baseMana = data.baseMana;
        currentMana = data.currentMana;

        baseMoveSpeed = data.baseMoveSpeed;
    }

    public void TakeDamage(float damage)
    {
        Logger.Log("Take Damage");
        currentHealth -= damage;
        //UIManager.Instance.OnChangeHealth?.Invoke(currentHealth , GetTotalHealth());

        if (currentHealth > 0)
        {
            player.Controller.ChangeState(StateEnum.Damaged);
            OnDamaged?.Invoke();
        }
        else if (currentHealth <= 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        base.Death();
        if (!isDeath)
        {
            isDeath = true;
            player.Controller.ChangeState(StateEnum.Death);
            OnDeath?.Invoke();
        }
    }

    public void AddExp(int expAmonut)
    {
        exp += expAmonut;
        while (exp >= nextLevelExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        exp -= nextLevelExp;
        level++;
        Logger.Log("레벨업");
    }

    //장비 능력치 반영
    public void EquipItem(Item item)
    {
        EquipItem equipItem = (EquipItem)item;
        addAttack += equipItem.AttackBonus;
        addDefence += equipItem.DefenseBonus;
        addHealth += equipItem.HpBonus;
        addMana += equipItem.MpBonus;
        addEvasionRate += equipItem.EvasionRate;

        Logger.Log("장비 능력치 반영");
    }

    public void UnequipItem(Item item)
    {
        EquipItem equipItem = (EquipItem)item;  
        addAttack -= equipItem.AttackBonus;
        addDefence -= equipItem.DefenseBonus;
        addHealth -= equipItem.HpBonus;
        addMana -= equipItem.MpBonus;
        addEvasionRate -= equipItem.EvasionRate;
    }
}
