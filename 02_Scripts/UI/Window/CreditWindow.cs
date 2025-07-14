using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class CreditWindow : BaseWindow
{
    public override UIType UIType => UIType.Window;

    public RectTransform scrollView;
    public RectTransform content; // ScrollRect 안의 Content
    public float scrollSpeed = 100f;

    /// <summary>
    /// 화면을 활성화하는 스크립트입니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();
        ShowCredit();
    }
    /// <summary>
    /// 화면을 비활성화하는 스크립트입니다.
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }
    /// <summary>
    /// Credit 창의 애니메이션 기능을 표시합니다.
    /// </summary>
    private void ShowCredit()
    {
        // 스크롤뷰 내부 영역의 높이
        float viewportHeight = scrollView.GetComponent<RectTransform>().rect.height;

        //스크롤뷰 내부에 있는 실제 컨텐츠의 전체 높이
        float contentHeight = content.rect.height;

        float startY = 0f;
        float endY = contentHeight - viewportHeight;

        float duration = endY / scrollSpeed;

        content.anchoredPosition = new Vector2(0, 0);

        content.DOAnchorPosY(endY, duration)
               .SetEase(Ease.Linear)
               .SetOptions(snapping: true);
    }
}
