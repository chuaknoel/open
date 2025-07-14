using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQuickSlot : Slot, ISkillSlot
{
    protected SkillData skill;
    public int slotNum;

    protected SkillExecutor executor;

    [SerializeField] protected Image skillImage;
    [SerializeField] private Image skillCoolTimeImage; 
    public string key;

    public SkillData GetSkill() => skill;

    public virtual void Init(int slotNum)
    {
        this.slotNum = slotNum;
        executor = GetComponent<SkillExecutor>();
        skillCoolTimeImage = transform.GetChild(1).GetComponent<Image>();   
    }

    public virtual void SetSkill(SkillData s)
    {
        if(s == null)
        {
            skill = null;
            return;
        }

        SetSkillData(s);
    }

    public virtual void SetSkillData(SkillData s)
    {
        skill = s;
        SetAnime(SkillManager.instance.FindSkill(skill.skillCode).skillAnimeClips);
    }

    public virtual void SetAnime(List<AnimationClip> skillAnime) { }
    
    public void UpdateSkill()
    {
        if (GetSkill() != null)
        {
            skillImage.enabled = true;
            skillImage.sprite = skill.skillImage;
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
        skillImage.sprite = null;
        skillImage.enabled = false;
    }
    // 스킬 사용
    public virtual void UseSkill()
    {
        if (GetSkill() == null)
        {
            Logger.Log($"Slot {slotNum} : 스킬이 없음");
            return;
        }
        if (executor.IsUseAble())
        {
            ExcuteSkill();
            StartCoroutine("SkillCoolTime");
            Logger.Log($"{skill.skillName} 스킬을 사용했습니다.");
        }
    }

    public virtual void ExcuteSkill() { }

    IEnumerator SkillCoolTime()
    {
        skillCoolTimeImage.gameObject.SetActive(true);
        skillCoolTimeImage.fillAmount = 1f;
        while (executor.remainingTime > 0)
        {
            skillCoolTimeImage.fillAmount = Mathf.Clamp01(executor.remainingTime / executor.skill.coolTime);
            yield return null;
        }
    }
}
