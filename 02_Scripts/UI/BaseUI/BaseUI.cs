using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
/// <summary>
/// UIType 정의
/// </summary>
public enum UIType
{
    None,
    SelfWindow, // 자기 자신을 켜거나 끄는 단순한 창
    ParentWindow, // 자식 윈도우들을 제어하는 상위 창
    ToastMessage,
    Panel
}
/// <summary>
/// UI 구성요소들의 부모 클래스
/// </summary>
public abstract class BaseUI : MonoBehaviour
{
    public abstract UIType UIType { get; }

    public virtual void OpenUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        gameObject.transform.SetAsLastSibling();
    }
    public virtual void OpenUI(OpenParam param) => OpenUI();

    public virtual void CloseUI() 
    {
        gameObject.SetActive(false);
    }
}
