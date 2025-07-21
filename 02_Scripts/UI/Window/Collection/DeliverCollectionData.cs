using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverCollectionData : MonoBehaviour
{
    private ShowCollectionInfo showCollectionInfo;

    [SerializeField] private Button[] ItemInfoButton = new Button[5];

    public void Init()
    {
        showCollectionInfo = GetComponent<ShowCollectionInfo>();
        ShowData();
    }
    public void DeliverData()
    {

    }
    private void ShowData()
    {
        for (int i = 0; i < ItemInfoButton.Length; i++)
        {
            ItemInfoButton[i].onClick.RemoveAllListeners();
            ItemInfoButton[i].onClick.AddListener(() => showCollectionInfo.UpdateInfo());
        }
    }
}
