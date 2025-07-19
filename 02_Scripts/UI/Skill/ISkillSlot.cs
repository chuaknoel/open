using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillSlot 
{
    public SkillData GetSkill();
    public void SetSkill(SkillData skill);
    public void UpdateSkill();
}
