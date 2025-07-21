using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempSkillQuickSlotManager : MonoBehaviour
{
    [HideInInspector] public SkillQuickSlotManager SkillQuickSlotManager;
   public SkillTempSlot[] skillTempSlots = new SkillTempSlot[4];
  
    public void SetTempSkillQuickSlot()
    {
        for (int i = 0; i < skillTempSlots.Length; i++)
        {
            if(SkillQuickSlotManager.skillSlots[i].GetSkill() != null)
            {
                skillTempSlots[i].SetSkill(SkillQuickSlotManager.skillSlots[i].GetSkill());
                skillTempSlots[i].UpdateSkill();
            }
            else
            {
                skillTempSlots[i].SetSkill(null);
                skillTempSlots[i].UpdateSkill();
            }

        }
    }
    //public void ChangeTempSkillSlotText()
    //{
    //    for (int i = 0; i < skillTempSlots.Length; i++)
    //    {
    //        skillTempSlots[i].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
    //            = SkillQuickSlotManager.playerSkillSlotText[i].text;
    //    }
    //}
}
