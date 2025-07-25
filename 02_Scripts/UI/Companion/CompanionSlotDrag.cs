using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSlotDrag : BaseDrag<CompanionSkillSlot, SkillData>
{
    protected override SkillData GetData()
    {
        throw new System.NotImplementedException();
    }

    protected override Sprite GetSprite(SkillData data)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDropCompleted(SkillData droppedData, CompanionSkillSlot originSlot)
    {
        throw new System.NotImplementedException();
    }

    protected override bool SlotIsOut()
    {
        return true; 
    }
    }
