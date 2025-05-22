using UnityEngine;

public class DianaBase : MonoBehaviour
{
    private InteractableBase interactable;

    private void Start()
    {
        interactable = GetComponent<InteractableBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Balloon"))
        {
            interactable.SetKeyActivated(true);
        }
    }
}