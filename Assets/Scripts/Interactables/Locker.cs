using UnityEngine;
using System.Collections.Generic;

public class Locker : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] private Sprite normalLocker, lockerBlocked;
    [SerializeField] private GameObject kidEyes, monsterEyes;

    private bool activated;
    private GameObject _other;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    // 🔥 Lista estática de lockers activos
    public static List<Locker> AllLockers = new List<Locker>();

    private void Awake()
    {
        AllLockers.Add(this);
    }

    private void OnDestroy()
    {
        AllLockers.Remove(this);
    }

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
        if (this.gameObject != thisObject) return;

        activated = !activated;
        _other = other;

        if (activated)
        {
            spriteRenderer.sprite = lockerBlocked;

            _other.GetComponent<Collider2D>().enabled = false;
            lastPosition = _other.transform.position;
            _other.transform.position = this.transform.position;

            originalColor = _other.GetComponent<SpriteRenderer>().color;
            _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

            if (_other.CompareTag("Kid"))
            {
                _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.HiddenInLocker);
                kidEyes.SetActive(true);
                AudioManager.Instance.PlaySFX("entryLocker");
            }
            else if (_other.CompareTag("Player"))
            {
                _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.HiddenInLocker);
                monsterEyes.SetActive(true);
                AudioManager.Instance.PlaySFX("entryLocker");
            }

            _other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            spriteRenderer.sprite = normalLocker;

            _other.transform.position = lastPosition;
            _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            if (_other.CompareTag("Kid"))
            {
                _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                kidEyes.SetActive(false);
                AudioManager.Instance.PlaySFX("exitLocker");
            }
            else if (_other.CompareTag("Player"))
            {
                _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);
                monsterEyes.SetActive(false);
                AudioManager.Instance.PlaySFX("exitLocker");
            }

            Rigidbody2D rb = _other.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            _other.GetComponent<Collider2D>().enabled = true;
            _other.GetComponent<SpriteRenderer>().color = originalColor;

            _other = null;
        }
    }

    public GameObject GetObjectContained()
    {
        return _other;
    }

    public void ForceExit()
    {
        if (_other != null)
        {
            Interacted(_other, gameObject);
        }
    }

    public void Interacted() { }
}
