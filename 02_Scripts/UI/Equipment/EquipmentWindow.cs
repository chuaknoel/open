using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class EquipmentWindow : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat; 
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private EquipmentSlot[] equipmentSlots = new EquipmentSlot[4];
    [SerializeField] private TextMeshProUGUI[] equipInfoTexts = new TextMeshProUGUI[6];
    [SerializeField] private Button[] increaseButtons = new Button[3]; // 힘,민첩,지능 증가량 버튼
    private bool needToCheckLevelUp = false;


   private void OnEnable()
    {
        UpdateEquipSlot();
       //CheckLevelUp();
    }
    public void Init()
    {
        playerStat = PlayManager.Instance.player.GetStat();

        Logger.Log("장비창 초기화");
    }
    public void UpdateEquipSlot()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            EquipType slotType = equipmentSlots[i].equipSlotType;

            // 같은 장비 타입의 슬롯에 넣어주기
            if (equipmentManager.equippedItems.TryGetValue(slotType, out Item item) && item != null)
            {      
                equipmentSlots[i].SetItem(item);
                equipmentSlots[i].UpdateSlot();
            }
            else
            {
                equipmentSlots[i].ClearSlot();
            }
        }      
    }

    // 장비 능력치 반영 결과 보이기
    public void ShowEquipInfo()
    {
        equipInfoTexts[0].text = playerStat.GetTotalAttack().ToString();
        equipInfoTexts[1].text = playerStat.GetTotalDefence().ToString();
        equipInfoTexts[2].text = playerStat.GetTotalMoveSpeed().ToString();
        equipInfoTexts[3].text = playerStat.GetMaxHealth().ToString();
        equipInfoTexts[4].text = playerStat.GetMaxMana().ToString();
        equipInfoTexts[5].text = playerStat.GetTotalEvasion().ToString();
    }
    // 레벨업시 힘,민첩,마력 증가 버튼 활성화
    private void StatIncreaseButton()
    {
        for(int i = 0;i < increaseButtons.Length;i++)
        {
            Button button = increaseButtons[i];

            button.gameObject.SetActive(needToCheckLevelUp);

            if (button.gameObject.activeSelf)
            {
                // button.onClick.AddListener(playerStat.힘민첩마력 증가 함수);
            }
        }   
    }
}
