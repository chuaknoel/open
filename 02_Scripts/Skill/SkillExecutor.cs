using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static Constants.AnimatorHash;

public class SkillExecutor : MonoBehaviour
{
    public SkillData skill;
    public IObjectPool<Skill> skillPool;
    public float remainingTime;
    public BaseCreature user;

    private void Update()
    {
        CheckCoolTime();
    }

    public void SetSkill(SkillData skill, IObjectPool<Skill> skillPool, BaseCreature user)
    {
        this.skill = skill;
        this.skillPool = skillPool;
        this.user = user;
    }

    public void CheckCoolTime()
    {
        if (remainingTime > 0f)
            remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0f);
    }

    public bool IsUseAble()
    {
        if (remainingTime > 0) return false;
        return user.GetStat<BaseStat>().UseMana(skill.cunsumeMana);
    }

    public void UseSkill()
    {
        remainingTime = skill.coolTime;
        skillPool.Get().ExecuteSkill();
    }
}
