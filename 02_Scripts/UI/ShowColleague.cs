using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
/// <summary>
/// 플레이어의 동료를 표시하는 스크립트입니다.
/// </summary>
public class ShowColleague : MonoBehaviour
{
    [SerializeField] private TMP_Text[] colleagueText = new TMP_Text[3];
    [SerializeField] private Image[] colleagueImage = new Image[3];

    /// <summary>
    /// 동료 정보를 표시합니다.
    /// </summary>
    /// <param name="colleague"></param>
    public void ShowColleagues(string[] colleague)
    {
        for (int i = 0; i < colleague.Length; i++)
        {
            if(colleague[i] != null)
            {
                colleagueText[i].text = colleague[i];
                colleagueImage[i].gameObject.SetActive(true);
            }
           else
            {
                colleagueText[i].text = "";
                colleagueImage[i].gameObject.SetActive(false);
            }
        }
    }
}
