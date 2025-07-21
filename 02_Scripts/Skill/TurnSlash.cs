using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class TurnSlash : Skill
{
    public CircleCollider2D attackArea;

    public override void Init(SkillData skillData, BaseCreature owner, IObjectPool<Skill> connectPool)
    {
        base.Init(skillData, owner, connectPool);
        attackArea.radius = skillData.range;
        attackArea.enabled = false;
    }
   
    public override void ExecuteSkill()
    {
        base.ExecuteSkill();
        //Logger.Log("스킬 실행");
        transform.position = owner.transform.position;
        StartCoroutine(SkillProcess());
    }

    private IEnumerator SkillProcess()
    {
        float nextActionTime = nodeToTime[0];
        
        yield return new WaitForSeconds(nextActionTime);

        attackArea.enabled = true;

        nextActionTime = endTime - nextActionTime;

        yield return new WaitForSeconds(nextActionTime);

        EndSkill();
    }

    private void EndSkill()
    {
        inRangeTargets.Clear();
        attackArea.enabled = false;
        connectPool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetMask) == 0) return;

        if (collision.TryGetComponent<IDamageable>(out IDamageable target))
        {
            if (!inRangeTargets.Contains(target))
            {
                inRangeTargets.Add(target);
                Logger.Log($"{skillData.amount} {owner.GetStat<BaseStat>().GetTotalAttack()}");
                target.TakeDamage(skillData.amount + owner.GetStat<BaseStat>().GetTotalAttack());
            }
        }
    }
}
