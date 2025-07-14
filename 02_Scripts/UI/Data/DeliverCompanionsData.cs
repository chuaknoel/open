using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 동료 데이터를 전달해주는 스크립트입니다.
/// </summary>
public class DeliverCompanionsData : MonoBehaviour
{
    [SerializeField] private Transform companionParent;
    [SerializeField] private GameObject companionPrefab;
    [SerializeField] private List<GameObject> companions = new List<GameObject>();
    [SerializeField] private List<CompanionInfo> companionInfos = new List<CompanionInfo>();
    [SerializeField] private int slotsPerPage = 4; // 한 페이지에 4개
    private int currentPage = 0;
    private ShowCompanionInfo showCompanionInfo;
    // Start is called before the first frame update
    void OnEnable()
    {
        showCompanionInfo = GetComponent<ShowCompanionInfo>();
        DeliverData();
        ShowData();
    }

    private CompanionData GetData(string key)
    {
        return DataManager.Instance.CompanionDB[key];
    }

    /// <summary>
    /// UI에 읽어온 동료 데이터를 전달해줍니다.
    /// </summary>
    public void DeliverData()
    {
        if(companions.Count > 0)
        {
            return;
        }
        for (int i = 1; i <= DataManager.Instance.CompanionDB.Count; i++)
        {
            var companion = Instantiate(companionPrefab);

            companion.transform.SetParent(companionParent);

            string key = "COMPANION_" + i.ToString("D3");
            CompanionInfo companionInfo = new CompanionInfo(GetData(key).NameKey, GetData(key).IsJoined,
                GetData(key).TrustLevel, GetData(key).DialogueKey, GetData(key).DescKey, GetData(key).SkillNameKey, GetData(key).SkillDescKey,5,5,5,100,100,0.1f);

            companions.Add(companion);
            companionInfos.Add(companionInfo);
        }
    }
    private void ShowData()
    {
        int startIndex = currentPage *slotsPerPage;

        for (int i = 0; i < companions.Count; i++)
        {
            if (i >= startIndex && i < startIndex + slotsPerPage)
            {          
                ShowCompanionText showCompanionText = companions[i].GetComponent<ShowCompanionText>();
                showCompanionText.ShowText(companionInfos[i]);

                int capturedIndex = i;

                showCompanionText.managementButton.onClick.RemoveAllListeners();
                showCompanionText.managementButton.onClick.AddListener( () => showCompanionInfo.UpdateInfo(companionInfos[capturedIndex]));
            }
        }
    }
}
