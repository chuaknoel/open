using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverCollectionData : MonoBehaviour
{
    private ShowCollectionInfo showCollectionInfo;

    [SerializeField] private List<Button> ItemInfoButton = new List<Button>();

    public void Init()
    {
        showCollectionInfo = GetComponent<ShowCollectionInfo>();
        FindButtons(this.gameObject.transform);
        ShowData();
    }
    public void DeliverData()
    {

    }
    private void ShowData()
    {
        for (int i = 0; i < ItemInfoButton.Count; i++)
        {
            ItemInfoButton[i].onClick.RemoveAllListeners();
            ItemInfoButton[i].onClick.AddListener(() => showCollectionInfo.UpdateInfo());
        }
    }

    /// <summary>
    /// DFS를 활용하여 도감 창의 버튼들을 전부 찾는 메서드입니다.
    /// </summary>
    /// <param name="parent"></param>
    private void FindButtons(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // 자식의 버튼을 찾습니다.
            if (child.TryGetComponent<Button>(out Button btn))
            {
                ItemInfoButton.Add(btn);  
            }
            // 재귀적으로 버튼을 찾습니다.
            FindButtons(child);
        }     
    }
}
