using Enums;
using System.Collections.Generic;
using UnityEngine;
using static Constants.AnimatorHash;

public class PlayerSkillQuickSlot : SkillQuickSlot
{
    private Player player;
    public override void OnEnable()
    {

    }
    public override void Init(int slotIndex)
    {
        base.Init(slotIndex);
        this.slotIndex = slotIndex;
        executor = GetComponent<SkillExecutor>();
        player = PlayManager.instance.player;
    }

    public override void SetSkillData(SkillData s)
    {
        base.SetSkillData(s);
        executor.SetSkill(skill, player.skillPoolDictionary[skill.skillCode], player);
    }

    public override void SetAnime(List<AnimationClip> skillAnime)
    {
        player.overrideController["SkillButton0_E"] = skillAnime[0];
        player.overrideController["SkillButton0_N"] = skillAnime[1];
        player.overrideController["SkillButton0_S"] = skillAnime[2];
        player.overrideController["SkillButton0_W"] = skillAnime[3];
    }

    public override void ExcuteSkill()
    {
        base.ExcuteSkill();
        player.ChangeAnimation(SkillButtonHash, slotIndex);
        player.ChangeState(StateEnum.Skill);
        executor.UseSkill();
    }
}
