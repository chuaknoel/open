using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    [SerializeField] private SkillSlot[] skillSlots;
    [SerializeField] BookAnimation bookAnimation;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private int currentPage = 0;

    private SkillTempSlotManager skillTempSlotManager;
    private Coroutine currentTapCoroutine;

    public void Init()
    {
        skillTempSlotManager = GetComponent<SkillTempSlotManager>();

        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        SlotInit();
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

            StartShowPageCoroutine(currentPage);
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

            StartShowPageCoroutine(currentPage);
        }
    }
    private void StartShowPageCoroutine(int index)
    {
        if (currentTapCoroutine != null)
        {
            StopCoroutine(currentTapCoroutine);
            currentTapCoroutine = null;
        }

        currentTapCoroutine = StartCoroutine(ShowPage(index));
    }
    private IEnumerator ShowPage(int index)
    {
        ClearPage();
        yield return new WaitForSeconds(0.5f);
        pages[index].SetActive(true);
        currentTapCoroutine = null;
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
    private void SlotInit()
    {
        // 나중에 수정
        skillSlots = GetComponentsInChildren<SkillSlot>(includeInactive:true);

        for (int i =0; skillSlots.Length > i; i++)
        {
           skillSlots[i].Init(skillTempSlotManager);

            if (skillSlots[i].gameObject.TryGetComponent<SkillDrag>(out SkillDrag skillDrag))
            {
                skillDrag.Init();
            }
        }
    }
}
