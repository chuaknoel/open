using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillTempSlot : SkillSlot, ISkillSlot
{
    public virtual void Init(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public override void SetSkill(SkillData s)
    {
        if (s == null)
        {
            skill = null;
            return;
        }

        SetSkillData(s);
    }

    public virtual void SetSkillData(SkillData s)
    {
        skill = s;      
    }

    public override void UpdateSkill()
    {     
        if (GetSkill() != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = skill.skillImage;
        }
        else
        {
            ClearSlot();
        }   
    }
    // 슬롯 비우기
    public override void ClearSlot()
    {
        this.skill = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
    } 
}
