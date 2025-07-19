using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSkillSlot : SkillQuickSlot
{
    public override void OnEnable()
    {

    }
    // 스킬 사용
    public override void UseSkill()
    {
        //if (GetSkill() == null)
        //{
        //    Logger.Log($"Slot {slotNum} : 스킬이 없음");
        //    return;
        //}
        //if (executor.IsUseAble())
        //{
        //    ExcuteSkill();
        //    StartCoroutine("SkillCoolTime");
        //    Logger.Log($"{skill.skillName} 스킬을 사용했습니다.");
        //}
    }
}
