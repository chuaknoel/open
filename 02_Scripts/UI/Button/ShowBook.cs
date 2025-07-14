using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBook : MonoBehaviour
{
    [SerializeField] private PlayerUIInput playerUIInput;
    Button bookButton;

    private void Start()
    {
        bookButton = GetComponent<Button>();
        bookButton.onClick.AddListener(playerUIInput.OnOpenBookWindow);
    }
}
