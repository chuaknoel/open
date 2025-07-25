using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 게임의 모든 텍스트 데이터(로컬라이제이션)를 로드하고 관리하는 통합 매니저입니다.
/// 싱글턴 패턴으로 구현되어 어디서든 `LanguageManager.Instance`로 쉽게 접근할 수 있습니다.
/// </summary>
public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// 싱글턴 패턴을 위한 정적 인스턴스입니다.
    /// </summary>
    public static LanguageManager Instance { get; private set; }

    /// <summary>
    /// 현재 선택된 언어의 텍스트만 담는 최종 데이터베이스입니다. (Key: 텍스트 키, Value: 실제 텍스트)
    /// </summary>
    public Dictionary<string, string> TextDB { get; private set; }

    /// <summary>
    /// 현재 설정된 언어의 코드입니다. (예: "ko", "en")
    /// </summary>
    public string CurrentLanguage { get; private set; }

    /// <summary>
    /// 데이터 로딩이 완료되었는지 확인하는 플래그입니다.
    /// </summary>
    public bool IsReady { get; private set; } = false;

    /// <summary>
    /// 게임 내에서 언어가 변경되었을 때, UI 텍스트들이 스스로를 새로고침하도록 알리는 이벤트입니다.
    /// </summary>
    public event Action OnLanguageChanged;

    /// <summary>
    /// 게임 시작 시 한 번만 호출되는 초기화 메소드입니다.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // 기기의 시스템 언어를 확인하여 초기 언어를 설정합니다.
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean:
                CurrentLanguage = "ko";
                break;
            case SystemLanguage.English:
                CurrentLanguage = "en";
                break;
            default:
                CurrentLanguage = "en"; // 지원하지 않는 언어일 경우 기본값
                break;
        }
    }

    public void Init()
    {
        LoadLanguageData();
    }

    /// <summary>
    /// UILocalization.csv 파일을 로드하고, TextDB를 채웁니다.
    /// </summary>
    private void LoadLanguageData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/UILocalization");
        if (textAsset != null)
        {
            TextDB = CSVConverter.LoadLocalization(textAsset.text, CurrentLanguage);

            // 데이터 로딩이 완료되었음을 표시합니다.
            IsReady = true;

            // 모든 리스너에게 "준비 완료 및 언어 변경" 신호를 보냅니다.
            OnLanguageChanged?.Invoke();
        }
    }

    /// <summary>
    /// Key에 해당하는 현지화된 텍스트를 반환합니다.
    /// </summary>
    public string GetText(string key)
    {
        if (!IsReady || string.IsNullOrEmpty(key)) return $"{key}_NotReady";

        if (TextDB.TryGetValue(key, out string value))
        {
            return value;
        }

        return $"{key}_NotFound";
    }

    /// <summary>
    /// 게임의 언어를 변경하는 기능입니다. (예: 옵션 메뉴에서 호출)
    /// </summary>
    public void ChangeLanguage(string langCode)
    {
        if (CurrentLanguage == langCode) return;

        CurrentLanguage = langCode;
        IsReady = false; // 새로운 언어 로딩 시작
        LoadLanguageData(); // 이 함수 마지막에서 OnLanguageChanged가 다시 호출됩니다.
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}