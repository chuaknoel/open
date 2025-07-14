using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Skill : MonoBehaviour
{
    public SkillData skillData;
    public List<AnimationClip> skillAnimeClips;
    public BaseCreature owner;
    public IObjectPool<Skill> connectPool;

    public float endTime;
    public List<int> actionNode;
    protected List<float> nodeToTime = new List<float>();

    public HashSet<IDamageable> inRangeTargets = new HashSet<IDamageable>();
    public LayerMask targetMask;

    public virtual void Init(SkillData skillData, BaseCreature owner, IObjectPool<Skill> connectPool)
    {
        //this.skillData = skillData;
        this.owner = owner;
        this.connectPool = connectPool;
        targetMask = owner.targetMask;

        endTime = skillAnimeClips[0].length;

        float frameRate = skillAnimeClips[0].frameRate;

        foreach (var node in actionNode)
        {
            nodeToTime.Add(node / frameRate);
        }
    }

    public virtual void ExecuteSkill() { }
}
