using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SearchTarget : MonoBehaviour
{
    public SearchTargetMode targetMode;
    public CircleCollider2D detectedCollider;
    public float searchRange;
    public LayerMask targetMask;

    public HashSet<BaseCreature> inRangeTargets = new HashSet<BaseCreature>();

    public BaseCreature currentTarget;

    public void Init(LayerMask targetMask)
    {
        detectedCollider ??= GetComponent<CircleCollider2D>();
        detectedCollider.radius = searchRange;
        this.targetMask = targetMask;
    }

    public BaseCreature FindTarget()
    {
        switch (targetMode)
        {
            case SearchTargetMode.Nearest:
                return FindNearestTarget();

            case SearchTargetMode.Strongest:
                return FindStrongestTarget();

            case SearchTargetMode.Weakest:
                return FindWeakestTarget();       
        }

        return null;
    }
    private BaseCreature FindNearestTarget() //감지중인 오브젝트 중 가장 가까운 오브젝트 검출
    {
        BaseCreature nearest = null;
        float minDis = float.MaxValue;

        foreach (var target in inRangeTargets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < minDis)
            {
                minDis = distance;
                nearest = target;
            }
        }
        return nearest;
    }

    private BaseCreature FindStrongestTarget()
    {
        BaseCreature strongest = null;
        float maxValue = 0;

        foreach (var target in inRangeTargets)
        {
            if(target is IParty party)
            {
                if (party.IsLeader)
                {
                    strongest = target;
                    return strongest;
                }
            }

            float targetHP = target.GetStat<BaseStat>().GetCurrentHealth();
            if (targetHP > maxValue)
            {
                maxValue = targetHP;
                strongest = target;
            }
        }
        
        return strongest;
    }

    private BaseCreature FindWeakestTarget()
    {
        BaseCreature weakest = null;
        float minValue = float.MaxValue;

        foreach (var target in inRangeTargets)
        {
            float targetHP = target.GetStat<BaseStat>().GetCurrentHealth();
            if (targetHP < minValue)
            {
                minValue = targetHP;
                weakest = target;
            }
        }

        return weakest;
    }

    public BaseCreature GetCurrentTarget()
    {
        if(currentTarget == null)
        {
            currentTarget = FindTarget();
        }
        return currentTarget;
    }

    public float CurrentTargetDis()
    {
        Vector3 targetDir = transform.position - currentTarget.transform.position;
        float targetBounds = currentTarget.GetHitBox().bounds.extents.x;
        return targetDir.magnitude - targetBounds;
    }

    public Vector2 CurrentTargetPos()
    {
        Vector3 posDir = (currentTarget.transform.position - transform.position).normalized;
        float targetBounds = currentTarget.GetHitBox().bounds.extents.x;
        return currentTarget.transform.position - (posDir * targetBounds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetMask) == 0) return;

        if (collision.TryGetComponent<BaseCreature>(out BaseCreature baseCreature))
        {
            inRangeTargets.Add(baseCreature);
            currentTarget = FindTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseCreature>(out BaseCreature baseCreature))
        {
            if (inRangeTargets.Contains(baseCreature))
            {
                inRangeTargets.Remove(baseCreature);
                if (currentTarget == baseCreature)
                {
                    currentTarget = null;
                }

                currentTarget ??= FindTarget();
            }
        }
    }
}
