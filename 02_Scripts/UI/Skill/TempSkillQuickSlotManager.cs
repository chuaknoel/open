using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSkillQuickSlotManager : MonoBehaviour
{
    [HideInInspector] public SkillQuickSlotManager SkillQuickSlotManager;
    [SerializeField] private SkillQuickSlot[] skillQuickSlots = new SkillQuickSlot[4];

    public void SetTempSkillQuickSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if(SkillQuickSlotManager.skillSlots[i].GetSkill() != null)
            {
                skillQuickSlots[i].SetSkill(SkillQuickSlotManager.skillSlots[i].GetSkill());
                skillQuickSlots[i].UpdateSkill();   
            }
        }
    }
}
