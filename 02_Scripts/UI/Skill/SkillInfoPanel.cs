using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] protected TextMeshProUGUI skillDescription;

    public void ShowText(SkillData skill)
    {
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
    }
}
