using TMPro;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public class InteractInterface : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 3f;

    private HighlightedObject lastHighlighted;

    public TextMeshProUGUI interactPrompt;
    public KeyCode interactKey = KeyCode.F;

    private void Start()
    {
        interactPrompt.enabled = false;
    }

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        if (lastHighlighted != null)
        {
            lastHighlighted.Highlight(false);
            lastHighlighted = null;
        }
        bool canInteract = false;
        IInteractable interactObj = null;

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {

            if (hitInfo.collider.TryGetComponent<IInteractable>(out interactObj))
            {
                canInteract = true;
            }


            if (hitInfo.collider.TryGetComponent(out HighlightedObject highlight))
            {
                if (lastHighlighted != highlight)
                {
                    if (lastHighlighted != null) lastHighlighted.Highlight(false);
                    highlight.Highlight(true);
                    lastHighlighted = highlight;
                }
            }
            else
            {
                if (lastHighlighted != null)
                {
                    lastHighlighted.Highlight(false);
                    lastHighlighted = null;
                }
            }

            if (canInteract && Input.GetKeyDown(interactKey))
            {
                interactObj.Interact();
                Debug.Log("Interacted with " + hitInfo.collider.name);
            }
        }

        if (interactPrompt != null)
        {
            interactPrompt.enabled = canInteract;
        }
    }
}
