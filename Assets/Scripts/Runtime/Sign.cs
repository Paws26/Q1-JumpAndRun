using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private LocalizedString dialogueText;

    private bool canInteract = false;
    private InputAction interactAction;

    void Start()
    {
        this.interactAction = InputSystem.actions.FindAction("Attack");
        this.interactAction.performed += ToggleDialogueBox;

        this.dialogueBox.SetActive(false);
        this.canInteract = false;
    }

    private void ToggleDialogueBox(InputAction.CallbackContext context)
    {
        if (!this.canInteract)
        {
            return;
        }
        if (this.dialogueBox.activeInHierarchy)
        {
            this.dialogueBox.SetActive(false);
        } else
        {
            this.dialogueBox.SetActive(true);
            var uiDocument = this.dialogueBox.GetComponent<UIDocument>();
            var label = uiDocument.rootVisualElement.Q<Label>();
            label.text = this.dialogueText.GetLocalizedString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.canInteract = false;
            this.dialogueBox.SetActive(false);
        }
    }
}
