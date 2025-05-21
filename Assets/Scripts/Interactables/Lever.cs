using UnityEngine;

public class Lever : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] private Sprite normalLever, leverActivated;
    [SerializeField] private bool onlyKid = false;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;

    public ParticleSystem particleS;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();   

        if (interactable.GetIsActivated())
        {
            spriteRenderer.sprite = leverActivated;
        } 
        else 
        {
            spriteRenderer.sprite = normalLever;
        }
    }

    public void Interacted() 
    {
        interactable.SetKeyActivated(!interactable.GetIsActivated());

        if (interactable.GetIsActivated())
        {
            spriteRenderer.sprite = leverActivated;
        }
        else 
        {
            spriteRenderer.sprite = normalLever;
        }

        if (particleS != null) 
        {
            particleS.Play();
        }
    }

    public bool OnlyKid()
    {
        return onlyKid;
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}