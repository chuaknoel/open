using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plyer의 HP,MP를 UI를 통해 보여줍니다.
/// </summary>
public class ShowStatus : MonoBehaviour
{
    [SerializeField] private StatusBar hpBar;
    [SerializeField] private StatusBar mpBar;

    /// <summary>
    /// HP를 갱신해서 보여줍니다.
    /// </summary>
    /// <param name="hp">현재 체력</param>
    /// <param name="maxHp">최대 체력</param>
    public void UpdateHPBar(float hp, float maxHp)
    {
        hpBar.ShowBar(hp, maxHp);
    }

    /// <summary>
    /// MP를 갱신해서 보여줍니다.
    /// </summary>
    /// <param name="mp">현재 마력</param>
    /// <param name="maxMp">최대 마력</param>
    public void UpdateMPBar(float mp, float maxMp)
    {
        mpBar.ShowBar(mp, maxMp);
    }

    /// <summary>
    /// HP와 MP를 갱신해서 보여줍니다.
    /// </summary>
    /// <param name="hp">현재 체력</param>
    /// <param name="maxHp">최대 체력</param>
    /// <param name="mp">현재 마력</param>
    /// <param name="maxMp">최대 마력</param>
    public void UpdateStatus(float hp, float maxHp, float mp, float maxMp)
    {
        UpdateHPBar(hp, maxHp);
        UpdateMPBar(mp, maxMp);
    }
}
