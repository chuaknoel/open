using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScollviewTest : MonoBehaviour
{
    public ScrollRect scrollRect;

    private float scrollSnap;       //한번 스크롤시 이동할 스냅양

    public float slotHeight = 100f; //슬롯 높이 (임시)

    private float previousValue;    //스크롤 입력을 받기전 위치 : 스크롤렉트는 휠 입력을 내부에서 처리하기 때문에 입력 처리를 받기전 값을 알고 있어야 정확한 스냅이 가능해진다.

    private float spacing;          //스냅 값을 구하기 위한 그리드의 스페이싱 값

    private float scrollInput;      //스냅 방향을 정하기 위한 휠 입력값

    private float correctionValue;  //최하단(0) 지점에서 스냅 동작을 할때 상단 UI가 잘리지 않도록 보정하는 값

    private void Start()
    {
        Init();
        previousValue = scrollRect.verticalScrollbar.value;
        spacing = scrollRect.content.GetComponent<GridLayoutGroup>().spacing.y;
    }

    private void Update()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            ScnollSnap();
        }
    }

    public void Init()
    {
        StartCoroutine(UpdateScrollSnap());
    }

    private IEnumerator UpdateScrollSnap() //스냅양 계산
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content); //슬롯 추가시 content 크기가 늘어나기 때문에 업데이트 해주기
        yield return null;                                               //UI 변경 사항이 적용 되려면 1프레임을 기다려주면 안전하게 초기화 가능
        scrollSnap = (slotHeight + spacing) / (scrollRect.content.rect.height - scrollRect.viewport.rect.height); //슬롯 높이와 스페이싱 값을 같이 처리해야 실제로 이동할 거리가 나옴
    }

    private void ScnollSnap()
    {
        int dir = scrollInput > 0 ? 1 : -1;                  // 스크롤 방향
        float nextPos = previousValue + (dir * scrollSnap);  // 스크롤 방향에 따른 이동할 위치 값

        if (nextPos < 0 && previousValue > 0)                //이동할 구역이 음수라는 것은 이동거리가 스냅거리보다 짧은 것을 뜻함.
        {                                                    //현재 벨류가 0.2이고 스냅이 0.3일때 실제 이동거리는 0.2가 됌
            correctionValue = scrollSnap + nextPos;          //위치 0에서 스냅 거리를 주면 상단 아이콘이 잘린거처럼 보이는 현상이 생기게 됌
                                                             //그래서 이동거리를 초과하는 거리 : 이동가능한 거리(0.2) + 스냅(-0.3) = -0.1
        }                                                    //스냅(0.3) 에서 초과거리 -0.1을 계산하여 실제 이동거리 보장값 correctionValue(0.2)을 구해준다.

        if (dir > 0 && previousValue <= 0)                   //dir이 양수면 위방향 스크롤 입력이고, 스크롤벨류의 위치 최하단(0) 일때
        {
            scrollRect.verticalNormalizedPosition += correctionValue; //실제 스냅이 아닌 보정값만 처리를 하여 상단 슬롯이 잘리는 현상 방지
        }
        else
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(nextPos);
        }

        previousValue = scrollRect.verticalNormalizedPosition;
    }
}
