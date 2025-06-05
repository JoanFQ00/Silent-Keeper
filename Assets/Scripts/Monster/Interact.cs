using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float interactRadius;
    [SerializeField] SpriteRenderer iconSR;
    [SerializeField] Sprite icon;
    public LayerMask detectionLayer;
    private MonsterMove monsterMove;
    private Vector2 lastDirBox;

    private void Start()
    {
        if (gameObject.tag == "Player")
            monsterMove = GetComponent<MonsterMove>();

        if (monsterMove != null)
            lastDirBox = monsterMove.GetMoveDir();
        else
            lastDirBox = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.tag == "Player" && !GameManager.Instance.isPaused && !IntroRoomsManager.Instance.roomInAnimation)
        {
            Interacting(gameObject.tag);
        }

        InteractableDetection();
    }

    private void InteractableDetection()
    {
        Collider2D interactableHit = Physics2D.OverlapCircle(transform.position, interactRadius, detectionLayer);

        if (interactableHit != null && gameObject.tag == "Player")
        {
            bool leverOK = false;
            bool otherInLocker = false;

            if (interactableHit.TryGetComponent(out Lever lever))
            {
                leverOK = lever.OnlyKid();
            }

            if (interactableHit.TryGetComponent(out Locker locker))
            {
                otherInLocker = locker.GetObjectContained() != null && locker.GetObjectContained() != gameObject;
            }

            if (!leverOK && !otherInLocker)
            {
                iconSR.sprite = icon;
                Vector3 scale = iconSR.gameObject.transform.localScale;
                scale.x = gameObject.transform.localScale.x < 0.0f ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                iconSR.gameObject.transform.localScale = scale;
            }
            else if (iconSR != null)
                if (iconSR.sprite != null)
                    iconSR.sprite = null;
        }
        else if (iconSR != null)
            if (iconSR.sprite != null)
                iconSR.sprite = null;
    }

    public void Interacting(string playerTag, GameObject objectInteracted = null)
    {
        bool canInteract = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRadius, detectionLayer);
        Collider2D matchedCollider = null;

        if (monsterMove != null)
        {
            Vector2 moveDir = monsterMove.GetMoveDir();

            if (moveDir != Vector2.zero && (moveDir.x == 0 || moveDir.y == 0))
                lastDirBox = moveDir;
        }

        if (playerTag == "Kid" && objectInteracted != null)
        {
            foreach (var col in colliders)
            {
                if (col != null && col.gameObject == objectInteracted)
                {
                    matchedCollider = col;
                    canInteract = true;
                    break;
                }
            }
        }
        else if (playerTag == "Player" && colliders.Length > 0)
        {
            matchedCollider = colliders[0];
            canInteract = true;
        }

        if (canInteract && matchedCollider != null)
        {
            if (matchedCollider.GetComponent<Lever>() != null)
            {
                if (matchedCollider.GetComponent<Lever>().OnlyKid())
                {
                    if (gameObject.tag == "Kid")
                    {
                        matchedCollider.GetComponent<Lever>().Interacted();
                        AudioManager.Instance.PlaySFX("lever");
                        gameObject.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                        KidControl.interactable = null;
                    }
                }
                else
                {
                    if (gameObject.tag == "Player")
                    {
                        matchedCollider.GetComponent<Lever>().Interacted();
                        AudioManager.Instance.PlaySFX("palanca");
                    }
                }
            }
            else if (matchedCollider.GetComponent<DoorBase>() != null && !TransitionCamAnimation.Instance.playersInTransition && gameObject.tag == "Player")
            {
                matchedCollider.GetComponent<DoorBase>().Interacted();
            }
            else if (matchedCollider.GetComponent<Locker>() != null)
            {
                var locker = matchedCollider.GetComponent<Locker>();
                GameObject containedObj = locker.GetObjectContained();

                if (gameObject == containedObj || containedObj == null)
                {
                    if (gameObject == containedObj)
                    {
                        locker.Interacted(gameObject, matchedCollider.gameObject);
                        AudioManager.Instance.PlaySFX("taquilla");
                        if (gameObject.tag == "Kid") KidControl.interactable = null;
                    }
                    else
                    {
                        if (containedObj == null && gameObject.tag == "Kid")
                        {
                            if (gameObject.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
                            {
                                locker.Interacted(gameObject, matchedCollider.gameObject);
                                AudioManager.Instance.PlaySFX("taquilla");
                                KidControl.interactable = null;
                            }
                        }
                        else if (containedObj == null && gameObject.tag == "Player")
                        {
                            if (gameObject.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
                            {
                                locker.Interacted(gameObject, matchedCollider.gameObject);
                                AudioManager.Instance.PlaySFX("taquilla");
                            }
                        }
                    }
                }
            }
            else if (matchedCollider.GetComponent<BoxS>() != null)
            {
                if (gameObject.tag == "Player")
                {
                    matchedCollider.GetComponent<BoxS>().Interacted();
                }
            }
            else if (matchedCollider.GetComponent<TorchBase>() != null)
            {
                if (gameObject.tag == "Player")
                {
                    matchedCollider.GetComponent<TorchBase>().Interacted(gameObject, null);
                    AudioManager.Instance.PlaySFX("fireball");
                }
            }
            else if (matchedCollider.GetComponent<BalloonBox>() != null)
            {
                if (gameObject.tag == "Player")
                    matchedCollider.GetComponent<BalloonBox>().Interacted();
            }
            else if (matchedCollider.GetComponent<MoveBox>() != null)
            {
                if (gameObject.tag == "Player")
                    matchedCollider.GetComponent<MoveBox>().MoverEnDireccion(lastDirBox);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}