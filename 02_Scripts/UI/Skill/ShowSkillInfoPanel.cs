using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowSkillInfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] private GameObject skillInfoPanelPrefab;
    private GameObject skillInfoPanel;
    private SkillSlot slot;

    private void OnEnable()
    {
        slot = GetComponent<SkillSlot>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
        ChangeTooltipPos();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(skillInfoPanel);
    }

    /// <summary>
    /// 스킬 정보창 보여주기
    /// </summary>
    protected void ShowTooltip()
    {      
        //  활성화
        skillInfoPanel =  Instantiate(skillInfoPanelPrefab);
        skillInfoPanel.transform.SetParent(canvas.transform);
        skillInfoPanel.transform.SetAsLastSibling();

        // 내용 전달
        SkillInfoPanel _skillInfoPanel = skillInfoPanel.GetComponent<SkillInfoPanel>();
        _skillInfoPanel.ShowText(slot.GetSkill());

        // 스킬 정보창 위치 변경
        ChangeTooltipPos();
    }

    /// <summary>
    /// 스킬 정보창 위치 변경
    /// </summary>
    private void ChangeTooltipPos()
    {
        Vector2 mouseScreenPos = slot.GetComponent<RectTransform>().position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mouseScreenPos,
            null,
            out Vector2 localPos
        );
        skillInfoPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(localPos.x + 198f, localPos.y - 53f);
    }
}
