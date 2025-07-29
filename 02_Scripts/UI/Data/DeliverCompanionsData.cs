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
    [SerializeField] private List<ShowCompanionText> companionUIObjects = new List<ShowCompanionText>();
    [SerializeField] private int slotsPerPage = 4; // 한 페이지에 4개

    private int currentPage = 0;
    private CompanionWindow companionWindow;
    public List<CompanionData> companionDatas = new List<CompanionData>(); // DBManager의 CompanionDB로 변경하기
    CompanionEquipManager equipManager;
  
    private void OnEnable()
    {      
        ShowData();
    }
    public void Init()
    {
        equipManager = UIManager.Instance.companionEquipManager;
        companionWindow = GetComponent<CompanionWindow>();
        DeliverData();
        FindShowCompanionText(companionParent);
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
        for (int i = 1; i <= DataManager.Instance.CompanionDB.Count; i++)
        {
            string key = "COMPANION_" + i.ToString("D3");
            //CompanionData companionInfo = new CompanionData(GetData(key).NameKey, GetData(key).IsJoined,
            //    GetData(key).TrustLevel, GetData(key).DialogueKey, GetData(key).DescKey, GetData(key).SkillNameKey, GetData(key).SkillDescKey,5,5,5,100,100,0.1f);

            // 값 설정
            CompanionData companionData = new CompanionData();
            var data = GetData(key);
            companionData.ID = data.ID;
            companionData.NameKey = data.NameKey;
            companionData.IsJoined = data.IsJoined;
            companionData.TrustLevel = data.TrustLevel;
            companionData.DialogueKey = data.DialogueKey;
            companionData.DescKey = data.DescKey;
            companionData.SkillNameKey = data.SkillNameKey;
            companionData.SkillDescKey = data.SkillDescKey;

            companionData.AttackPower = 5;
            companionData.DefensePower = 5;
            companionData.MoveSpeed = 5;
            companionData.Hp = 100;
            companionData.Mp = 100;
            companionData.EvasionRate = 0.1f;
            companionDatas.Add(companionData);

        }
    }
    private void ShowData()
    {
        // int startIndex = currentPage *slotsPerPage;

        for (int i = 0; i < companionUIObjects.Count; i++)
        {
            //if (i >= startIndex && i < startIndex + slotsPerPage)
            //{
                if (DataManager.Instance.CompanionDB.ElementAt(i).Value.IsJoined == false)
                {
                    companionUIObjects[i].gameObject.SetActive(true);
                    ShowCompanionText showCompanionText = companionUIObjects[i].GetComponent<ShowCompanionText>();
                    showCompanionText.ShowText(companionDatas[i]);

                    int capturedIndex = i;
                    showCompanionText.managementButton.onClick.RemoveAllListeners();
                    showCompanionText.managementButton.onClick.AddListener(() => companionWindow.UpdateInfo(companionDatas[capturedIndex], equipManager.inventories[capturedIndex]));
                }       
           // }
        }
    }

    /// <summary>
    /// BFS를 활용하여 ShowCompanionText가 붙어있는 오브젝트들을 찾습니다.
    /// </summary>
    private void FindShowCompanionText(Transform root)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
      
            if (current.TryGetComponent<ShowCompanionText>(out ShowCompanionText showCompanionText)) 
            {
                companionUIObjects.Add(showCompanionText);
            }

            // 자식들을 Queue에 추가합니다.
            foreach(Transform child in current)
            {
                queue.Enqueue(child);
            }
        }
    }
}
