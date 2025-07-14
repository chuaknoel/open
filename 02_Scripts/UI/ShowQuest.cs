using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
/// <summary>
/// 퀘스트를 보여주는 스크립트입니다.
/// </summary>
public class ShowQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text questText;

    /// <summary>
    /// 퀘스트를 표시합니다.
    /// </summary>
    /// <param name="quest"></param>
   public void ShowQuestText(string quest)
    {
        questText.text = "현재 목표 : " + quest;
    }
}
