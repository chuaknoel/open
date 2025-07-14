using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// 버튼과 윈도우 쌍을 관리하는 클래스입니다.
/// </summary>
[System.Serializable]
public class ButtonWindowPair
{
    public Button button;
    public BaseWindow window;
}

/// <summary>
/// 버튼 클릭을 관리하는 클래스입니다.
/// </summary>
public class ButtonHandler : MonoBehaviour
{
    private UIManager uiManager;

    [SerializeField] private List<ButtonWindowPair> windowPairs;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GetComponent<UIManager>();

        // 버튼과 윈도우쌍을 연결합니다.
        foreach (var pair in windowPairs)
        {
            var window = pair.window;
            pair.button.onClick.AddListener(() => uiManager.OpenWindow(window));
        }
    }
}
