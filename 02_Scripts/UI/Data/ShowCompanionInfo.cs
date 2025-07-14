using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCompanionInfo : MonoBehaviour
{
    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TextMeshProUGUI companionName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI[] companionStat = new TextMeshProUGUI[6];
    [SerializeField] private BookWindow bookWindow;

    public void UpdateInfo(CompanionInfo companionInfo)
    {
        bookWindow.HideTitleText();
        InfoPanel.SetActive(true);
        titleText.gameObject.SetActive(false);
        companionName.text = companionInfo.CompanionName;
        description.text = companionInfo.DescKey;
        companionStat[0].text = companionInfo.AttackPower.ToString();
        companionStat[1].text = companionInfo.DefensePower.ToString();
        companionStat[2].text = companionInfo.MoveSpeed.ToString();
        companionStat[3].text = companionInfo.HP.ToString();
        companionStat[4].text = companionInfo.MP.ToString();
        int value = (int)(companionInfo.EvasionRate * 100f);
        companionStat[5].text = value.ToString();
    }
}
