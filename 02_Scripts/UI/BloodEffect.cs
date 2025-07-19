using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 체력이 20% 이하일 시 피격 이펙트를 발생시키는 스크립트입니다.
/// </summary>
public class BloodEffect : MonoBehaviour
{
    [SerializeField] private Image bloodEffect;
    [SerializeField] private float duration; // 피격 효과 시간
   
    /// <summary>
    /// 피격 효과를 나타냅니다.
    /// </summary>
    /// <param name="hp">플레이어의 현재 체력</param>
    /// <param name="maxHp">플레이어의 최대 체력</param>
    /// <returns></returns>
    IEnumerator ShowBloodEffect()
    {
        while (true)
        {
            // 알파값 0까지 흐려지기
            bloodEffect.DOFade(0f, duration)
                .OnComplete(() =>
                {
                    // 다시 선명하게 복귀
                    bloodEffect.DOFade(0.27f, duration);
                });
            yield return new WaitForSeconds(duration*2);
        }
    }

    /// <summary>
    /// 피격 효과를 끄는 함수입니다.
    /// </summary>
    public void DestroyBloodEffect()
    {
        bloodEffect.DOFade(0f, duration);
    }
}
