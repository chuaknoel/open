using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTap : MonoBehaviour
{
    private SkillTempSlotManager skillTempSlotManager;
    [SerializeField] BookAnimation bookAnimation;
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int currentPage = 0;

    private void OnEnable()
    {
        skillTempSlotManager = GetComponent<SkillTempSlotManager>();

        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
    }
    private void NextPage()
    {     
        bookAnimation.BookOpen();

        if (this.gameObject.activeSelf)
        {
            skillTempSlotManager.DestroySkillTempSlot();
            currentPage++;

            if (currentPage >= pages.Count)
            {
                currentPage = 0; // 마지막 페이지였으면 처음으로
            }

            StartCoroutine(ShowPage(currentPage));

        }
    }
    private void PrevPage()
    {     
        bookAnimation.BookClose();
        if (this.gameObject.activeSelf)
        {
            skillTempSlotManager.DestroySkillTempSlot();
            currentPage--;

            if (currentPage < 0)
            {
                currentPage = pages.Count - 1; // 처음 페이지에서 왼쪽 누르면 마지막으로
            }

            StartCoroutine(ShowPage(currentPage));
        }
    }

    private IEnumerator ShowPage(int index)
    {
        ClearPage();
        yield return new WaitForSeconds(0.5f);
        pages[index].SetActive(true);
    }
    private void ClearPage()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }
    }
    private void OnDisable()
    {
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
    }
}
