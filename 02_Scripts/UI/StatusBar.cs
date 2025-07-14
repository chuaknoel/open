using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HP Bar와 Mp Bar가 공통으로 사용하는 클래스입니다.
/// </summary>
public class StatusBar : MonoBehaviour
{
    private Image fillImage;

    private void Start()
    {
        fillImage = GetComponent<Image>();
    }

    /// <summary>
    /// 상태 바를 갱신합니다.
    /// </summary>
    /// <param name="current">현재 값</param>
    /// <param name="max">최대 값</param>
    public void ShowBar(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
