using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputs : MonoBehaviour
{
    [SerializeField] private NpcDialogueUI dialogueUI;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction nextPageAction;

    private void OnEnable()
    {
        var dialogueMap = inputActions.FindActionMap("Dialogue");
        nextPageAction = dialogueMap.FindAction("Next Page");

        nextPageAction.Enable();
        nextPageAction.performed += OnNextPage;
    }

    private void OnDisable()
    {
        nextPageAction.performed -= OnNextPage;
        nextPageAction.Disable();
    }

    private void OnNextPage(InputAction.CallbackContext context)
    {
        dialogueUI.OnClickContinue();
        Logger.Log("DialogueInputs" + "Next page action triggered.");
    }
}