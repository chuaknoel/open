using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Unity 에디터에서 연결할 도착 지점 Teleporter
    [SerializeField]
    private Teleporter destinationTeleporter;

    // 플레이어가 이 텔레포터를 사용한 직후인지 확인하는 변수
    // (도착하자마자 다시 출발 지점으로 돌아가는 무한 루프를 방지하기 위함)
    private bool isJustTeleported = false;

    // 이 트리거 영역에 다른 Collider가 들어왔을 때 호출됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 들어온 오브젝트가 'Player' 태그를 가지고 있고, 방금 텔레포트한 상태가 아니라면
        if (other.CompareTag("Player") && !isJustTeleported)
        {
            // 도착 지점이 설정되어 있다면, 플레이어를 그 위치로 이동시킵니다.
            if (destinationTeleporter != null)
            {
                // 도착 지점 텔레포터에게 "지금 플레이어가 도착했으니, 다시 돌려보내지 마라"고 알려줍니다.
                destinationTeleporter.isJustTeleported = true;

                // 플레이어의 위치를 도착 지점 텔레포터의 위치로 변경합니다.
                other.transform.position = destinationTeleporter.transform.position;
            }
        }
    }

    // 이 트리거 영역에서 다른 Collider가 나갔을 때 호출됩니다.
    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 텔레포트 존을 완전히 빠져나갔다면, 다시 텔레포트할 수 있도록 상태를 초기화합니다.
        if (other.CompareTag("Player"))
        {
            isJustTeleported = false;
        }
    }
}
