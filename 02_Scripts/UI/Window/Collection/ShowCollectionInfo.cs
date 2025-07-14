using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCollectionInfo : MonoBehaviour
{
    [SerializeField] private BookWindow bookWindow;
    [SerializeField] private GameObject collectionInfo;
    [SerializeField] private TMP_Text titleText;

    public void UpdateInfo()
    {
        bookWindow.HideTitleText();
        collectionInfo.SetActive(true);
        titleText.gameObject.SetActive(false);
    }
}
