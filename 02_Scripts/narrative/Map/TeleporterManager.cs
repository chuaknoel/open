using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public bool isMoving { get; private set; }

    // [추가] 시네머신 카메라의 Confiner 컴포넌트를 저장해 둘 변수
    private CinemachineConfiner2D cameraConfiner;
    // [추가] 텔레포터로부터 미리 전달받은 카메라 경계를 임시 저장할 변수
    private PolygonCollider2D preparedBoundary;

    public void Init()
    {
        // 시작할 때 자신의 자식 오브젝트들에서 모든 Teleporter 컴포넌트를 찾아 리스트에 자동으로 등록합니다.
        // 이렇게 하면 Teleporter가 스스로를 등록할 필요 없이 매니저가 모든 것을 파악할 수 있습니다.
        GetComponentsInChildren<Teleporter>(true, managedTeleporters); // 비활성 오브젝트도 포함하여 찾습니다.

        foreach (Teleporter teleporter in managedTeleporters)
        {
            teleporter.Init(this, targetLayer);
        }

        // [추가] 시작할 때 씬에 있는 CinemachineConfiner2D를 한 번만 찾아 저장합니다.
        cameraConfiner = FindObjectOfType<CinemachineConfiner2D>();
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

        // [추가] 플레이어 위치를 바꾸기 전에, 미리 전달받은 경계로 카메라를 업데이트합니다.
        UpdateCameraBoundary();

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


    // [추가] 텔레포터가 호출하여, 다음에 사용할 카메라 경계를 미리 준비시키는 메서드
    public void PrepareCameraBoundary(PolygonCollider2D boundary)
    {
        this.preparedBoundary = boundary;
    }

    // [추가] 준비된 경계로 실제 카메라를 업데이트하는 메서드
    private void UpdateCameraBoundary()
    {
        if (cameraConfiner == null) return;

        if (preparedBoundary != null)
        {
            cameraConfiner.m_BoundingShape2D = preparedBoundary;
        }
    }

}
