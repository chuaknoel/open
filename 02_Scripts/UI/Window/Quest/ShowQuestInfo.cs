using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShowQuestInfo : MonoBehaviour
{
    [SerializeField] private GameObject questInfo;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private BookWindow bookWindow;

    public void UpdateInfo()
    {
        bookWindow.HideTitleText();
        questInfo.SetActive(true); 
        titleText.gameObject.SetActive(false);
    }
}
