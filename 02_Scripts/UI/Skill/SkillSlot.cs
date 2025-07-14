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

    public SkillData skill;
    public SkillData GetSkill() => skill;
    public void SetSkill(SkillData s) => skill = s;

    private void OnEnable()
    {
        skillManager = SkillManager.instance;
      
        skill = skillManager.skillDatas[0];

        SkillDrag skillDrag = GetComponent<SkillDrag>();
        dragImage = skillDrag.dragItemImage;
        //UpdateSlot();
    }

    public override void UseItem()
    {
        Logger.Log("스킬 슬롯 클릭");
        tempSlotManager.CreateSkillTempSlot(this);
    }
}
