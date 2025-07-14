using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<int> treeData = new List<int>();

    public Dictionary<int, Skill> skillTree = new Dictionary<int, Skill>();

    public void Init()
    {
        foreach (int tree in treeData)
        {
            skillTree[tree] = SkillManager.instance.FindSkill(tree);
        }
    }

    public Skill GetSkill(int skillCode)
    {
        return skillTree[skillCode];
    }
}
