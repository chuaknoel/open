using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Tooltip에 정보를 보여주는 클래스입니다.
/// </summary>
public class Tooltip : BaseWindow
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text titleTypeText;
    [SerializeField] private TMP_Text descriptionText;

    public RectTransform rect;

    public override UIType UIType => UIType.Window;

    /// <summary>
    /// Tooltip이 열렸을 때 정보를 출력해줍니다.
    /// </summary>
    /// <param name="param"></param>
    public override void OpenUI(OpenParam param)
    {
        base.OpenUI();
        var p = param as TooltipParam;
        titleText.text = p.title;
        titleTypeText.text = p.titleType;
        descriptionText.text = p.description;
    }

    /// <summary>
    /// Tooltip을 비활성화 시켜줍니다.
    /// </summary>
    public override void CloseUI()
    {
        Logger.Log("툽팁 비활성화");
        base.CloseUI();
    }
}
