using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotType
{
    item,
    skill
}

public static class DragData<Slot,TData> 
{
    public static TData DraggedData;
    public static Slot OriginSlot;

    public static void Clear()
    {
        DraggedData = default;
        OriginSlot = default;
    }
}
public abstract class BaseDrag<Slot, TData> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  
{
    [SerializeField] protected Canvas canvas;
    public Image dragItemImage;

    protected Slot slot;
    protected Vector3 startPosition;
    protected Transform startParent;
    protected GameObject draggedSlot; // 드래그 받은 슬롯;

    private void OnEnable()
    {
        slot = GetComponent<Slot>();
    }

    protected abstract TData GetData();
    protected abstract Sprite GetSprite(TData data);
    protected abstract void OnDropCompleted(TData droppedData, Slot originSlot);

    // 드래그 시작시
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (slot == null || GetData() == null) return;

        // 아이템과 슬롯 임시 저장
        DragData<Slot, TData>.DraggedData = GetData();
        DragData<Slot, TData>.OriginSlot = slot;

        // 드래그 이미지 활성화
        dragItemImage.enabled = true;
        dragItemImage.sprite = GetSprite(GetData());

        // 드래그 이미지 위로
        startParent = dragItemImage.transform.parent;
        dragItemImage.transform.SetParent(canvas.transform);
        dragItemImage.transform.SetAsLastSibling();
        dragItemImage.GetComponent<Image>().raycastTarget = false;

        // 드래그 이동
        startPosition = dragItemImage.transform.position;
    }
    // 드래그 중
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (slot == null || GetData() == null) return;

        // 드래그 이동
        dragItemImage.transform.position = Input.mousePosition;
    }
    // 드래그 끝
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (DragData<Slot, TData>.DraggedData == null || DragData<Slot, TData>.OriginSlot == null) return;
        
        // 드래그 원래 위치로
        dragItemImage.transform.position = startPosition;
        dragItemImage.transform.SetParent(startParent);

        // 드래그 비활성화
        dragItemImage.raycastTarget = true;
        dragItemImage.enabled = false;

        OnDropCompleted(DragData<Slot, TData>.DraggedData, DragData<Slot, TData>.OriginSlot);

        // 임시 데이터 비우기
        DragData<Slot, Item>.Clear();
    }
    /// <summary>
    /// 마우스 포인터의 슬롯이 무엇인지 반환합니다.
    /// </summary>
    protected virtual T CheckMousePointerSlot<T>() where T : Component
    {
        // 마우스 위치에서 Raycast
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            // 마우스 위치의 오브젝트가 스킬 임시 슬롯이 아니라면 스킬 장착 해제.
            if (result.gameObject.TryGetComponent<T>(out var skillSlot))
            {
                draggedSlot = result.gameObject;
                return skillSlot;
            }
        }
        return null;
    }

   
}
