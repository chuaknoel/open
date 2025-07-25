using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Enums;

public class CompanionWindow : MonoBehaviour
{
    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TextMeshProUGUI companionName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI[] companionStat = new TextMeshProUGUI[6];
    [SerializeField] private BookWindow bookWindow;

    private CompanionEquipManager companionEquipManager;


    public void Init()
    {
        UIManager uiManager = UIManager.Instance;
        this.companionEquipManager = uiManager.companionEquipManager;
    }
    public void UpdateInfo(CompanionData companionData, Inventory inventory)
    {
        bookWindow.HideTitleText();
        InfoPanel.SetActive(true);
        titleText.gameObject.SetActive(false);
        companionName.text = companionData.NameKey;
        description.text = companionData.DescKey;
        companionStat[0].text = companionData.AttackPower.ToString();
        companionStat[1].text = companionData.DefensePower.ToString();
        companionStat[2].text = companionData.MoveSpeed.ToString();
        companionStat[3].text = companionData.Hp.ToString();
        companionStat[4].text = companionData.Mp.ToString();
       // int value = (int)(companionInfo.EvasionRate * 100f);
        companionStat[5].text = companionData.EvasionRate.ToString();
        OnDisableCompanionInventory(); // 기존 동료 인벤토리 비활성화
        inventory.gameObject.SetActive(true);
    }

    /// <summary>
    /// 동료 인벤토리 비활성화
    /// </summary>
    public void OnDisableCompanionInventory()
    {
        foreach (var inventory in companionEquipManager.inventories)
        {
            inventory.gameObject.SetActive(false);
        }
    }
}
