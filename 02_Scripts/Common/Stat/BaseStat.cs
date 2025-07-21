using Unity.Collections;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    //공격력 방어력 회피율 체력 마나 이동속도 : 모든 개체 전용
    [Header("Attack Stat")]
    protected float baseAttack;
    protected float addAttack;
    [SerializeField] protected float attackRange;
    protected float addAttackRagne;

    [Header("Defence Stat")]
    protected float baseDefence;
    protected float addDefence;

    [Header("Evasion Stat")]
    protected float baseEvasionRate;
    protected float addEvasionRate;

    [Header("Health Stat")]
    [SerializeField] protected float baseHealth;     //기본 체력
    [SerializeField] protected float currentHealth;  //남은 체력
    protected float addHealth;                       //추가 체력
    protected float maxHealth => GetMaxHealth();     //최대 체력

    [Header("Mana Stat")]
    [SerializeField] protected float baseMana;
    [SerializeField] protected float currentMana;
    protected float addMana;
    protected float maxMana => GetMaxMana();

    [Header("Move Stat")]
    protected float baseMoveSpeed;
    protected float addMoveSpeed;
    protected float totalMoveSpeed => GetTotalMoveSpeed();

    [HideInInspector] public bool isDeath;

    [HideInInspector] public bool isInvincible;

    public virtual void Init(BaseCreature owner)   //초기화 부문
    {
        isDeath = false;
        isInvincible = false;
    }

    public virtual float GetTotalAttack()
    {
        return baseAttack + addAttack;
    }

    public virtual float GetAttackRange()
    {
        return attackRange + addAttackRagne;
    }

    public virtual float GetTotalDefence()
    {
        return baseDefence + addDefence;
    }

    public virtual float GetTotalEvasion()
    {
        return baseEvasionRate + addEvasionRate;
    }

    public virtual float GetMaxHealth()              //최대 체력 구하기
    {
        return baseHealth + addHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual float GetMaxMana()
    {
        return baseMana + addMana;
    }

    public float GetCurrentMana()
    {
        return currentHealth;
    }

    public virtual float GetTotalMoveSpeed()
    {
        return baseMoveSpeed + addMoveSpeed;
    }

    public virtual void RecoverHealth(float heal)    //체력 회복 메서드
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);    
    }

    public virtual void RecoverMana(float mana)
    {
        currentMana = Mathf.Min(currentMana + mana, maxMana);
    }

    public virtual bool UseMana(float consumAmount)
    {
        if(currentMana < consumAmount)
        {
            Logger.Log("마나가 부족합니다.");
            return false;
        }

        currentMana -= consumAmount;
        return true;
    }

    public virtual void Death() { }
}
