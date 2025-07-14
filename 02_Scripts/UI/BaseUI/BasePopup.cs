using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePopup : BaseUI
{
    [SerializeField] protected Button okButton;
    [SerializeField] protected Button closeButton;

    /// <summary>
    /// UI 활성화시 호출
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();

        if(okButton != null)
        {
            okButton.onClick.AddListener(CloseUI);
        }
        if (closeButton != null && okButton != null)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
    }
    /// <summary>
    /// UI 비활성화시 호출
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();

        if (closeButton != null && okButton != null)
        {
            okButton.onClick.RemoveListener(CloseUI);
            closeButton.onClick.RemoveListener(CloseUI);
        }
    }
}
