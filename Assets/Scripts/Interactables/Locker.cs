using UnityEngine;

public class Locker : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] private Sprite normalLocker, lockerBlocked;

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
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                if (_other.gameObject.tag == "Kid")
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.HiddenInLocker);
                else if (_other.gameObject.tag == "Player")
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.HiddenInLocker);

                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {

                //Camera cam = GameObject.FindAnyObjectByType<Camera>();

                //if (cam.GetComponent<CameraFollow>().GetCameraTarget().tag == "Kid" && _other.gameObject.tag == "Kid")
                //{
                //    _other.transform.position = this.transform.position + this.transform.right * 1.2f;
                //    _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                //    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);

                //    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                //    //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                //    _other.GetComponent<SpriteRenderer>().color = originalColor;
                //    _other = null;
                //}
                //else if (cam.GetComponent<CameraFollow>().GetCameraTarget().tag == "Player" && _other.gameObject.tag == "Player")
                //{
                //    _other.transform.position = this.transform.position + this.transform.right * 1.2f;
                //    _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                //    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

                //    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                //    //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                //    _other.GetComponent<SpriteRenderer>().color = originalColor;
                //    _other = null;
                //}

                spriteRenderer.sprite = normalLocker;

                _other.transform.position = lastPosition;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                if (_other.gameObject.tag == "Kid")
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                else if (_other.gameObject.tag == "Player")
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
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