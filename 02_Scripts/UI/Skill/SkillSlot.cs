using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot :Slot , ISkillSlot
{
    private SkillManager skillManager;
    private SkillTempSlotManager tempSlotManager;
   // private Image dragImage;
    
    protected SkillData skill;
    public virtual SkillData GetSkill() => skill;
    public virtual void SetSkill(SkillData s) => skill = s;

    public virtual void Init( SkillTempSlotManager _tempSlotManager)
    {
        skillManager = SkillManager.Instance;   
        skill = skillManager.skillDatas[0];

        //dragImage = GetComponent<SkillDrag>().dragItemImage;

        tempSlotManager = _tempSlotManager;
    }

    public override void UseItem()
    {
        tempSlotManager.CreateSkillTempSlot(this);
    }

    public virtual void UpdateSkill()
    {
        if (GetSkill() != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = skill.skillImage;
        }
    }
}
