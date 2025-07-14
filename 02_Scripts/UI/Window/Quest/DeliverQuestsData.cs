using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverQuestsData : MonoBehaviour
{
   private ShowQuestInfo showQuestInfo;

   // [SerializeField] private Transform questParent;
   // [SerializeField] private GameObject questInfoPrefab;

    [SerializeField] private Button[] questInfoButton = new Button[5];

    private void OnEnable()
    {
        showQuestInfo = GetComponent<ShowQuestInfo>();  
        ShowData();
    }
    public void DeliverData()
    {
        
    }
    private void ShowData()
    {
        for (int i = 0; i < questInfoButton.Length; i++)
        {
            questInfoButton[i].onClick.RemoveAllListeners();
            questInfoButton[i].onClick.AddListener(() => showQuestInfo.UpdateInfo());
        }        
    }
}
