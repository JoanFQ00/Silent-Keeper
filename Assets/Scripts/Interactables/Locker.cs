using UnityEngine;

public class Locker : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] private Sprite normalLocker, lockerBlocked;
    [SerializeField] private GameObject kidEyes, monsterEyes;

    private bool activated;
    private GameObject _other;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;

    private void Start()
    {
        _other = null;
        activated = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        kidEyes.SetActive(false);
        monsterEyes.SetActive(false);
    }

    public void Interacted(GameObject other, GameObject thisObject)
    {
        if (this.gameObject == thisObject)
        {
            activated = !activated;
            _other = other;

            if (activated)
            {
                spriteRenderer.sprite = lockerBlocked;

                _other.GetComponent<Collider2D>().enabled = !activated;
                lastPosition = _other.transform.position;
                _other.transform.position = this.transform.position;
                originalColor = _other.GetComponent<SpriteRenderer>().color;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                if (_other.gameObject.tag == "Kid")
                {
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.HiddenInLocker);
                    kidEyes.SetActive(true);
                    AudioManager.Instance.PlaySFX("entryLocker");
                }
                else if (_other.gameObject.tag == "Player")
                {
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.HiddenInLocker);
                    monsterEyes.SetActive(true);
                    AudioManager.Instance.PlaySFX("entryLocker");
                }

                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                //DESACTIVAR ANIMACIoN OJOS

                spriteRenderer.sprite = normalLocker;

                _other.transform.position = lastPosition;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                if (_other.gameObject.tag == "Kid")
                {
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                    kidEyes.SetActive(false);
                    AudioManager.Instance.PlaySFX("exitLocker");
                }
                else if (_other.gameObject.tag == "Player")
                {
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);
                    monsterEyes.SetActive(false);
                    AudioManager.Instance.PlaySFX("exitLocker");
                }

                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                _other.GetComponent<SpriteRenderer>().color = originalColor;
                _other.GetComponent<Collider2D>().enabled = !activated;
                _other = null;
            }
        }
    }

    public GameObject GetObjectContained()
    {
        return _other;
    }

    public void Interacted() { }
}