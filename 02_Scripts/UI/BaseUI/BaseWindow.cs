using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 모든 UI 창들의 공통 기능을 담는 부모 클래스
/// </summary>
public abstract class BaseWindow : BaseUI
{
    [SerializeField] protected Button closeButton;

    /// <summary>
    /// UI 활성화시 호출
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();
        AddCloseButtonListener(CloseUI);
    }

    /// <summary>
    /// 인벤토리 활성화시 호출
    /// </summary>
    public virtual void OpenInventroy()
    {
        GameObject inventroy = transform.GetChild(0).gameObject;

        inventroy.SetActive(!inventroy.activeSelf);
        inventroy.transform.SetAsLastSibling();

        AddCloseButtonListener(CloseInventroy);
    }

    /// <summary>
    /// UI 비활성화시 호출
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
        RemoveCloseButtonListener(CloseUI);
    }

    /// <summary>
    /// 인벤토리 비활성화시 호출
    /// </summary>
    public virtual void CloseInventroy()
    {
        GameObject inventroy = transform.GetChild(0).gameObject;
        inventroy.SetActive(false);

        RemoveCloseButtonListener(CloseInventroy);
    }

    /// <summary>
    /// 닫기 버튼 이벤트 연결
    /// </summary>
    /// <param name="action"></param>
    protected virtual void AddCloseButtonListener(UnityEngine.Events.UnityAction action)
    {
        if (closeButton != null)
        {        
            closeButton.onClick.AddListener(action);
        }
    }

    /// <summary>
    /// 닫기 버튼 이벤트 제거
    /// </summary>
    /// <param name="action"></param>
    private void RemoveCloseButtonListener(UnityEngine.Events.UnityAction action)
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(action);
        }
    }
}
