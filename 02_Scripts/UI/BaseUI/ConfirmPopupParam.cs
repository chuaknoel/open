using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI를 열 때 사용하는 파라미터들의 공통 부모 클래스
/// </summary>
public abstract class OpenParam { }

/// <summary>
/// 확인창을 열 때 사용하는 파라미터들
/// </summary>
public class ConfirmPopupParam : OpenParam
{
    public string Title;
    public string Message;
    public bool ShowPopup;
}