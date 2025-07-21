using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillQuickSlot : SkillSlot, ISkillSlot
{
    protected SkillExecutor executor;

    [SerializeField] protected Image skillImage;
    [SerializeField] private Image skillCoolTimeImage; 
    public string key;
    private Coroutine coroutine;

    public virtual void Init(int slotIndex)
    {
        this.slotIndex = slotIndex;
        executor = GetComponent<SkillExecutor>();
        skillCoolTimeImage = transform.GetChild(1).GetComponent<Image>();   
    }

    public override void SetSkill(SkillData s)
    {
        if(s == null)
        {
            skill = null;
            Logger.Log(this.gameObject.name);
            return;
        }

        SetSkillData(s);
    }

    public virtual void SetSkillData(SkillData s)
    {
        skill = s;
        SetAnime(SkillManager.Instance.FindSkill(skill.skillCode).skillAnimeClips);
    }

    public virtual void SetAnime(List<AnimationClip> skillAnime) { }
    
    public override void UpdateSkill()
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
        StopSkillCoolTime();
    }
    // 스킬 사용
    public virtual void UseSkill()
    {
        if (GetSkill() == null)
        {
            Logger.Log($"Slot {slotIndex} : 스킬이 없음");
            return;
        }
        if (executor.IsUseAble())
        {
            executor.remainingTime = executor.skill.coolTime;
            ExcuteSkill();
            ShowSkillCoolTime();

            Logger.Log($"{skill.skillName} 스킬을 사용했습니다.");
        }
    }

    public void ShowSkillCoolTime()
    {
        StopSkillCoolTime();

        // 새로 코루틴 실행
        coroutine = StartCoroutine(SkillCoolTime());
    }
    public virtual void ExcuteSkill() { }

    IEnumerator SkillCoolTime()
    {
        if (executor.remainingTime > 0)
        {
            skillCoolTimeImage.gameObject.SetActive(true);

            while (executor.remainingTime > 0)
            {
                skillCoolTimeImage.fillAmount = executor.remainingTime / executor.skill.coolTime;
                yield return null;
            }
        }

        skillCoolTimeImage.fillAmount = 0f;
        skillCoolTimeImage.gameObject.SetActive(false);
    }
    public void StopSkillCoolTime()
    {
        // 이미 실행 중이면 먼저 멈춤
        if (coroutine != null)
        {
            skillCoolTimeImage.fillAmount = 0f;
            skillCoolTimeImage.gameObject.SetActive(false);
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
