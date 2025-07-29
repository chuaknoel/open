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
        itemImage = transform.GetChild(0).GetComponent<Image>();
        tempSlotManager = _tempSlotManager;
    }

    public override void UseItem()
    {
        if (skill.isLock) 
        {
            return; 
        }
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
