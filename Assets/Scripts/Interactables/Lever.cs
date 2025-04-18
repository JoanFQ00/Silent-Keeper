using UnityEngine;

public class Lever : MonoBehaviour, IInteractableWithButton
{
    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();   

        if (interactable.GetIsActivated())
        {
            spriteRenderer.color = Color.green;
        } 
        else 
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void Interacted() 
    {
        interactable.SetKeyActivated(!interactable.GetIsActivated());

        if (interactable.GetIsActivated())
        {
            spriteRenderer.color = Color.green;
        } 
        else 
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}