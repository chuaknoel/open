using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 삭제 스크립트입니다.
/// </summary>
public class DestroyWindow : BaseWindow
{
    public override UIType UIType => UIType.Window;
    public UIInventory uiInventory;

    [SerializeField] private Button okButton;
    [HideInInspector] public Slot slot;

    /// <summary>
    /// 아이템 삭제창이 활성화시 실행됩니다.
    /// </summary>
    public override void OpenUI()
    {
        base.OpenUI();
        okButton.onClick.AddListener(() => DestroyItem(slot));
        closeButton.onClick.AddListener(CloseUI);   
    }
    
    /// <summary>
    /// 아이템 삭제창이 비활성화될 때 실행됩니다. .SS
    /// </summary>
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 아이템 삭제 기능입니다.
    /// </summary>
    /// <param name="slot"></param>
    private void DestroyItem(Slot slot)
    {
        if (slot != null && slot.Item != null) 
        {
            slot.ClearSlot();
            CloseUI();
        }
    }
}
