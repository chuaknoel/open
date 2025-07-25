using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 책 Wndow 활성화 기능을 버튼에 할당하는 스크립트입니다.
/// </summary>
public class ShowBook : MonoBehaviour
{
    [SerializeField] private PlayerUIInput playerUIInput;
    Button bookButton;

    public void Init()
    {
        bookButton = GetComponent<Button>();
        bookButton.onClick.AddListener(playerUIInput.OnOpenBookWindow);
        //Logger.Log("Book Window 초기화");
    }
}
