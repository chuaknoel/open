using Enums;
using System.Collections.Generic;
using UnityEngine;
using static Constants.AnimatorHash;

public class PlayerSkillQuickSlot : SkillQuickSlot
{
    private Player player;
    protected Dictionary<int, string> nodeNames = new Dictionary<int, string>();

    public override void Init(int slotIndex)
    {
        base.Init(slotIndex);
        this.slotIndex = slotIndex;

        nodeNames[0] = $"SkillButton{slotIndex}_E";
        nodeNames[1] = $"SkillButton{slotIndex}_N";
        nodeNames[2] = $"SkillButton{slotIndex}_S";
        nodeNames[3] = $"SkillButton{slotIndex}_W";

        executor = GetComponent<SkillExecutor>();
        player = PlayManager.Instance.player;
    }

    public override void SetSkillData(SkillData s)
    {
        base.SetSkillData(s);
        executor.SetSkill(skill, player.skillPoolDictionary[skill.skillCode], player);
    }

    public override void SetAnime(List<AnimationClip> skillAnime)
    {
        for (int i = 0; i < nodeNames.Count; i++)
        {
            player.overrideController[nodeNames[i]] = skillAnime[0];
        }
    }

    public override void ExcuteSkill()
    {
        base.ExcuteSkill();
        player.ChangeAnimation(SkillButtonHash, slotIndex);
        player.ChangeState(StateEnum.Skill);
        executor.UseSkill();
    }
}
