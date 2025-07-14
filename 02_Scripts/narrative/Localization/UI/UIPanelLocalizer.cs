using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIPanelLocalizer : MonoBehaviour
{
    private List<TMP_Text> childTextComponents;

    private void Awake()
    {
        Debug.Log($"[{gameObject.name}] UIPanelLocalizer.Awake() 호출됨: 자식 TextMeshPro 컴포넌트 찾기 시작...");

        childTextComponents = new List<TMP_Text>(GetComponentsInChildren<TMP_Text>(true));

        Debug.Log($"[{gameObject.name}] 총 {childTextComponents.Count}개의 TextMeshPro 컴포넌트를 찾았습니다.");
    }

    private void OnEnable()
    {
        Debug.Log($"[{gameObject.name}] UIPanelLocalizer.OnEnable() 호출됨: LanguageManager 이벤트 구독 시도...");

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += RefreshAllText;
            Debug.Log($"[{gameObject.name}] LanguageManager의 OnLanguageChanged 이벤트 구독 성공.");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] LanguageManager.Instance를 찾을 수 없어 이벤트 구독에 실패했습니다!");
        }

        if (LanguageManager.Instance != null && LanguageManager.Instance.IsReady)
        {
            Debug.Log($"[{gameObject.name}] LanguageManager가 이미 준비되어 있어 즉시 텍스트 새로고침을 실행합니다.");
            RefreshAllText();
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] LanguageManager가 아직 준비되지 않아, 텍스트 새로고침을 대기합니다.");
        }
    }

    private void OnDisable()
    {
        Debug.Log($"[{gameObject.name}] UIPanelLocalizer.OnDisable() 호출됨: LanguageManager 이벤트 구독 취소 시도...");
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= RefreshAllText;
            Debug.Log($"[{gameObject.name}] LanguageManager의 OnLanguageChanged 이벤트 구독을 성공적으로 취소했습니다.");
        }
    }

    private void RefreshAllText()
    {
        Debug.Log($"--- [{gameObject.name}] RefreshAllText() 실행! ---");

        if (childTextComponents == null)
        {
            Debug.LogError("childTextComponents 리스트가 null입니다. Awake 단계에서 문제가 있었을 수 있습니다.");
            return;
        }
        if (LanguageManager.Instance == null || !LanguageManager.Instance.IsReady)
        {
            Debug.LogWarning("LanguageManager가 준비되지 않아 텍스트를 새로고침할 수 없습니다.");
            return;
        }

        // 찾아둔 모든 텍스트 컴포넌트를 순회합니다.
        foreach (var textComponent in childTextComponents)
        {
            // 각 텍스트 컴포넌트의 '게임 오브젝트 이름'을 로컬라이제이션 Key로 사용합니다.
            string key = textComponent.gameObject.name;

            // LanguageManager에 Key를 넘겨 현지화된 텍스트를 받아옵니다.
            string localizedText = LanguageManager.Instance.GetText(key);

            Debug.Log($"자식 오브젝트 '{key}'의 텍스트를 '{localizedText}' (으)로 설정합니다.");

            // UI에 최종 텍스트를 적용합니다.
            textComponent.text = localizedText;
        }

        Debug.Log($"--- [{gameObject.name}] 텍스트 새로고침 완료 ---");
    }
}