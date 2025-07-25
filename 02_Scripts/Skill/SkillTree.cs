using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<int> treeData = new List<int>();

    public Dictionary<int, Skill> skillTree = new Dictionary<int, Skill>();

    public void Init(List<int> skillData)
    {
        treeData = skillData;

        foreach (int tree in treeData)
        {
            Skill skill = SkillManager.Instance.FindSkill(tree);
            if (skill == null) continue;
            skillTree[tree] = skill;
        }
    }

    public Skill GetSkill(int skillCode)
    {
        return skillTree[skillCode];
    }
}
