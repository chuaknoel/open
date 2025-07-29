using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTempSlotManager : MonoBehaviour
{
   // public GameObject tempSkillSlot;

    [HideInInspector] public TempSkillQuickSlotManager tempSkillQuickSlotManager;
    [HideInInspector] public GameObject _tempSkillSlot;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject bookWindow;
    [SerializeField] private Image dragImage;
    [SerializeField] private SkillQuickSlotManager skillQuickSlotManager;

    public void Init()
    {
        UIManager uiManager = UIManager.Instance;   
        this.skillQuickSlotManager = uiManager.skillQuickSlotManager;
    }

    public async Task CreateSkillTempSlot(Slot slot)
    {
        if (_tempSkillSlot == null)
        {
            // _tempSkillSlot = Instantiate(tempSkillSlot);
            _tempSkillSlot = Instantiate(await AddressableManager.Instance.LoadAsset<GameObject>("TempSkillQuickSlots"));
           
            tempSkillQuickSlotManager = _tempSkillSlot.GetComponent<TempSkillQuickSlotManager>();
            tempSkillQuickSlotManager.SkillQuickSlotManager = skillQuickSlotManager;
            tempSkillQuickSlotManager.SetTempSkillQuickSlot();

            foreach (Transform child in _tempSkillSlot.transform)
            {
                SkillTempSlotDrag skillDrag = child.gameObject.GetComponent<SkillTempSlotDrag>();
                skillDrag.Init(canvas, dragImage, skillQuickSlotManager);

               SkillTempSlotEvent skillSlotEvent = child.gameObject.GetComponent<SkillTempSlotEvent>();
                skillSlotEvent.Init(skillQuickSlotManager);
            }
            _tempSkillSlot.transform.SetParent(slot.transform);
            _tempSkillSlot.GetComponent<RectTransform>().localPosition = new Vector3(0, -85f, 0);

            _tempSkillSlot.transform.SetParent(bookWindow.transform);
            _tempSkillSlot.transform.SetAsLastSibling();

            ChangeTempSkillSlotText();
        }
        else
        {
            Destroy(_tempSkillSlot);
        }
    }
    public void ChangeTempSkillSlotText()
    {
        if (_tempSkillSlot != null)
        {
            tempSkillQuickSlotManager = _tempSkillSlot.GetComponent<TempSkillQuickSlotManager>();
            for (int i = 0; i < tempSkillQuickSlotManager.skillTempSlots.Length; i++)
            {
                tempSkillQuickSlotManager.skillTempSlots[i].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                    = skillQuickSlotManager.playerSkillSlotText[i].text;
            }
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
