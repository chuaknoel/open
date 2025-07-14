using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTempSlotManager : MonoBehaviour
{

    public GameObject tempSkillSlot;
   [HideInInspector] public GameObject _tempSkillSlot;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject bookWindow;
    [SerializeField] private Image dragImage;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] SkillQuickSlotManager skillQuickSlotManager;

    public void CreateSkillTempSlot(Slot slot)
    {
        Logger.Log(_tempSkillSlot);
        if (_tempSkillSlot == null)
        {
            _tempSkillSlot = Instantiate(tempSkillSlot);
            Logger.Log(_tempSkillSlot);
            TempSkillQuickSlotManager tempSkillQuickSlotManager = _tempSkillSlot.GetComponent<TempSkillQuickSlotManager>();
            tempSkillQuickSlotManager.SkillQuickSlotManager = skillQuickSlotManager;
            tempSkillQuickSlotManager.SetTempSkillQuickSlot();

            foreach (Transform child in _tempSkillSlot.transform)
            {
                SkillDrag skillDrag = child.gameObject.GetComponent<SkillDrag>();
                skillDrag.Init(canvas, dragImage);

                SkillQuickSlotEvent skillSlotEvent = child.gameObject.GetComponent<SkillQuickSlotEvent>();
                skillSlotEvent.skillQuickSlotManager = skillQuickSlotManager;
            }
            _tempSkillSlot.transform.SetParent(slot.transform);
            _tempSkillSlot.GetComponent<RectTransform>().localPosition = new Vector3(0, -85f, 0);

            _tempSkillSlot.transform.SetParent(bookWindow.transform);
            _tempSkillSlot.transform.SetAsLastSibling();
        }
        else
        {
            Destroy(_tempSkillSlot);
        }
    }

    /// <summary>
    /// 스킬 퀵슬롯을 삭제합니다.
    /// </summary>
    public void DestroySkillTempSlot()
    {
        //GameObject _temporaryQuickSlot = 
        //Debug.Log($"Destroy 대상: {skillManager.temporaryQuickSlot}, 타입: {skillManager.temporaryQuickSlot.GetType()}");

        //Destroy(skillManager.temporaryQuickSlot);
        if (_tempSkillSlot != null)
        {
            Destroy(_tempSkillSlot);
        }
    }

}
