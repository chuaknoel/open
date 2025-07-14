using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

/// <summary>
/// 플레이어의 위치를 보여주는 스크립트입니다.
/// </summary>
public class ShowLocation : MonoBehaviour
{
    [SerializeField] private TMP_Text locationText;

    /// <summary>
    /// 플레이어의 위치를 표시합니다.
    /// </summary>
    /// <param name="location">위치</param>
    public void ShowLocationText(string location)
    {
        locationText.text = location;
    }
}
