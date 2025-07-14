using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startPos; // 시작 위치
    private Vector2 startMousePos; // 마우스 시작 위치
    private Vector2 movePos; // 움직일 위치

    private CursorLockMode startLockMode; // 이동 시작시 커서 모드
   /// <summary>
   /// 드래그 시작
   /// </summary>
   /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 화면 밖으로 못나가게
        startLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Confined;

        // 위치 저장
        startPos = transform.position;
        startMousePos = eventData.position;
        
        // 캔버스 가장 위로 지정
        transform.SetAsLastSibling();
    }
    /// <summary>
    /// 드래그 중
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // 움직여야할 위치 저장
        movePos = eventData.position - startMousePos;
        // 드래그 이동
        transform.position = startPos + movePos;
    }
    /// <summary>
    /// 드래그 끝
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.lockState = startLockMode;
    }
}
