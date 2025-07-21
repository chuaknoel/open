using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public SoundUI soundUI;
    public KeySetting keySetting;

    private void Start()
    {
        soundUI ??= GetComponentInChildren<SoundUI>();
        keySetting ??= GetComponentInChildren<KeySetting>();

        keySetting.Init();
    }
}
