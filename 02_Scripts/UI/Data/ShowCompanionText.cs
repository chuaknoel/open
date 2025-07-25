using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// 동료 데이터를 표시하는 스크립트입니다.
/// </summary>
public class ShowCompanionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI companionName;
    [SerializeField] private TextMeshProUGUI joining;
    [SerializeField] private TextMeshProUGUI trustLevel;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private Slider trustLevelSlider;
    public Button managementButton; // 관리 버튼
    /// <summary>
    ///  동료 데이터를 표시합니다.
    /// </summary>
    /// <param name="_companionName">동료 이름</param>
    /// <param name="_joining">합류 여부</param>
    /// <param name="trustLevel">신뢰도 레벨</param>
    /// <param name="_dialogue">대화문</param>
    public void ShowText(CompanionData companion)
    {
        this.companionName.text = companion.NameKey;
        this.joining.text = "[합류중]";
        this.trustLevel.text = companion.TrustLevel.ToString();
        float trustLevel = (float)(companion.TrustLevel);
        this.percentText.text = "(" + ((int)Mathf.Round(companion.TrustLevel * 100f)).ToString() + "%)";
        trustLevelSlider.value = trustLevel / 100f;
        this.dialogue.text = companion.DialogueKey;
    }
}
