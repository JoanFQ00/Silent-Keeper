using UnityEngine;

public class Lever : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] private Sprite normalLever, leverActivated;
    [SerializeField] private bool onlyKid = false;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;

    public ParticleSystem particleS;
    private bool isPlayingParticles = false;
    private float particleTimer = 0f;
    private int particlePlayCount = 0;
    [SerializeField] private float intervalBetweenPlays = 1.0f;

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

    void Update()
    {
        if (isPlayingParticles)
        {
            particleTimer += Time.deltaTime;

            if (particleTimer >= intervalBetweenPlays && particlePlayCount == 1)
            {
                particleS.Stop();
                particleS.Play(); 
                particlePlayCount++;
                particleTimer = 0f; 
            }
            else if (particlePlayCount >= 2)
            {
                isPlayingParticles = false; 
            }
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

        if (particleS != null && !isPlayingParticles)
        {
            isPlayingParticles = true;
            particlePlayCount = 0;
            particleTimer = 0f;
            particleS.Play();
            particlePlayCount++;
        }
    }

    public bool OnlyKid()
    {
        return onlyKid;
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}