using UnityEngine;
using Cinemachine;

/// <summary>
/// 텔레포트 지점 역할만 수행하는 매우 가벼운 컴포넌트.
/// 자신의 부모 계층에 있는 TeleporterManager와 통신합니다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Teleporter : MonoBehaviour
{
    [Header("텔레포터 설정")]
    [Tooltip("이동할 목적지 텔레포터 오브젝트")]
    [SerializeField] private Teleporter destinationTeleporter;

    [Header("카메라 경계 설정")]
    [SerializeField] private PolygonCollider2D destinationBoundary;

    [Header("대상 설정")]
    [Tooltip("텔레포터와 상호작용할 오브젝트의 레이어를 선택합니다.")]
    [SerializeField] private LayerMask targetLayer;

    // 자신이 속한 TeleporterManager에 대한 참조
    private TeleporterManager teleporterManager;


    public void Init(TeleporterManager teleporterManager, LayerMask targetLayer)
    {
        this.teleporterManager = teleporterManager;
        this.targetLayer = targetLayer;

        if (teleporterManager == null)
        {
            Logger.LogError($"[Teleporter] '{gameObject.name}'가 TeleporterManager의 자식으로 배치되지 않았습니다! 상위 계층에 TeleporterManager가 필요합니다.");
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTargetLayer(other.gameObject.layer))
        {
            if (teleporterManager.MoveCheck())
            {
                teleporterManager.EndTeleport();
            }
            else
            {
                // [추가] 텔레포트 요청 전에, 도착할 곳의 카메라 경계를 매니저에게 미리 알려줍니다.
                teleporterManager.PrepareCameraBoundary(destinationBoundary);

                // 찾은 매니저에게 텔레포트를 요청합니다.
                teleporterManager.RequestTeleport(other.transform, destinationTeleporter);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTargetLayer(other.gameObject.layer))
        {
            // 대상이 영역을 벗어나면, 매니저에게 쿨다운 해제를 알립니다.
            teleporterManager.ClearCooldownForTarget(other.transform);
        }
    }

    private bool IsTargetLayer(int objectLayer)
    {
        return (targetLayer.value & (1 << objectLayer)) > 0;
    }
}
