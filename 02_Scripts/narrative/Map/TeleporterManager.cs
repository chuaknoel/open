using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬에 존재하는 모든 텔레포터를 중앙에서 관리하는 매니저 클래스.
/// 텔레포터 목록을 직접 제어하고 상태를 강제로 초기화하는 기능이 포함되어 있습니다.
/// </summary>
public class TeleporterManager : MonoBehaviour
{
    // 관리할 텔레포터들의 리스트
    private readonly List<Teleporter> managedTeleporters = new List<Teleporter>();

    // 텔레포트 후 재사용 대기시간에 있는 대상을 추적하는 HashSet
    private readonly HashSet<Transform> targetsOnCooldown = new HashSet<Transform>();

    public LayerMask targetLayer;

    private bool isMoving;

    public void Init()
    {
        // 시작할 때 자신의 자식 오브젝트들에서 모든 Teleporter 컴포넌트를 찾아 리스트에 자동으로 등록합니다.
        // 이렇게 하면 Teleporter가 스스로를 등록할 필요 없이 매니저가 모든 것을 파악할 수 있습니다.
        GetComponentsInChildren<Teleporter>(true, managedTeleporters); // 비활성 오브젝트도 포함하여 찾습니다.

        foreach (Teleporter teleporter in managedTeleporters)
        {
            teleporter.Init(this, targetLayer);
        }
    }

    /// <summary>
    /// 텔레포터가 대상을 이동시켜달라고 요청하는 메서드.
    /// </summary>
    public void RequestTeleport(Transform target, Teleporter destination)
    {
        if (targetsOnCooldown.Contains(target)) return;
        if (destination == null)
        {
            Logger.LogError($"[TeleporterManager] 목적지가 유효하지 않습니다!");
            return;
        }

        isMoving = true;
        target.position = destination.transform.position;
        targetsOnCooldown.Add(target);
    }

    /// <summary>
    /// 대상이 텔레포터 영역을 벗어났을 때 호출되어 쿨다운을 해제하는 메서드.
    /// </summary>
    public void ClearCooldownForTarget(Transform target)
    {
        targetsOnCooldown.Remove(target);
    }

    public void EndTeleport()
    {
        isMoving = false;
    }

    public bool MoveCheck()
    {
        return isMoving;
    }

    /// <summary>
    /// [리뷰 반영] 관리하는 모든 텔레포터 게임 오브젝트를 활성화하거나 비활성화합니다.
    /// PlayManager가 씬을 전환할 때 호출하여 텔레포터의 동작을 일괄 제어할 수 있습니다.
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void ToggleAllTeleporters(bool isActive)
    {
        foreach (var teleporter in managedTeleporters)
        {
            if (teleporter != null)
            {
                teleporter.gameObject.SetActive(isActive);
            }
        }
    }

    /// <summary>
    /// [리뷰 반영] 모든 텔레포트 관련 상태를 강제로 초기화합니다.
    /// 씬이 로드되거나, 예외 상황에서 상태가 꼬였을 때 호출하여 시스템을 안정화시킵니다.
    /// </summary>
    public void ResetAllStates()
    {
        targetsOnCooldown.Clear();
    }
}
