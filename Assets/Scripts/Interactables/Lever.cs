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
    [SerializeField] private AnimationClip circusAnim;

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
        isPlayingParticles = false;
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
        if (onlyKid && !interactable.GetIsActivated())
            interactable.SetKeyActivated(!interactable.GetIsActivated());
        else if (!onlyKid)
            interactable.SetKeyActivated(!interactable.GetIsActivated());

        if (interactable.GetIsActivated())
        {
            if (onlyKid)
                OnKidLeverActivated(circusAnim);

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

    private void OnKidLeverActivated(AnimationClip circusAnim)
    {
        GetComponent<RoomDoorsAnimManager>().OnEventChanged();
        IntroRoomsManager.Instance.SetAnimToPlay(circusAnim);
        IntroRoomsManager.Instance.StartRoomAnim();
    }
}