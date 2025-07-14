using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{

    // 슬롯 아웃라인 관리
    public void SlotOutline(bool value)
    {
        SlotEvent slotEvent = GetComponent<SlotEvent>();
        slotEvent.outline.enabled = value;
    }
}
