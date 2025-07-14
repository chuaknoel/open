using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Tooltip을 사용할 때 쓰는 클래스입니다.
/// </summary>
public class TooltipParam : OpenParam
{
    public string title;
    public string titleType;
    public string description;

    public TooltipParam (string _title, string _titleType,string _description)
    {
        this.title = _title;
        this.titleType = _titleType;
        this.description = _description;
    }
}
