using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot :Slot , ISkillSlot
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private SkillTempSlotManager tempSlotManager;

    private Image dragImage;

    protected SkillData skill;
    public virtual SkillData GetSkill() => skill;
    public virtual void SetSkill(SkillData s) => skill = s;

    public virtual void OnEnable()
    {
        skillManager = SkillManager.instance;   
        skill = skillManager.skillDatas[0];

        SkillDrag skillDrag = GetComponent<SkillDrag>();
        dragImage = skillDrag.dragItemImage;
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
